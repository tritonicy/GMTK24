using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera")]
    private float xMovement;
    private float yMovement;
    [SerializeField] Transform playerCam;
    [SerializeField] Transform orientation;
    [Header("Move")]
    [SerializeField] float groundDrag;
    [SerializeField] public float runSpeed;
    [Header("Dash")]
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashForce;
    [SerializeField] public float upwardDashSpeed;
    [HideInInspector] public bool isDashing = false;
    [SerializeField] public float dashDuration;
    [SerializeField] public float moveLockTimer;
    private float moveLockTimeCounter;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private bool isMoveSpeedChanged;
    private bool keepMomentum;
    [SerializeField] float speedChangeFactor;
    [Header("Player Components")]
    [HideInInspector] public Rigidbody rb;
    [Header("Jump")]
    [SerializeField] LayerMask groundLayer;
    private float playerStartingHalfHeight;
    private bool readyToJump = true;
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldown;
    [SerializeField] public float airDrag;
    [SerializeField] public float fallMultiplier = 2.5f;
    [SerializeField] public float lowJumpMultiplier = 1.5f;
    [HideInInspector] public bool isMoveLocked;
    [Header("Hidden Variables")]
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public bool isGrounded;
    [Header("Shooting")]
    [SerializeField] GameObject normalProjectilePrefab;
    [SerializeField] GameObject heavyProjectilePrefab;
    [SerializeField] Transform cam;
    [SerializeField] Transform attackPoint;
    private bool readyToShoot = true;
    
    //ziplama updatede calisiyor ilerde fixedupdateye almak gerekebilir.
    private void Start()
    {
        desiredMoveSpeed = runSpeed;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerStartingHalfHeight = this.transform.localScale.y;
    }
    private void Update()
    {        
        GatherInput();
        CheckGrounded();

        if (isGrounded && !isDashing) rb.drag = groundDrag;
        else rb.drag = 0f;

        if(Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) {
            isMoveLocked = true;
            Dash();
        }
        if(Input.GetMouseButton(0) && readyToShoot) {
            Shoot(normalProjectilePrefab, 1);
        }
        if(Input.GetMouseButton(1) && readyToShoot) {
            Shoot(heavyProjectilePrefab, 2);
        }

        if(isDashing) {
            moveLockTimeCounter += Time.deltaTime;
            if(moveLockTimeCounter >= moveLockTimer) {
                moveLockTimeCounter = 0f;
                isMoveLocked = false;
                moveSpeed = runSpeed;
                rb.drag = groundDrag;
            }
        }

    }
    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
        JumpFallControl();
    }
    private void GatherInput()
    {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && readyToJump)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        if(isMoveLocked) return;
        Vector3 moveVector = orientation.forward * yMovement + orientation.right * xMovement;
        if (isGrounded)
        {
            rb.AddForce(moveVector.normalized * moveSpeed * Time.deltaTime * 2f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveVector.normalized * moveSpeed * Time.deltaTime * 2f * airDrag, ForceMode.Force);
        }
    }
    private void Shoot(GameObject projectilePrefab, int timeBetween)
    {
        readyToShoot = false;
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, cam.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDir = cam.transform.forward;
        if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 500f)) {
            forceDir = (hit.point - attackPoint.position).normalized;
        }
        rb.AddForce(forceDir * 20f, ForceMode.Impulse);

        Invoke(nameof(ResetAttack), timeBetween);
    }
    private void ResetAttack()
    {
        readyToShoot = true;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(this.transform.position, Vector3.down, playerStartingHalfHeight + 0.2f, groundLayer);
    }

    private void SpeedControl()
    {
        // movespeed yavas yavas degisirse (lerp) hiz siniri cok erken devreye giriyor o yuzden sorun oluyor.
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if (flatVel.magnitude > moveSpeed * Time.deltaTime / 10)
        {   
            Vector3 limitedvel = flatVel.normalized * moveSpeed * Time.deltaTime / 10;
            rb.velocity = new Vector3(limitedvel.x, rb.velocity.y, limitedvel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
    }
    private void JumpFallControl()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    Vector3 forceToApply;
    private void Dash() {
        keepMomentum = true;
        isDashing = true;
        Vector3 lookDir = CalculateDirection(orientation);
        moveSpeed = dashSpeed;

        forceToApply = lookDir * dashForce + playerCam.up * upwardDashSpeed;
        
        Invoke(nameof(DelayedForceApply), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 CalculateDirection(Transform playerCam)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 returnForce;

        if (verticalInput == 0 && horizontalInput == 0)
        {
            returnForce = playerCam.forward;
        }
        else
        {
            returnForce = playerCam.forward * verticalInput + playerCam.right * horizontalInput;
        }

        return returnForce.normalized;
    }

    private void ResetDash() {
        isDashing = false;
    }
    private void DelayedForceApply() {
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

}
