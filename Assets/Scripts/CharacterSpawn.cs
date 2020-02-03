using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour
{
    [Header("NPC to spawn")]
    [Tooltip("An NPC that you want to spawn. Simply drag and drop something, that has NPC script here")]
    public GameObject NPCToSpawn;
    [Header("NPCs initial waypoint")]
    [Tooltip("Assign the first waypoint, that NPC will go towards to")]
    public WayPoint FirstWayPoint;
    [Header("Spawn options")]
    [Tooltip("OnStart spawn when the level is loaded, OnFunction spawns when the function Spawn is called")]
    [SerializeField]
    private SpawnOption option;
    // Start is called before the first frame update
    void Start()
    {
        if (option == SpawnOption.OnStart)
            Spawn();
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ContextMenu("Spawn function")]
    public void Spawn()
    {
        var npc = Instantiate(NPCToSpawn, transform.position, Quaternion.identity).GetComponent<NPC>();
        if (npc != null)
            AssignWayPoint(npc, FirstWayPoint);
    }

    public void AssignWayPoint(NPC npc, WayPoint wayPoint)
    {
        npc.wayPoint = wayPoint;
    }

    private enum SpawnOption
    {
        OnStart, OnFunction
    }
}
