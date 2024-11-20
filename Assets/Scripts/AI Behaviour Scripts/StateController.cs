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
    [SerializeField] float attackTime;
    [SerializeField] float repositionTime;
    [SerializeField] Vector3 patrolWayPoint;

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
        patrolWayPoint = ComputeNewDestination();
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
    public Vector3 GetWaypoint()
    {
        return patrolWayPoint;
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

    public float GetRepositionTime()
    {
        return repositionTime;
    }
    public Vector3 ComputeNewDestination()
    {
        Vector3 newPosition = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y, transform.position.z + Random.Range(-2f, 2f));

        return newPosition;
    }
}
