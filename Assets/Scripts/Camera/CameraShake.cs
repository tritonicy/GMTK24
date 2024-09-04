using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    [SerializeField] Camera mainCam;
    private Vector3 originalPos;
    private Animator animController;
    private bool isShaking = false;

    private void Awake() {
        Instance = this;
        animController = GetComponent<Animator>();

        originalPos = this.transform.localPosition;

    }
    public void Shake(float duration, float magnitude) {
        if (!isShaking)
        {
            isShaking = true;
            transform.DOShakePosition(duration, magnitude).OnComplete(() => {
                this.transform.DOLocalMove(originalPos, 0.1f);
                isShaking = false;
            });
            mainCam.DOShakePosition(duration, magnitude);
            animController.SetTrigger("Hurt");
        }

    }
    public void ChangeFov(int endFov) {
        mainCam.DOFieldOfView(endFov, 0.25f);
    }
}
