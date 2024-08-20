using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] public float sens;
    [SerializeField] public Transform orientation;
    private float xRotation;
    private float yRotation;

    private void Start()
    {
        DisableCursor();
    }

    private void Update()
    {
        if(!orientation.GetComponentInParent<PlayerMovement>().isControlsActive) {
            EnableCursor();
            return;
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);
        DisableCursor();
    }
    public void DisableCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EnableCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
