using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionState : StateMachineBehaviour
{

    private StateController controller;

    private float repositionTime;
    private float attackRange;
    private float chaseRange;

    private float repositionTimer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<StateController>();
        repositionTime = controller.GetRepositionTime();
        attackRange = controller.GetAttackRange();  
        chaseRange = controller.GetChaseRange();
        repositionTimer = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance=controller.GetDistanceFromPlayer();
        if (repositionTimer >= repositionTime)
        {
            animator.SetBool("isRepositioning", false);

            if (distance <= attackRange)
            {
                animator.SetBool("isAttacking", true);
            }
            else if (distance <= chaseRange)
            {
                animator.SetBool("isChasing", true);
            }
            else animator.SetBool("isPatrolling", true);
        }
        repositionTimer += Time.deltaTime;

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