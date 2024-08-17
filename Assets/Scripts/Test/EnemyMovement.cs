using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    private float timeBetweenAttacks = 0f;
    private Transform playerTransform;
    [SerializeField] bool isCloseRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    private bool isAttacking;
    private bool isInAttackRange;
    [SerializeField] private float attackRange;

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

    private void Attack() {
        if(!isAttacking) {
            StopChase();
            isAttacking = true;
            Debug.Log("Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void Chase() {
        // Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, this.transform.localScale.y + 0.2f, groundLayer);
        // Vector3 movePos = new Vector3(playerTransform.transform.position.x, hit.point.y , playerTransform.position.z);
        agent.SetDestination(new Vector3(playerTransform.position.x, this.transform.position.y, playerTransform.position.z));
        Debug.Log("chasing");
    }

    private void ResetAttack() {
        isAttacking = false;
    }
    private void StopChase() {
        agent.SetDestination(this.transform.position);
    }
}