using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    public static PlayerProperties Instance;
    public int health;

    private void Start() {
        if(Instance == null) {
            Instance = this;
        }
        else{
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;

        if(health <= 0 ){
            // Debug.Log("died");
        }
    }
}
