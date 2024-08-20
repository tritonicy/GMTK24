using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [Range(0.001f, 0.08f)] public float amount = 0.002f;
    [Range(1f,100f)] public float frequency = 10f;
    [Range(10f,100f)] public float smooth = 10f;
    Vector3 startPos;

    private void Start() {
        startPos = transform.localPosition;
    }

    private void Update() {
        CheckForHeadbobTrigger();
        StopHeadBob();
    }

    private void CheckForHeadbobTrigger() {
        float inputMagnitude = new Vector3 (Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).magnitude;
        if(inputMagnitude > 0f) {
            StartHeadbob();
        }
    }

    private void StartHeadbob() {
        Vector3 pos = Vector3.zero;

        pos.y += Mathf.Lerp(pos.y, MathF.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, MathF.Cos(Time.time * frequency /2 ) * amount * 1.6f, smooth * Time.deltaTime);
        transform.localPosition += pos;

    }

    private void StopHeadBob() {
        if(transform.localPosition == startPos) return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 1 * Time.deltaTime);
    }
}
