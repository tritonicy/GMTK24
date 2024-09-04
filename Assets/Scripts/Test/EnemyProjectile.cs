using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [SerializeField] public Rigidbody rb;
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.parent.GetComponent<PlayerProperties>().TakeDamage(damage);
            CameraShake.Instance.Shake(0.5f, 0.2f);
        }
        Destroy(this.gameObject);
    }

}
