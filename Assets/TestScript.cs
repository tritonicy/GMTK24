using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.AI.Navigation.Editor;
using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{

    [SerializeField] NavMeshData firstMeshData;
    public static TestScript instance;


    private void Start()
    {
        instance = this;
        GetComponent<NavMeshSurface>().navMeshData = firstMeshData;
        NavMesh.AddNavMeshData(firstMeshData);
    }

    public void changeMeshData(NavMeshData meshData)
    {
        GetComponent<NavMeshSurface>().navMeshData = meshData;
        NavMesh.AddNavMeshData(meshData);
    }


}
