using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [Header("First Spawn Area")]
    [SerializeField] public Vector3 centerPoint1;
    [SerializeField] public Vector3 sizeBounds1;
    private Bounds bounds1;
    [Header("Second Spawn Area")]
    [SerializeField] public Vector3 centerPoint2; 
    [SerializeField] public Vector3 sizeBounds2; 
    private Bounds bounds2; 
    [Header("Third Spawn Area")]
    [SerializeField] public Vector3 centerPoint3;
    [SerializeField] public Vector3 sizeBounds3;
    private Bounds bounds3;
    [Header("Spawning objects")]
    [Range(0.01f,1f)] [SerializeField] public float spawnRate;
    [SerializeField] private GameObject[] firstAreaPrefabs;
    [SerializeField] private GameObject[] secondAreaPrefabs;
    [SerializeField] private GameObject[] thirdAreaPrefabs;
    private bool isInstantiating = false;

    private void Awake() {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    private void Start() {
        bounds1 = new Bounds(centerPoint1, sizeBounds1);
        bounds2 = new Bounds(centerPoint2, sizeBounds2);
        bounds3 = new Bounds(centerPoint3, sizeBounds3);
    }
    private void Update() {
        if(bounds1.Contains(playerMovement.transform.position)) {
            // oyuncu 1. dusman spawnlanma bolgesinde
            if(isInstantiating) return;

            Vector3 spawnPoint = Vector3.positiveInfinity;
            while(CheckIfPlaceOccupied(spawnPoint)) {
                spawnPoint = new Vector3(Random.Range(bounds1.min.x, bounds1.max.x),Random.Range(bounds1.min.y, bounds1.max.y), Random.Range(bounds1.min.z, bounds1.max.z));
            }
            int randomInt = Random.Range(0, firstAreaPrefabs.Length);
            SpawnPrefab(firstAreaPrefabs[randomInt], spawnPoint);
            Invoke(nameof(resetInstantiating), 1f / spawnRate);
        }
        else if(bounds2.Contains(playerMovement.transform.position)) {
            // oyuncu 2. dusman spawnlanma bolgesinde
            if (isInstantiating) return;

            Vector3 spawnPoint = Vector3.positiveInfinity;
            while (CheckIfPlaceOccupied(spawnPoint))
            {
                spawnPoint = new Vector3(Random.Range(bounds2.min.x, bounds2.max.x), Random.Range(bounds2.min.y, bounds2.max.y), Random.Range(bounds2.min.z, bounds2.max.z));
            }
            int randomInt = Random.Range(0, secondAreaPrefabs.Length);
            SpawnPrefab(secondAreaPrefabs[randomInt], spawnPoint);
            Invoke(nameof(resetInstantiating), 1f / spawnRate);
        }
        else if (bounds3.Contains(playerMovement.transform.position))
        {
            // oyuncu 3. dusman spawnlanma bolgesinde
            if (isInstantiating) return;

            Vector3 spawnPoint = Vector3.positiveInfinity;
            while (CheckIfPlaceOccupied(spawnPoint))
            {
                spawnPoint = new Vector3(Random.Range(bounds3.min.x, bounds3.max.x), Random.Range(bounds3.min.y, bounds3.max.y), Random.Range(bounds3.min.z, bounds3.max.z));
            }
            int randomInt = Random.Range(0, thirdAreaPrefabs.Length);
            SpawnPrefab(thirdAreaPrefabs[randomInt], spawnPoint);
            Invoke(nameof(resetInstantiating), 1f / spawnRate);
        }

    }
    
    private void OnDrawGizmos() {
        Gizmos.DrawCube(centerPoint1, sizeBounds1);
        Gizmos.DrawCube(centerPoint2, sizeBounds2);
        Gizmos.DrawCube(centerPoint3, sizeBounds3);
    }

    private bool CheckIfPlaceOccupied(Vector3 pos) {
        Collider[] colliders = Physics.OverlapSphere(pos,1f);
        if(colliders.Length == 0) return true;
        return false;
    }
    private GameObject SpawnPrefab(GameObject prefab, Vector3 pos) {
        isInstantiating = true;
        return Instantiate(prefab, pos, Quaternion.identity);
    }
    private void resetInstantiating() {
        isInstantiating = false;
    }


}
