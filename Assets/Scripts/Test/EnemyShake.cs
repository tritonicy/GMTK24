using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyShake : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        transform.DOShakePosition(duration, magnitude);
    }
}
