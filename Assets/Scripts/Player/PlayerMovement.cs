using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerStartingHalfHeight = this.transform.localScale.y;
    }
    private void Update()
    {
        GatherInput();
        CheckGrounded();

        if(isGrounded) rb.drag = groundDrag;
        else rb.drag = 0f;

        Debug.Log(rb.velocity.magnitude);
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void GatherInput()
    {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        Vector3 moveVector = orientation.forward * yMovement + orientation.right * xMovement;

        rb.AddForce(moveVector.normalized * moveSpeed * Time.deltaTime, ForceMode.Force);
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(this.transform.position, Vector3.down, playerStartingHalfHeight + 0.2f, groundLayer);
    }
}
