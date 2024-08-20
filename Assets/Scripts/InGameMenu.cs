using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] CameraMovement cameraMovement;

    public void ChangeSensivity(float value) {
        cameraMovement.sens = value;
    }

}
