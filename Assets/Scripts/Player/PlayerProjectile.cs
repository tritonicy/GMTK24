using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyProperties>().TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }
}

