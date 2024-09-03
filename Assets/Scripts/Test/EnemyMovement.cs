using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    private float timeBetweenAttacks = 1f;
    private Transform camTransform;
    private Transform playerTransform;
    [SerializeField] bool isCloseRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    private bool isAttacking;
    private bool isInRunningAwayRange;
    private bool isInRunningAway = false;
    private float runAwayRange = 0f;
    private bool isInAttackRange;
    [SerializeField] private float attackRange;
    private float initialAttackRange;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] public Transform bulletPos;
    [SerializeField] float bulletSpeed = 100;
    private float initialBulletSpeed;
    private Vector3 newMovePos;
    int enemyLayer = 1 << 10;
    int projectileLayer = 1 << 12;
    int excludeLayer;

    private void Awake()
    {
        excludeLayer = projectileLayer | enemyLayer;
        agent = GetComponent<NavMeshAgent>();
        camTransform = GameObject.Find("Orientation").transform;
        playerTransform = FindObjectOfType<PlayerMovement>().gameObject.transform;
    }
    private void Start()
    {
        if (isCloseRange) attackRange = 2f;
        else runAwayRange = attackRange / 2;

        initialAttackRange = attackRange;
        initialBulletSpeed = bulletSpeed;
        GrowAttackRange();
        GrowShotSpeed();
    }

    private void Update()
    {
        isInRunningAwayRange = Physics.CheckSphere(this.transform.position, runAwayRange, playerLayer);
        isInAttackRange = Physics.CheckSphere(this.transform.position, attackRange, playerLayer);

        if (isInRunningAwayRange && !isInRunningAway)
        {
            RunAway();
        }
        else if (isInAttackRange)
        {
            Attack();
        }
        else
        {
            Chase();
            Attack();
        }
    }
        private void RunAway()
    {
        Vector3 differenceVector = new Vector3(this.transform.position.x - playerTransform.position.x, this.transform.position.y, this.transform.position.z - playerTransform.position.z).normalized * (runAwayRange + 1);
        bool isValidMove = false;
        newMovePos = new Vector3(999f, 999f, 999f);
        while (!isValidMove)
        {
            newMovePos = new Vector3(this.transform.position.x + differenceVector.x +
                Random.Range(-runAwayRange, runAwayRange), this.transform.position.y, this.transform.position.z + differenceVector.z + Random.Range(-runAwayRange, runAwayRange));

            isValidMove = !Physics.CheckSphere(newMovePos, runAwayRange, playerLayer);
        }
        agent.SetDestination(newMovePos);
        isInRunningAway = true;
    }

    private void Attack()
    {
        if(isCloseRange) {
            agent.SetDestination(new Vector3(playerTransform.position.x, this.transform.position.y, playerTransform.position.z));
            if(isInAttackRange) {
                this.gameObject.GetComponentInChildren<EnemyShake>().Shake(0.1f,0.2f);
                playerTransform.GetComponent<PlayerProperties>().TakeDamage(10);
                GameObject.Find("Hand").GetComponent<CameraShake>().Shake(0.2f,0.4f);
                Destroy(this.gameObject);
            }
            return;
        }

        if (isInRunningAway)
        {
            Invoke(nameof(ResetRunAway), 1f);
        }

        Vector3 distanceVector = camTransform.position - transform.position;
        Ray ray = new Ray(transform.position, distanceVector);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity,~excludeLayer))
        {
            if (raycastHit.collider.gameObject.tag != "Player")
            {
                agent.SetDestination(new Vector3(playerTransform.position.x, this.transform.position.y, playerTransform.position.z));
                Debug.Log((raycastHit.collider.gameObject.name));
            }
            else
            {
                if (!isAttacking)
                {
                    StopChase();
                    isAttacking = true;
                    Quaternion rotationVector = GetComponentInChildren<MeshCollider>().gameObject.transform.rotation;
                    GameObject bullet = Instantiate(projectilePrefab, bulletPos.position, Quaternion.identity);
                    Vector3 shotVector = playerTransform.position - bulletPos.position; 
                    bullet.GetComponent<Rigidbody>().AddForce(shotVector.normalized * bulletSpeed, ForceMode.Impulse);

                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }
        }
    }


    private void Chase()
    {
        agent.SetDestination(new Vector3(playerTransform.position.x, this.transform.position.y, playerTransform.position.z));
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
    private void StopChase()
    {
        agent.SetDestination(this.transform.position);
    }
    private void ResetRunAway()
    {
        isInRunningAway = false;
    }

    public void GrowAttackRange()
    {
        if (FindObjectOfType<PlayerProperties>().newScale.y == 0f)
        {
            attackRange = initialAttackRange;
        }
        else
        {
            attackRange = initialAttackRange * FindObjectOfType<PlayerProperties>().newScale.y;
        }
    }
    public void GrowShotSpeed() {
        if (FindObjectOfType<PlayerProperties>().newScale.y == 0f)
        {
            bulletSpeed = initialBulletSpeed;
        }
        else
        {
            bulletSpeed = initialBulletSpeed * FindObjectOfType<PlayerProperties>().newScale.y;
        }
    }
}