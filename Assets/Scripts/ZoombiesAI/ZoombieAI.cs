using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZoombieAI : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public enum ZombieState { Idle, Chase, Attack, Dead }
    public Animator animator;
    public ZombieState currentState = ZombieState.Idle;
    public Transform player;
    public float ChaseDistance = 10f;
    public float attackDistance = 2f;
    public float attackCooldown = 2f;
    public float attackDelay = 1.5f;
    public int damage = 10;
    public int health = 100;
    private CapsuleCollider capsuleCollider;
    private bool isAttacking;
    private float lastAttackTime;
    public GameObject bloodScreenEffect;
    private GameObject instantiatedObject;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
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
        switch (currentState)
        {
            case ZombieState.Idle:
                animator.SetBool("iswalking", false);
                animator.SetBool("isattacking", false);
                
                if (Vector3.Distance(transform.position, player.position) <= ChaseDistance)
                    currentState = ZombieState.Chase;
                break;

            case ZombieState.Chase:
                animator.SetBool("iswalking", true);
                animator.SetBool("isattacking", false);

                navAgent.SetDestination(player.position);
                    if (Vector3.Distance(transform.position, player.position) <= attackDistance)
                    currentState = ZombieState.Attack;
                break;
            case ZombieState.Attack:
                animator.SetBool("isattacking", true);
                
                navAgent.SetDestination(transform.position);
                if(!isAttacking && Time.time - lastAttackTime >= attackCooldown)
                {
                    StartCoroutine(AttackwithDelay());
                    Debug.Log("Attack Player");
                    StartCoroutine(ActivateBloodScreenEffect());
                }
                
                if (Vector3.Distance(transform.position, player.position) > attackDistance)
                    currentState = ZombieState.Chase;
                break;

            case ZombieState.Dead:
                animator.SetBool("iswalking", false);
                animator.SetBool("isattacking", false);
                animator.SetBool("isdead", true);
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

    private IEnumerator AttackwithDelay()
    {
        //damage the player
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if(playerMovement != null)
        {
            playerMovement.TakeDamage(damage);
        }
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        lastAttackTime = Time.time;
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
        if(health <= 0)
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
        if(instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            instantiatedObject = null;
        }
    }

}
