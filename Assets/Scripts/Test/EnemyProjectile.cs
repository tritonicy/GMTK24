using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] public Rigidbody rb;
    private Vector3 initialDir;
    [SerializeField] int damage;
    PlayerMovement playerMovement;
    private float currentAngle;
    [SerializeField] private float homingSpeed;
    [SerializeField] private float maxHomingAngle;

    private void Start() {
        playerMovement = FindObjectOfType<PlayerMovement>();
        initialDir = playerMovement.transform.position - this.transform.position;
    }
    private void Update() {
        Vector3 sinDir = calcAngle(playerMovement);

        if(currentAngle < maxHomingAngle) {
            rb.AddForce(sinDir * Time.deltaTime * homingSpeed, ForceMode.Impulse);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.parent.GetComponent<PlayerProperties>().TakeDamage(damage);
            CameraShake.Instance.Shake(0.5f, 0.2f);
        }
        Destroy(this.gameObject);
    }

    private Vector3 calcAngle(PlayerMovement player) {
        Vector3 difference = player.transform.position - this.transform.position;

        // Fark vektoru ile ilk ates edilen yon arasindaki aciyi hesaplama
        currentAngle =  Vector3.Angle(initialDir,difference) * Mathf.Deg2Rad;

        // Fark vektörünü, başlangıç yönüne dik olacak şekilde projekte et (cross-product ile değil)
        // difference vektorunden cikarinca difference vektorunun sinusunu veriyor, projectileye gore.
        Vector3 projection = difference - Vector3.Project(difference, initialDir);

        return projection;
    }

}
