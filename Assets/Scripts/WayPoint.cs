using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{   [Tooltip("Animation that will be played when NPC reaches this waypoint")]
    public AnimationClip arrivingAnimation;
    [Header("Delay boundaries")]
    public float minimalDelay;
    public float maximalDelay;
    private float actualDelay;

    [Header("Loops")]
    [Tooltip("Select number of animation loops.Numbers that are 0 or less will make an infinite loop, other values will have usual number of loops")]
    public int loopsAmount;
    [Space]
    [Header("Next Waypoint for a NPC")]
    [Tooltip("Simply drag and drop next waypoint or select ot from parameters menu.")]
    public WayPoint nextWayPoint;
    [Space]
    [Header("Types of movement")]
    [Tooltip("Walk is the slowest one, then goes Run and Sprint is the quickest one")]
    private MovementType typeOfMovement;

    [Space]
    [Header("Waiting to go to next waypoint")]
    [Tooltip("Specifies whether NPC will wain on this waypoint forthe call of function or no. Swithching on means waiting for the function")]
    public bool waitForGoFunction;

    private NPC NPCToInterrupt;// The last NPC that went to this point. Used to have a reference to him in order to interrupt him later on.
    // Start is called before the first frame update
    void Start()
    {
        actualDelay = Random.Range(minimalDelay, maximalDelay);

    }

    public float GetSpeedOfMovement()
    {
        switch (typeOfMovement)
        {
            case MovementType.Walk:
                return 1f;
            case MovementType.Run:
                return 3.5f;
            case MovementType.Sprint:
                return 5f;
            default:
                return 1f;
        }
    }
    public float GetActualDelay()
    {
        return actualDelay;
    }

    public void SetNPCToInterrupt(NPC NPCToInterrupt)
    {
        this.NPCToInterrupt = NPCToInterrupt;
    }
    [ContextMenu("Go To the Next Point")]
    public void ForceNPCToGoToNextPoint()
    {
        if (NPCToInterrupt != null)
        {
            NPCToInterrupt.GoToNextPoint();
        }
    }

    private enum MovementType
    {
        Walk, Run, Sprint
    }
}
