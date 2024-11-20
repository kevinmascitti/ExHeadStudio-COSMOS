using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{

    StateController controller;
    float attackRange;
    float distanceFromPlayer;
    float chaseRange;

    float attackTime;
    float attackTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTimer = 0f;
        controller = animator.GetComponent<StateController>();
        attackRange = controller.GetAttackRange();
        chaseRange = controller.GetChaseRange();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(attackTimer >= attackTime)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRepositioning", true);
        }
        animator.transform.LookAt(null ,new Vector3(controller.GetPlayerTransform().position.x, 0f, controller.GetPlayerTransform().position.z));
        distanceFromPlayer = controller.GetDistanceFromPlayer();

        if (distanceFromPlayer > attackRange && distanceFromPlayer < chaseRange)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", true);
        }
        else if(distanceFromPlayer > chaseRange)
        {
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isAttacking", false);
        }
        attackTimer *= Time.deltaTime;
        
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
    
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
