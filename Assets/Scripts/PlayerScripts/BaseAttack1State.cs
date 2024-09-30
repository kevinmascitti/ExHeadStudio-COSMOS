using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack1State : StateMachineBehaviour
{
    public static EventHandler OnAttackBase1;
    public static Action OnClearEnemyHitList;
    public static EventHandler OnAttackBase1Finished;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //OnClearEnemyHitList.Invoke();
        
        animator.SetBool("isBaseAttack", false);
        OnAttackBase1.Invoke(this, EventArgs.Empty);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnAttackBase1Finished.Invoke(this,EventArgs.Empty);
        OnClearEnemyHitList.Invoke();
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
