using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] public Rigidbody rb;
    [SerializeField] int damage;
    public float initialBulletSpeed = 4000f;
    public float bulletSpeed = 4000f;
    [SerializeField] public float duration = 0.5f;
    [SerializeField] public float magnitude = 0.2f;

    void Start()
    {
        initialBulletSpeed = bulletSpeed;
        rb.AddForce(transform.up * bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerProperties.Instance.TakeDamage(damage);
            CameraShake.Instance.Shake(duration, magnitude);
        }
        Destroy(this.gameObject);
    }

}
