using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private void Awake() {
        Instance = this;
        Debug.Log(GetComponent<Camera>().fieldOfView);
    }

    public void Shake(float duration, float magnitude) {
        transform.DOShakePosition(duration, magnitude);
    }
    public void ChangeFov(int endFov) {
        GetComponent<Camera>().DOFieldOfView(endFov, 0.25f);
    }
}
