using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] public Rigidbody rb;
    [SerializeField] int damage;

    void Start()
    {
        rb.AddForce(transform.up * 4000f);
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            PlayerProperties.Instance.TakeDamage(damage);
            CameraShake.Instance.Shake(0.1f,0.2f);
        }
        Destroy(this.gameObject);
    }

}
