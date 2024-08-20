using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] public Rigidbody rb;
    [SerializeField] int damage;

    [SerializeField] public float duration = 0.5f;
    [SerializeField] public float magnitude = 0.2f;

    void Start()
    {
        rb.AddForce(transform.up * 4000f);
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            PlayerProperties.Instance.TakeDamage(damage);
            CameraShake.Instance.Shake(duration, magnitude);
        }
        Destroy(this.gameObject);
    }

}
