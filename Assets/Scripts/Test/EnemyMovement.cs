using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
    }
}
