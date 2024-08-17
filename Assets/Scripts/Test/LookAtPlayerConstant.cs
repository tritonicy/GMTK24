using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LookAtPlayerConstant : MonoBehaviour
{

    [SerializeField] Transform PlayerTransform;

    void Update()
    {
        transform.LookAt(new Vector3(PlayerTransform.position.x,transform.position.y,PlayerTransform.position.z));
    }
}
