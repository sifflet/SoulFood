using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject deathyPrefab;
    public GameObject guardPrefab;
    public GameObject cameraRigPrefab;

    private static List<NPCDriver> deathies;
    private static List<NPCDriver> guards;

    private const int DEATHY_NUM = 3;
    private const int GUARDS_NUM = 2;

	void Start ()
    {
        deathies = new List<NPCDriver>();
        guards = new List<NPCDriver>();

        SpawnAllNpcs();
        SetupNPCStateMachines();

        (guards[0] as GuardDriver).IsLeader = true;
        deathies[0].SetControlledByAI(false); // human controlled
        //guards[0].SetControlledByAI(false);
        //guards[1].SetControlledByAI(false);
	}
	
	void Update ()
    {
        UpdateNPCs();
	}

    public static List<NPCDriver> Deathies { get { return deathies; } }
    public static List<NPCDriver> Guards { get { return guards; } }
    public static List<NPCDriver> AllNPCs
    {
        get
        {
            List<NPCDriver> allNPCs = new List<NPCDriver>(deathies);
            allNPCs.AddRange(guards);
            return allNPCs;
        }
    }

    private void UpdateNPCs()
    {
        foreach (NPCDriver npc in AllNPCs)
        {
            npc.Update();
        }
    }

    private void SpawnAllNpcs()
    {
        for (int i = 0; i < DEATHY_NUM; i++)
        {
            Transform spawnPoint = GameObject.Find("DeathySpawn" + (i + 1)).transform;
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = deathyPrefab.transform.position.y;
            GameObject npcInstance = Instantiate(deathyPrefab, spawnPosition, spawnPoint.rotation) as GameObject;
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;

            deathies.Add(new CollectorDriver(npcInstance, cameraInstance, spawnPoint));
        }

        for (int i = 0; i < GUARDS_NUM; i++)
        {
            Transform spawnPoint = GameObject.Find("GuardSpawn" + (i + 1)).transform;
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = guardPrefab.transform.position.y;
            GameObject npcInstance = Instantiate(guardPrefab, spawnPosition, spawnPoint.rotation) as GameObject;
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;

            guards.Add(new GuardDriver(npcInstance, cameraInstance, spawnPoint));
        }
    }

    private void SetupNPCStateMachines()
    {
        foreach (NPCDriver npc in AllNPCs)
        {
            npc.SetupStateMachine();
        }
    }
}
