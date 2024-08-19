using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.AI.Navigation.Editor;
using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{

    [SerializeField] NavMeshData meshData;

    private void Awake()
    {

    }
    private void Start()
    {
        GetComponent<NavMeshSurface>().navMeshData = meshData;
        NavMesh.AddNavMeshData(meshData);
    }

}
