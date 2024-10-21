using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateMachineBehaviour
{
   
     private float stateDuration;  //Indica la durata dello stato
     private float chaseRange; //Distanza entro la quale l'IA insegue il player
   
    StateController controller;

    float timer;
    Transform player; //Posizione del player
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<StateController>();
        timer = 0f;
        
        player = controller.GetPlayerTransform();
        stateDuration=controller.GetIdleStateDuration();
        chaseRange=controller.GetChaseRange();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceFromPlayer= controller.GetDistanceFromPlayer();
        if(timer > stateDuration)
        {
            animator.SetBool("isPatrolling", true);
        }
        
        if (distanceFromPlayer < chaseRange && controller.canChase)
            animator.SetBool("isChasing", true);

        timer += Time.deltaTime;
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
