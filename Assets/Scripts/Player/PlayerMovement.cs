using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float xMovement;
    private float yMovement;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform orientation;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public bool isGrounded;
    [SerializeField] LayerMask groundLayer;
    private float playerStartingHalfHeight;
    [SerializeField] float groundDrag;
    private bool readyToJump = true;
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldown;
    [SerializeField] public float airDrag;
    private float firstSpeed;
    int count = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerStartingHalfHeight = this.transform.localScale.y;
    }
    private void Update()
    {
        // velocity biraz olmasi gerekenin ustune cikiyor sorun olursa limit koy.
        GatherInput();
        CheckGrounded();

        if(isGrounded) rb.drag = groundDrag;
        else rb.drag =  0f;

        firstSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
    }
    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
        Debug.Log(new Vector3(rb.velocity.x,0f,rb.velocity.z).magnitude);

    }
    private void GatherInput()
    {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && readyToJump) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        Vector3 moveVector = orientation.forward * yMovement + orientation.right * xMovement;
        if(isGrounded) {
            rb.AddForce(moveVector.normalized * moveSpeed * Time.deltaTime * 2f, ForceMode.Force);
        }
        else{
            rb.AddForce(moveVector.normalized * moveSpeed * Time.deltaTime * 2f * airDrag , ForceMode.Force);
        }

    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(this.transform.position, Vector3.down, playerStartingHalfHeight + 0.2f, groundLayer);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed * Time.deltaTime / 10)
        {
            Vector3 limitedvel = flatVel.normalized * moveSpeed * Time.deltaTime / 10;
            rb.velocity = new Vector3(limitedvel.x, rb.velocity.y, limitedvel.z);
        }
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(this.transform.up * jumpForce , ForceMode.Impulse);
        Debug.Log(++count);
    }

    private void ResetJump() {
        readyToJump = true;
    }

}
