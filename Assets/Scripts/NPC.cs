using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private Animator animator;

    [Header("Animation clips")]
    public AnimationClip move;
    public AnimationClip idle;
    public AnimationClip action;
    [Header("WayPoint")]
    [Tooltip("The current destination of the NPC")]
    public WayPoint wayPoint;
    [Header("Basic speed of movement")]
    [Tooltip("The speed after each waypoint is initialSpeed*type of movement coefficient")]
    public float initialSpeed = 4f;
    [Space]
    private NavMeshAgent navMeshAgent;

    private bool hasToWaitForFunction = false;
    private bool canMove = false;
  
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        AssignDestination(wayPoint);
        navMeshAgent.speed = initialSpeed;

    }

    void Update()
    {
        if (navMeshAgent.remainingDistance < 0.01f && !navMeshAgent.isStopped)
        {
            StartCoroutine(PerformAnimationAfterDelay(wayPoint)); // play animation in a loop
            wayPoint.SetNPCToInterrupt(this);
            AssignDestination(wayPoint.nextWayPoint);
            AssignSpeed(wayPoint);
        }

    }

    private void AssignSpeed(WayPoint wayPoint)
    {
        navMeshAgent.speed = initialSpeed * wayPoint.GetSpeedOfMovement();
    }

    private void AssignDestination(WayPoint destination)
    {
        if (destination != null)
        {
            wayPoint = destination;
            navMeshAgent.SetDestination(wayPoint.transform.position);
        }
        else
        {
            navMeshAgent.isStopped = true;
        }
    }

    public IEnumerator PerformAnimationAfterDelay(WayPoint targetWayPoint)
    {
        int amount = targetWayPoint.loopsAmount;
        float delay = targetWayPoint.GetActualDelay();
        string stateName = null;
        hasToWaitForFunction = wayPoint.waitForGoFunction; //getting values from wayPoint

        if (targetWayPoint.arrivingAnimation != null)
            stateName = wayPoint.arrivingAnimation.name;

        navMeshAgent.isStopped = true;
        do{
            animator.Play(idle.name);
            yield return new WaitForSeconds(delay);
            if (stateName == null)
                amount = 1;
            else
                animator.Play(stateName);
            amount--;
        } while (amount != 0);
        if (hasToWaitForFunction)
        {
            canMove = false;
            animator.Play(idle.name);
            yield return new WaitUntil(() => canMove == true); // waiting when something calls GoToNextPoint
        }
        navMeshAgent.isStopped = false;
    }

    [ContextMenu("Go To the Next Point")]
    public void GoToNextPoint() /* function that is called by WayPoint class to go to  next point*/
    {
        canMove = true;
        navMeshAgent.isStopped = false;
        StopAllCoroutines();

    }
}
