using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointZombieAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent navAgent;
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
}
