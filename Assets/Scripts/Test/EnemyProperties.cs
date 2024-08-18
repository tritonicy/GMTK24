using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public int health;

    public void TakeDamage(int damage) {
        health -= damage;
        if(health <= 0 ) {
            Debug.Log("killed an enemy");
        }
    }
}
