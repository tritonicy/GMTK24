using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    private float timeBetweenAttacks = 1f;
    private Transform playerTransform;
    [SerializeField] bool isCloseRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    private bool isAttacking;
    private bool isInAttackRange;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] public Transform bulletPos;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<PlayerMovement>().gameObject.transform;
    }
    private void Start()
    {
        if (isCloseRange) attackRange = 3f;

        // agent.SetDestination(new Vector3(5f, transform.position.y, 15f));
    }

    private void Update()
    {
        isInAttackRange = Physics.CheckSphere(this.transform.position, attackRange, playerLayer);
        
        if(isInAttackRange) {
            Attack();
        }
        else {
            Chase();
        }
    }

    private void Attack()
    {
        Vector3 distanceVector = playerTransform.position - transform.position;
        Ray ray = new Ray(transform.position, distanceVector);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.gameObject.tag != "Player")
            {
                agent.SetDestination(new Vector3(playerTransform.position.x, this.transform.position.y, playerTransform.position.z));
            }
            else
            {
                if (!isAttacking)
                {
                    StopChase();
                    isAttacking = true;
                    Quaternion rotationVector = GetComponentInChildren<MeshCollider>().gameObject.transform.rotation;
                    Rigidbody rb = Instantiate(projectilePrefab, bulletPos.position , rotationVector).GetComponent<Rigidbody>();

                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
        }
    }
    private void Chase() {
        agent.SetDestination(new Vector3(playerTransform.position.x, this.transform.position.y, playerTransform.position.z));

        // Debug.Log("chasing");
    }

    private void ResetAttack() {
        isAttacking = false;
    }
    private void StopChase() {
        agent.SetDestination(this.transform.position);
    }
}