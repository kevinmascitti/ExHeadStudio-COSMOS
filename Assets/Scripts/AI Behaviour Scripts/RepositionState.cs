using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionState : StateMachineBehaviour
{

    private StateController controller;

    private float repositionTime;
    private float attackRange;
    private float chaseRange;
    private Transform playerTransform;
    private float repositionTimer;
    bool canChase;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<StateController>();
        canChase= controller.canChase;
        repositionTime = controller.GetRepositionTime();
        attackRange = controller.GetAttackRange();  
        chaseRange = controller.GetChaseRange();
        repositionTimer = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = controller.GetPlayerTransform();
        controller.gameObject.transform.LookAt(playerTransform.position);
        float distance=controller.GetDistanceFromPlayer();
        if (repositionTimer >= repositionTime)
        {
            animator.SetBool("isRepositioning", false);

            if (distance <= attackRange)
            {
                animator.SetBool("isAttacking", true);
            }
            else if (canChase)
            {
                animator.SetBool("isChasing", true);
            }
            else animator.SetBool("isPatrolling", true);
        }
        repositionTimer += Time.deltaTime;

    }

    
}
