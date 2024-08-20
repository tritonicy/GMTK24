using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public int health;
    [SerializeField] GameObject experienceItemPrefab;

    public void TakeDamage(int damage) {
        health -= damage;
        Debug.Log("Dusmana vurdun");
        if(health <= 0 ) {
            KillYourself();
        }
    }
    public void KillYourself() {
        GameObject droppedItem = Instantiate(experienceItemPrefab, this.transform.position + new Vector3(0f,1f,0f), Quaternion.identity);

        Destroy(this.transform.parent.gameObject);
    }
}
