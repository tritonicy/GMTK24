using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    [SerializeField] Camera mainCam;

    private Animator animController;

    private void Awake() {
        Instance = this;
        animController = GetComponent<Animator>();

    }
    public void Shake(float duration, float magnitude) {
        transform.DOShakePosition(duration, magnitude);
        mainCam.DOShakePosition(duration, magnitude);
        animController.SetTrigger("Hurt");
    }
    public void ChangeFov(int endFov) {
        mainCam.DOFieldOfView(endFov, 0.25f);
    }
}
