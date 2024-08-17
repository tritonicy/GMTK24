using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform cameraPos;

    private void Update() {
        this.transform.position = cameraPos.position;
    }
}
