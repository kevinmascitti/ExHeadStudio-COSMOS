using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{

    StateController controller;
    float attackRange;
    float distanceFromPlayer;
    float chaseRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<StateController>();
        attackRange = controller.GetAttackRange();
        chaseRange = controller.GetChaseRange();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(controller.GetPlayerTransform().position);
        Debug.Log("Attacking");
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
