using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] public Rigidbody rb;
    [SerializeField] int damage;

    void Start()
    {
        rb.AddForce(transform.up * 2000f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerProperties.Instance.TakeDamage(damage);
        }
        
        Destroy(this.gameObject);
    }

}
