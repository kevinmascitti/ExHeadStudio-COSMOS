using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemyStates : MonoBehaviour
{
    public enum RangedEnemyStates
    {
        Idle,
        Patrol,
        TakeDistance,
        Aim,
        Attack,
        Reposition,
        Dead
    }

    private Animator rangedEnemyAnimator;
    private RangedEnemyStates rangedEnemyState;
    StateController controller;
    NavMeshAgent agent;
    Transform playerPosition;
    Vector3 playerToEnemyVector;
    Vector3 playerDirection;
    Vector3 escapeDirection;
    [Header("State controls")]
    [SerializeField] float escapeDistance;
    [SerializeField] float aimingDistance;
    private float playerDistance;
    float idleTimer;
    [SerializeField] float idleDuration;
    float patrolTimer;
    [SerializeField] float patrolDuration;
    float aimTimer;
    [SerializeField] float aimDuration; //Da settare uguale alla durata dell'animazione di mira
    float takeDistanceTimer;
    [SerializeField] float takeDistanceDuration;
    [SerializeField] float attackDuration; //Da settare uguale alla durata dell'animazione di attacco
    float attackTimer;
    bool inIdle = false;
    bool inPatrol = false;
    bool inAim = false;
    bool inTakeDistance = false;
    bool inAttack = false;
    private void Awake()
    {
        controller = GetComponent<StateController>();
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rangedEnemyAnimator = GetComponent<Animator>();
        rangedEnemyState = RangedEnemyStates.Idle;
        idleDuration = controller.GetIdleStateDuration();
        patrolDuration = controller.GetPatrollingStateDuration();
        takeDistanceDuration = patrolDuration;
        escapeDistance = controller.GetAttackRange();
        aimingDistance = controller.GetChaseRange();
        //idleDuration = rangedEnemyAnimator.GetComponent<Animation>().GetClip("rig_Enemy_Ranged_Idle").length;
       // patrolTimer = rangedEnemyAnimator.GetComponent<Animation>().GetClip("Nemico_Base_Corsa_Migliorata").length;
        takeDistanceTimer = patrolTimer;
        //attackDuration = rangedEnemyAnimator.GetComponent<Animation>().GetClip("rig_Enemy_Ranged_Attack").length;
    }


    // Update is called once per frame
    void Update()
    {
        if (rangedEnemyAnimator.GetBool("isDead"))
        {
            rangedEnemyState = RangedEnemyStates.Dead;
        }
        if (playerPosition.gameObject.GetComponent<PlayerMovement>().moveDir != null)
            playerDirection = playerPosition.gameObject.GetComponent<PlayerMovement>().moveDir;
        else playerDirection = gameObject.transform.position;
        playerToEnemyVector = playerPosition.position - gameObject.transform.position;
        playerDistance = playerToEnemyVector.magnitude;
        switch (rangedEnemyState)
        {
            case RangedEnemyStates.Idle:
                
                if (!inIdle) ResetIdleControls();
                Idle();
                break;

            case RangedEnemyStates.Patrol:
                if (!inPatrol) ResetPatrolControls();
                Patrol();
                break;

            case RangedEnemyStates.TakeDistance:
                if(!inTakeDistance) ResetTakeDistanceControls();
                
                TakeDistance();
                break;
            case RangedEnemyStates.Aim:
                if (!inAim) ResetAimControls();
                Aim();
                break;
            case RangedEnemyStates.Attack:
                if(!inAttack) ResetAttackControls();

                Attack();
                break;

            case RangedEnemyStates.Reposition:
                Reposition();
                break;
            case RangedEnemyStates.Dead:
                
                break;
            default:

                break;
        }
    }

    private void Idle() //In idle il nemico verifica dove si trova rispetto al player e si comporta di conseguenza
    {
        if (playerDistance > escapeDistance && playerDistance < aimingDistance)
        {
            rangedEnemyState = RangedEnemyStates.Aim;
            inIdle = false;
            return;

        }
        if (playerDistance < escapeDistance)
        {
            rangedEnemyState = RangedEnemyStates.TakeDistance;
            inIdle = false;
            return;
        }
        if (idleTimer > idleDuration)
        {
           
            
                rangedEnemyState = RangedEnemyStates.Patrol;
                inIdle = false;
                return;
        }
        idleTimer += Time.deltaTime;

    }
    private void ResetIdleControls()
    {
        idleTimer = 0f;
        inIdle = true;
        //rangedEnemyAnimator.SetTrigger("isIdle");
        rangedEnemyAnimator.Play("IdleState");

    }
    private void Patrol() //Finché è in patrol, si sposta e si comporta di conseguenza al player
    {

        
        
            if (playerDistance > escapeDistance && playerDistance < aimingDistance)
            {
                rangedEnemyState = RangedEnemyStates.Aim;
                agent.isStopped = true;
                inPatrol = false;
                //aimTarget = new Vector3(playerToEnemyVector.x + playerDirection.x, playerPosition.position.y, playerToEnemyVector.z + playerDirection.z);
                return;
            }
            if (playerDistance < escapeDistance)
            {
                rangedEnemyState = RangedEnemyStates.TakeDistance;
                inPatrol = false;
                return;
            }
            
            
        
        if (patrolTimer > patrolDuration)
        {
            rangedEnemyState = RangedEnemyStates.Idle;
            agent.isStopped = true;
            inPatrol = false;
            return;
        }
        if (agent.remainingDistance <= agent.stoppingDistance )
        {
            agent.SetDestination(controller.ComputeNewDestination());
            //Debug.Log(agent.destination);
        }
        
        patrolTimer += Time.deltaTime;
    }
    private void ResetPatrolControls()
    {
        patrolTimer = 0f;
        agent.isStopped = false;
        agent.SetDestination(controller.ComputeNewDestination());
        inPatrol = true;

        //rangedEnemyAnimator.SetTrigger("isPatroling");
        rangedEnemyAnimator.Play("PatrolState");
    }
    private void TakeDistance()
    {
        if (takeDistanceTimer > takeDistanceDuration || agent.remainingDistance <= agent.stoppingDistance)
        {
            
                if (playerDistance < aimingDistance)
                {
                    rangedEnemyState = RangedEnemyStates.Aim;
                    agent.isStopped = true;
                    inTakeDistance = false;
                    return;
                }
                /*if(playerDistance < escapeDistance)
                {
                    ResetTakeDistanceControls();
                    return;
                }*/
                
                    rangedEnemyState = RangedEnemyStates.Patrol;
                    inTakeDistance = false;
                    return;
                
            
        }
        
        takeDistanceTimer += Time.deltaTime;
        
    }
    private void ResetTakeDistanceControls()
    {
        takeDistanceTimer = 0f;
        inTakeDistance = true;
        Vector3 firstEscapeDirection = new Vector3(transform.position.x - playerToEnemyVector.x/2,transform.position.y, transform.position.z-playerToEnemyVector.z/2);
        if (controller.areaBounds.bounds.Contains(firstEscapeDirection))
            escapeDirection = firstEscapeDirection;
        else if (controller.enemyAreaOfAction.enemyList.Count > 1 && controller.enemyAreaOfAction.GetMeleeEnemyPosition() != Vector3.zero)
        {
            escapeDirection = controller.enemyAreaOfAction.GetMeleeEnemyPosition();
        }
        else escapeDirection = controller.areaBounds.bounds.center;
        agent.isStopped = false;
        agent.SetDestination(escapeDirection);

        //rangedEnemyAnimator.Play("isTakingDistance");
        rangedEnemyAnimator.Play("TakeDistanceState");

    }
    private void Aim() // In aim, enemy si prende del tempo per prendere bene la mira prima di sparare, ma se player si avvicina troppo spara senza pensarci
    {
        if(aimTimer > aimDuration)
        {
            if (playerDistance  <= aimingDistance)
            {
                rangedEnemyState = RangedEnemyStates.Attack;
                inAim = false;
                return;
            }
            
            if (playerDistance > aimingDistance)
            {
                rangedEnemyState = RangedEnemyStates.Patrol; // In questo caso se player è troppo lontano, enemy non lo insegue ma si sposta in un'altra posizione, si può cambiare volendo
                inAim = false;
                return;
            }
        }
        
        
        //Debug.Log();
        agent.transform.LookAt(playerPosition);
        aimTimer += Time.deltaTime;
    }
    private void ResetAimControls()
    {
        aimTimer= 0f;
        inAim = true;

       // rangedEnemyAnimator.SetTrigger("isAiming");
        rangedEnemyAnimator.Play("AimState");
    }
    private void Attack() 
    {
        if (attackTimer > attackDuration || attackTimer >= rangedEnemyAnimator.GetCurrentAnimatorStateInfo(0).length)
        {
            if (playerDistance > escapeDistance && playerDistance <= aimingDistance)
            {
                rangedEnemyState = RangedEnemyStates.Aim;
                inAttack = false;
                return;
            }
            else if(playerDistance < escapeDistance)
            {
                rangedEnemyState = RangedEnemyStates.TakeDistance;
                inAttack = false;
                return;
            }
            rangedEnemyState = RangedEnemyStates.Patrol;
            inAttack = false;
            return;
        }
        attackTimer += Time.deltaTime;
    }
    private void ResetAttackControls()
    {
        if(attackDuration<= 0f)
        {
            attackDuration = rangedEnemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        }
        inAttack = true;
        attackTimer = 0f;

        //rangedEnemyAnimator.SetTrigger("isAttacking");
        rangedEnemyAnimator.Play("AttackState");
    }
    private void Reposition()
    {

    }

    
    
}
