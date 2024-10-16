using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{

    StateController controller;
    float attackRange;
    float distanceFromPlayer;
    float chaseRange;
    Transform playerTransform;
    float attackTime;
    float attackTimer;
    Transform enemyTransform;
    public static EventHandler<EventArgs> EnemyShootEvent;
    bool canChase;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<StateController>();
        canChase = controller.canChase;
        attackTimer = 0f;
        attackRange = controller.GetAttackRange();
        chaseRange = controller.GetChaseRange();
        enemyTransform = controller.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = controller.GetPlayerTransform();

        
        //animator.transform.LookAt( playerTransform, new Vector3(playerTransform.position.x, 0f, playerTransform.position.z));
        if (attackTimer >= attackTime)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRepositioning", true);
        }
        controller.transform.LookAt(null ,new Vector3(playerTransform.position.x, 0f, playerTransform.position.z));
        distanceFromPlayer = controller.GetDistanceFromPlayer();

        if (distanceFromPlayer > attackRange && canChase)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isPatrolling", false);
        }
        else 
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isAttacking", false);
        }
        attackTimer *= Time.deltaTime;
        
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyTransform.gameObject.tag.Equals("ShootingEnemy"))
        {
            EnemyShootEvent?.Invoke(this, EventArgs.Empty); //DA METTERE NELL'ANIMAZIONE VERA E PROPRIA
    
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
