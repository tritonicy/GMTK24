using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    private void Awake() {
        Instance = this;
    }
    public void Shake(float duration, float magnitude) {
        transform.DOShakePosition(duration, magnitude);
    }
    public void ChangeFov(int endFov) {
        GetComponentInParent<Camera>().DOFieldOfView(endFov, 0.25f);
    }
}
