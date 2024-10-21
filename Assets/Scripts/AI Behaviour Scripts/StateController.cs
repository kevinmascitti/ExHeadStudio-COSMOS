using System;
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
    [SerializeField] bool isShooter;
    [SerializeField] float shootingRange;
    public Collider areaBounds;
    public int areaID;
    public bool canChase=false;
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
        //patrolWayPoint = ComputeNewDestination();
        animator = GetComponent<Animator>();
        AIArea.OnPlayerEnter += CanChase;
        AIArea.OnPlayerExit += StopChasing;
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

    public bool GetIsShooter()
    {
        return isShooter;
    }
    public Vector3 ComputeNewDestination()
    {
        if (areaBounds != null)
        {
            Vector3 newPosition = new Vector3(UnityEngine.Random.Range(areaBounds.bounds.min.x, areaBounds.bounds.max.x), transform.position.y, UnityEngine.Random.Range(areaBounds.bounds.min.z, areaBounds.bounds.max.z));
            return newPosition;
        }

        return transform.position;
    }
    public void SetAreaBounds(Collider aB)
    {
        areaBounds = aB;
        //Debug.Log(gameObject.name + ", " + aB.gameObject.name);
    }
    private void StopChasing(object sender, OnPlayerArg e)
    {
        if(e.areaId == areaID)
        {
            canChase = false;
        }
    }
    private void CanChase(object sender, OnPlayerArg e)
    {
        if(e.areaId == areaID && !canChase)
        {
            canChase = true;
        }
    }

    private void OnDestroy()
    {
        AIArea.OnPlayerEnter -= CanChase;
        AIArea.OnPlayerExit -= StopChasing;
    }
}
