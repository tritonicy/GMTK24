using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private int damage;

    public void setDamage(int damage) {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyProperties>().TakeDamage(damage);
            other.gameObject.GetComponent<EnemyShake>().Shake(0.1f,0.2f);
        }
        Destroy(this.gameObject);
    }
}

