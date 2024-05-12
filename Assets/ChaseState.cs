using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    
    private float chaseRange;
    private float attackRange;
    StateController controller;
    NavMeshAgent agent;
    Transform playerPosition;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        controller=animator.GetComponent<StateController>();
        chaseRange = controller.GetChaseRange();
        agent = animator.GetComponent<NavMeshAgent>();
        playerPosition = controller.GetPlayerTransform();
        agent.speed = controller.GetChasingSpeed();
        attackRange = controller.GetAttackRange();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(playerPosition.position);
        float distance = controller.GetDistanceFromPlayer();
        if (distance > chaseRange)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isAttacking", false);
        }
        if (distance <= attackRange)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        agent.SetDestination(animator.transform.position);
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
