using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject deathyPrefab;
    public GameObject guardPrefab;

    private static List<NPCDriver> deathies;
    private static List<NPCDriver> guards;

    private const int DEATHY_NUM = 3;
    private const int GUARDS_NUM = 2;

	void Start ()
    {
        deathies = new List<NPCDriver>();
        guards = new List<NPCDriver>();

        SpawnAllNpcs();
	}
	
	void Update ()
    {
        UpdateNPCs();
	}

    public static List<NPCDriver> Deathies { get { return deathies; } }
    public static List<NPCDriver> Guards { get { return guards; } }

    private void UpdateNPCs()
    {
        foreach (NPCDriver deathy in deathies)
        {
            deathy.Update();
        }

        foreach (NPCDriver guard in guards)
        {
            guard.Update();
        }
    }

    private void SpawnAllNpcs()
    {
        for (int i = 0; i < DEATHY_NUM; i++)
        {
            Transform spawnPoint = GameObject.Find("DeathySpawn" + (i + 1)).transform;
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = deathyPrefab.transform.position.y;

            deathies.Add(new NPCDriver(Instantiate(deathyPrefab, spawnPosition, spawnPoint.rotation) as GameObject, spawnPoint, true));
        }

        for (int i = 0; i < GUARDS_NUM; i++)
        {
            Transform spawnPoint = GameObject.Find("GuardSpawn" + (i + 1)).transform;
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = guardPrefab.transform.position.y;

            guards.Add(new NPCDriver(Instantiate(guardPrefab, spawnPosition, spawnPoint.rotation) as GameObject, spawnPoint, true));
        }
    }
}
