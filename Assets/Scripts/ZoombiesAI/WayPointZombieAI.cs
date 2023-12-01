using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointZombieAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public enum ZombieState { Walk, Chase, Attack, Dead }
    public Animator animator;
    public ZombieState currentState = ZombieState.Walk;
    public Transform player;
    public float ChaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public float attackDelay = 1.5f;
    public int damage = 10;
    public int health = 100;
    private CapsuleCollider capsuleCollider;
    private bool isAttacking;
    private bool isMoving = false;
    private float lastAttackTime;
    public GameObject bloodScreenEffect;
    private GameObject instantiatedObject;


    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if(playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.Log("Player object not found");
        }
        capsuleCollider = GetComponent<CapsuleCollider>();
        navAgent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch(currentState)
        {
            case ZombieState.Walk:
                if(!isMoving || navAgent.remainingDistance < 0.1f)
                {
                    Walk();
                }
                if (IsPlayerInRange(ChaseDistance))
                    currentState = ZombieState.Chase;
                    break;
           
            case ZombieState.Chase:
                ChasePlayer();
                if (IsPlayerInRange(attackDelay))
                    currentState = ZombieState.Attack;
                    break;
            case ZombieState.Attack:
                AttackPlayer();
                if (!IsPlayerInRange(attackDelay))
                    currentState = ZombieState.Chase;
                break;

            case ZombieState.Dead:
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDead", true);
                //  animator.SetBool("iswalking", false);
                //  animator.SetBool("isattacking", false);
                //   animator.SetBool("isdead", true);
                //animations
                navAgent.enabled = false;
                capsuleCollider.enabled = false;
                enabled = false;
                //incraese score
                GameManager.instance.currentScore += 1;

                Debug.Log("Dead");
                break;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }

    private void Walk()
    {
        navAgent.speed = 0.3f;
        Vector3 randomPosition = RandomNavMeshPosition();
        navAgent.SetDestination(randomPosition);
        isMoving = true;
    }

    private Vector3 RandomNavMeshPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
        return hit.position;
    }

    private void ChasePlayer()
    {
        navAgent.speed = 3f;
        //animations
        animator.SetBool("isChasing", true);
        animator.SetBool("isAttacking", false);
        navAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //animations
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttacking", true);
        navAgent.SetDestination(transform.position);
        if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            StartCoroutine(AttackWithDelay());
            StartCoroutine(ActivateBloodScreenEffect());
            lastAttackTime = Time.time;

            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(damage);
            }
        }

    }

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    private IEnumerator ActivateBloodScreenEffect()
    {
        InstantiateObject();
        yield return new WaitForSeconds(attackDelay);
        DeletObject();
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentState == ZombieState.Dead)
            return;
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    private void Die()
    {
        currentState = ZombieState.Dead;
    }

    void InstantiateObject()
    {
        instantiatedObject = Instantiate(bloodScreenEffect);
    }

    void DeletObject()
    {
        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            instantiatedObject = null;
        }
    }
}
