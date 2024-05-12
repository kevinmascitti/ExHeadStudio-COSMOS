using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateMachineBehaviour
{
     private float stateDuration; //Indica la durata dello stato
     private float chaseRange; //Distanza entro la quale l'IA insegue il player

    StateController controller;

    float timer;

    //Transform player; //Posizione del player
    List<Transform> wayPoints = new List<Transform>(); //Lista di punti da reaggiungere mentre � in patrol
    NavMeshAgent agent;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.GetComponent<StateController>();
        agent= animator.GetComponent<NavMeshAgent>();
        wayPoints = controller.GetWaypoints();
        timer = 0f;
        stateDuration = controller.GetPatrollingStateDuration();
        chaseRange = controller.GetChaseRange();
        agent.speed = controller.GetPatrollingSpeed();
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        Debug.Log(agent.destination);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            Debug.Log(agent.destination);
        }
        
        if(timer > stateDuration) {
            animator.SetBool("isPatrolling", false);
        }
        float distance = controller.GetDistanceFromPlayer();
        if (distance < chaseRange)
            animator.SetBool("isChasing", true);

        timer += Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
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
