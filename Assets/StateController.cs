using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [Header("Controlli stati AI")]
    [SerializeField] private float chaseRange;
    [SerializeField] float attackRange;
    [SerializeField] private float idleStateDuration;
    [SerializeField] private float patrollingStateDuration;
    [SerializeField] float enemyChasingSpeed;
    [SerializeField] float enemyPatrollingSpeed;
    [SerializeField] List<Transform> patrolWayPoints = new List<Transform>();

    /*
    ChaseState chaseState;
    IdleState idleState;
    PatrolState patrolState; 
    */
    Animator animator;
    float distanceFromPlayer;
    Transform playerTransform;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(playerTransform.position, animator.transform.position);
        

        //Debug.Log(distanceFromPlayer);
    }

    public float GetChaseRange()
    {
        return chaseRange;
    }
    public float GetIdleStateDuration()
    {
        return idleStateDuration;
    }
    public float GetPatrollingStateDuration()
    {
        return patrollingStateDuration;
    }
    public List<Transform> GetWaypoints()
    {
        return patrolWayPoints;
    }
    public float GetDistanceFromPlayer()
    {
        return distanceFromPlayer;
    }
    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }
    public float GetChasingSpeed()
    {
        return enemyChasingSpeed;
    }
    public float GetPatrollingSpeed()
    {
        return enemyPatrollingSpeed;
    }
    public float GetAttackRange()
    {
        return attackRange;
    }
}
