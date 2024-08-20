using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageTrigger : MonoBehaviour
{

    [SerializeField] bool DeadZone;
    [SerializeField] GameObject playerPivot;
    [SerializeField] float RequiredHeight;
    [SerializeField] NavMeshData meshData;

    private void OnTriggerEnter(Collider other)
    {
        if (DeadZone && other.CompareTag("Player"))
        {
            TestScript.instance.changeMeshData(meshData);
            if (playerPivot.transform.localScale.y < RequiredHeight)
            {
                //Kill the player
            }
        }
        else if (!DeadZone && other.CompareTag("Player"))
        {
            TestScript.instance.changeMeshData(meshData);
        }
    }

}
