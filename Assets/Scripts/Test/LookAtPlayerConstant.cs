using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LookAtPlayerConstant : MonoBehaviour
{
    private Transform camTransform;

    private void Start()
    {
        camTransform = GameObject.Find("CameraPos").transform;
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x + 90f, this.transform.rotation.y, this.transform.rotation.z);
    }
    void Update()
    {
        transform.LookAt(camTransform);
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x + 90,
            transform.eulerAngles.y,
            transform.eulerAngles.z
);

    }
}
