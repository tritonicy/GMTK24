using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
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
    private float initialRunSpeed;
    [SerializeField] public float runSpeed;
    [Header("Dash")]
    [SerializeField] public float dashSpeed;
    private float initialDashSpeed;
    [SerializeField] public float dashForce;
    private float initialDashForce;
    [SerializeField] public float upwardDashSpeed;
    [HideInInspector] public bool isDashing = false;
    [SerializeField] public float dashDuration;
    [SerializeField] public float moveLockTimer;
    private float moveLockTimeCounter;
    [SerializeField] float speedChangeFactor;
    [Header("Player Components")]
    [HideInInspector] public Rigidbody rb;
    [Header("Jump")]
    [SerializeField] LayerMask groundLayer;
    private float initialHalfHeight;
    private float halfHeight;
    [SerializeField] public float jumpForce;
    private float initialJumpForce;
    [SerializeField] public float jumpCooldown;
    [SerializeField] public float airDrag;
    private float initialAirDrag;
    [SerializeField] public float fallMultiplier = 2.5f;
    private float initialFallMultiplier;
    [SerializeField] public float lowJumpMultiplier = 1.5f;
    private float initialLowJumpMultiplier;
    [HideInInspector] public bool isMoveLocked;
    private float coyoteTimer = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTimer = 0.2f;
    private float jumpBufferTimeCounter;

    [Header("Hidden Variables")]
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public bool isGrounded;
    private int playerLayer = 1 << 8;
    [Header("Shooting")]
    [SerializeField] GameObject normalProjectilePrefab;
    [SerializeField] GameObject heavyProjectilePrefab;
    [SerializeField] Transform cam;
    [SerializeField] Transform attackPoint;
    [SerializeField] float shotSpeed = 100f;
    private float initialShotSpeed;
    private bool readyToShoot = true;
    [SerializeField] float timeBetweenNormal;
    [SerializeField] float timeBetweenHeavy;
    [SerializeField] public Vector3 normalBulletScale = new Vector3(0.1f,0.1f,0.1f);
    [SerializeField] public int normalBulletDamage = 20;
    [SerializeField] public int heavyBulletDamage = 40;
    private Vector3 initialNormalBulletScale;
    [SerializeField] Vector3 HeavyBulletScale = new Vector3(0.4f,0.4f,0.4f);
    [Header("Other")]
    [SerializeField] public GameManager gameManager;
    [SerializeField] public GameObject panel;
    public bool isControlsActive = true;
    [SerializeField] Animator animController;
    [SerializeField] Animator playerAnimator;
    //ziplama updatede calisiyor ilerde fixedupdateye almak gerekebilir.
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        playerLayer = ~playerLayer;
        initialRunSpeed = runSpeed;
        initialDashSpeed = dashSpeed;
        initialDashForce = dashForce;
        initialJumpForce = jumpForce;
        initialAirDrag = airDrag;
        initialFallMultiplier = fallMultiplier;
        initialLowJumpMultiplier = lowJumpMultiplier;
        initialNormalBulletScale = normalBulletScale;
        initialShotSpeed = shotSpeed;
        halfHeight = GameObject.Find("PlayerObj").transform.localScale.y;
        initialHalfHeight = halfHeight;
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        gameManager.OnMenuOpen += HandleMenu;
    }
    private void Update()
    {   
        GatherInput();
        CheckGrounded();
        // walking check
        if((rb.velocity.x > 0.1f || rb.velocity.z > 0.1f) && isGrounded) {
            playerAnimator.SetBool("isWalking", true);
        }
        else playerAnimator.SetBool("isWalking", false);
        
        // havadaysa drag ekle
        if (isGrounded && !isDashing) rb.drag = groundDrag;
        else rb.drag = 0f;

        //normal ve heavy attack
        if(Input.GetMouseButton(0) && readyToShoot) {
            Shoot(normalProjectilePrefab, timeBetweenNormal, normalBulletScale,normalBulletDamage);
        }
        if(Input.GetMouseButton(1) && readyToShoot) {
            Shoot(heavyProjectilePrefab, timeBetweenHeavy, normalBulletScale, heavyBulletDamage);
        }

        // dash atarken movement kitleme
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isDashing) {
            Dash();
        }
        // dash bitirme
        if(isDashing) {
            moveLockTimeCounter += Time.deltaTime;
            if(moveLockTimeCounter >= moveLockTimer) {
                moveLockTimeCounter = 0f;
                CameraShake.Instance.ChangeFov(60);
                isDashing = false;
                StartCoroutine(nameof(LerpDashToRunSpeed));
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
        if(!isControlsActive) return;
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");


        if(isGrounded) {
            coyoteTimeCounter = coyoteTimer;
        }
        else{
            coyoteTimeCounter -= Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            jumpBufferTimeCounter = jumpBufferTimer;
        }
        else {
            jumpBufferTimeCounter -= Time.deltaTime;
        }
        if (jumpBufferTimeCounter > 0 && coyoteTimeCounter > 0)
        {
            Jump();
            jumpBufferTimeCounter = 0f;
        }
    }

    private void MovePlayer()
    {
        if(isMoveLocked) return;
        Vector3 moveVector = orientation.forward * yMovement + orientation.right * xMovement;
        if (isGrounded)
        {
            rb.AddForce(moveVector.normalized * moveSpeed * Time.deltaTime, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveVector.normalized * moveSpeed * Time.deltaTime * airDrag, ForceMode.Force);
        }
    }
    private void Shoot(GameObject projectilePrefab, float timeBetween, Vector3 initialBulletScale, int projectileDamage)
    {
        if(!isControlsActive) return;
        readyToShoot = false;
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, cam.rotation);
        SFXManager.PlaySound(SoundType.PlayerAttack);
        projectile.GetComponent<Transform>().localScale = initialBulletScale;
        projectile.GetComponent<PlayerProjectile>().setDamage(projectileDamage);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDir = cam.transform.forward;
        if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 500f, playerLayer)) {
            forceDir = (hit.point - attackPoint.position).normalized;
        }
        rb.AddForce(forceDir * shotSpeed, ForceMode.Impulse);
        animController.SetTrigger("Shoot");

        Invoke(nameof(ResetAttack), timeBetween);
    }
    private void ResetAttack()
    {
        readyToShoot = true;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(this.transform.position, Vector3.down, halfHeight + 0.2f, groundLayer);
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
            coyoteTimeCounter = 0f;
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    Vector3 forceToApply;
    private void Dash() {
        isDashing = true;
        Vector3 lookDir = CalculateDirection(orientation);
        moveSpeed = dashSpeed;

        forceToApply = lookDir * dashForce + playerCam.up * upwardDashSpeed;
        CameraShake.Instance.ChangeFov(75);

        int randInt = UnityEngine.Random.Range(1, 3);
        switch (randInt)
        {
            case 1:
                SFXManager.PlaySound(SoundType.Dash1);
                break;
            case 2:
                SFXManager.PlaySound(SoundType.Dash2);
                break;
        }
        // rb.AddForce(forceToApply,ForceMode.Impulse);
        Invoke(nameof(DelayedForceApply), 0.025f);
        // Invoke(nameof(ResetDash), dashDuration);
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

    public void GrowBulletscale()
    {
        normalBulletScale = initialNormalBulletScale * GetComponent<PlayerProperties>().newScale.y;
    }

    public void GrowSpeed() {
        runSpeed = initialRunSpeed * GetComponent<PlayerProperties>().newScale.y;
    }
    public void GrowDashSpeed() {
        dashSpeed = initialDashSpeed * GetComponent<PlayerProperties>().newScale.y;
        dashForce = initialDashForce * GetComponent<PlayerProperties>().newScale.y;
    }

    public void GrowJumpSpeed() {
        jumpForce = initialJumpForce * GetComponent<PlayerProperties>().newScale.y;
        airDrag = initialAirDrag * GetComponent<PlayerProperties>().newScale.y;
        fallMultiplier = initialFallMultiplier * GetComponent<PlayerProperties>().newScale.y;
        lowJumpMultiplier = initialLowJumpMultiplier * GetComponent<PlayerProperties>().newScale.y;
    }
    public void GrowHeight() {
        halfHeight = initialHalfHeight * GetComponent<PlayerProperties>().newScale.y;
    }

    public void GrowShotSpeed() {
        shotSpeed = initialShotSpeed * GetComponent<PlayerProperties>().newScale.y;
    }

    public void HandleMenu() {
        if(panel.gameObject.activeSelf) {
            panel.gameObject.SetActive(false);
            EnableControls();
        }
        else{
            panel.gameObject.SetActive(true);
            DisableControls();
        }
    }
    public void DisableControls() {
        isControlsActive = false;
    }
    public void EnableControls() {
        isControlsActive = true;
    }

    public IEnumerator LerpDashToRunSpeed() {
        float time = 0f;
        float difference = Math.Abs(dashSpeed - runSpeed);
        float startVal = dashSpeed;

        while(time < difference) {
            moveSpeed = Mathf.Lerp(startVal, runSpeed, time / difference);
            time += Time.deltaTime * 16666f;

            yield return null;
        }
        moveSpeed = runSpeed;
    }

    public void Walk() {
        int randInt = UnityEngine.Random.Range(1,4);
        switch(randInt) {
            case 1:
                SFXManager.PlaySound(SoundType.Walk1, 0.4f);
                break;
            case 2:
                SFXManager.PlaySound(SoundType.Walk2, 0.4f);
                break;
            case 3:
                SFXManager.PlaySound(SoundType.Walk3, 0.4f);
                break;
        }
    }

}
