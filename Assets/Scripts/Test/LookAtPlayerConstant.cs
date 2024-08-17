using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LookAtPlayerConstant : MonoBehaviour
{

    [SerializeField] Transform PlayerTransform;

    private void Start() {
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x + 90f, this.transform.rotation.y,this.transform.rotation.z);
    }
    void Update()
    {
        transform.LookAt(PlayerTransform);
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x + 90,
            transform.eulerAngles.y,
            transform.eulerAngles.z
);
        
    }
}
