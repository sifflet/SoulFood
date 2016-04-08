using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject deathyPrefab;
    public GameObject guardPrefab;
    public GameObject cameraRigPrefab;
	public GameObject soulPrefab;

    public GameObject treeOneButton;
    public GameObject treeTwoButton;
    public GameObject treeThreeButton;

	/* NPC variables */
	public const float COLLISION_RANGE = 1.25f;

	/* Collector variables */
	public const float TIME_SPENT_SOUL_SEARCHING = 10.0f;
	public const float FLEE_RANGE = 20.0f;
	public const float EMERGENCY_FLEE_RANGE = 10.0f;
	public const float SOUL_COLLECTIBLE_RANGE = 2f;
	public enum FleeRangeType { Default, Emergency };
    public const float IMMORTALITY_TIME = 3.0f;

    /* Guards variables */
    public const float ACTIVATE_LUNGE_DISTANCE = 3.0f;
    public const float DIRECT_PURSUE_RANGE = 5.0f;
    public const float LUNGE_TIME = 0.25f;
    public static float LUNGE_CONE_ANGLE = 30.0f;
    public const float LUNGE_COLLISION_RANGE = 1f;
    public const float PURSUE_NEW_TARGET_TIME = 5.0f;

    private static int livesRemaining;
    private static int soulsConsumed;
    private static int soulLimit;

    private static List<Node> nodes;

    private int deathyNum = 3;
    private const int GUARDS_NUM = 2;

    public static List<Node> AllNodes { get { return nodes; } }
    public static List<NPCDriver> Deathies { get; set; }
    public static List<NPCDriver> Guards { get; set; }
    public static List<NPCDriver> AllNPCs
    {
        get
        {
            List<NPCDriver> allNPCs = new List<NPCDriver>(Deathies);
            allNPCs.AddRange(Guards);
            return allNPCs;
        }
    }

	void Start ()
    {
        InitializeNodes();
        InitializeGraph();
        SpawnTrees();
        SetGameLimits();

        Deathies = new List<NPCDriver>();
        Guards = new List<NPCDriver>();

        #region without networking
        /*
        SpawnAllNpcs();
        SetupNPCStateMachines();
        
        (guards[0] as GuardDriver).IsLeader = true;
        deathies[0].SetControlledByAI(false); // human controlled
        //guards[0].SetControlledByAI(false);
        //guards[1].SetControlledByAI(false);
        */
        #endregion

        #region with networking
        GetNetworkNPCs();
        SpawnAllNpcs();
        SetupNPCStateMachines();

        (Guards[0] as GuardDriver).IsLeader = true;
        #endregion
    }
	
	void Update ()
    {
        if (GameState())
            UpdateNPCs();
        else
            HandleGameConclusion();
	}

    private void UpdateNPCs()
    {
        /*
        foreach (NPCDriver npc in AllNPCs)
        {
            npc.Update();
        }
         * */
    }

    private void SpawnAllNpcs()
    {
        for (int i = Deathies.Count; i < deathyNum; i++)
        {
            Transform spawnPoint = GameObject.Find("Collect" + (i)).transform;
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = deathyPrefab.transform.position.y;
            GameObject npcInstance = Instantiate(deathyPrefab, spawnPosition, spawnPoint.rotation) as GameObject;
			npcInstance.name = "Collector " + i;
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;

            npcInstance.AddComponent<CollectorDriver>();
            CollectorDriver driver = npcInstance.GetComponent<CollectorDriver>();
            driver.Setup(npcInstance, cameraInstance, spawnPoint);
            driver.SetSoulPrefab(soulPrefab);
            Deathies.Add(driver);
        }

        for (int i = Guards.Count; i < GUARDS_NUM; i++)
        {
            Transform spawnPoint = GameObject.Find("Guard" + (i)).transform;
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = guardPrefab.transform.position.y;
            GameObject npcInstance = Instantiate(guardPrefab, spawnPosition, spawnPoint.rotation) as GameObject;
			npcInstance.name = "Guard " + i;
			GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;

            npcInstance.AddComponent<GuardDriver>();
            GuardDriver driver = npcInstance.GetComponent<GuardDriver>();
            driver.Setup(npcInstance, cameraInstance, spawnPoint);
            Guards.Add(driver);
        }
    }

    private void GetNetworkNPCs()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.name.Contains("Collector"))
            {
                NPCDriver driver = obj.GetComponent<CollectorDriver>();
                Deathies.Add(driver);
                AllNPCs.Add(driver);
                driver.Sacrebleu();
            }
            else
            {
                NPCDriver driver = obj.GetComponent<GuardDriver>();
                Guards.Add(driver);
                AllNPCs.Add(driver);
                driver.Sacrebleu();
            }
        }
    }

    private void SetupNPCStateMachines()
    {
        foreach (NPCDriver npc in AllNPCs)
        {
            npc.StartStateMachine();
        }
    }

    private void InitializeNodes()
    {
        nodes = new List<Node>();

        foreach (GameObject node in GameObject.FindGameObjectsWithTag("Node"))
        {
            nodes.Add(node.GetComponent<Node>());
        }
    }

    private void InitializeGraph()
    {
        foreach (Node node in nodes)
        {
            Graph.AddVertex(node, node.neighboringNodeDistances);
        }
    }

    private void SpawnTrees()
    {
        Vector3[] treeLocations = { new Vector3(-25f, 2f, -23.35f), new Vector3(-32f, 2f, -0.18f),  new Vector3(20f, 2f, -28.2f),   new Vector3(7f, 2f, -12.1f),
                                    new Vector3(29f, 2f, 12.3f),    new Vector3(-19.5f, 2f, 21.8f), new Vector3(-13f, 2f, 37.4f),   new Vector3(30f, 2f, 37.4f)};
        int[] treeSizes;
        switch (deathyNum)
        {
            case 4:
                treeSizes = new int[] { 1, 1, 2, 2, 2, 3, 3, 3 };
                break;
            case 3:
                treeSizes = new int[] { 1, 1, 1, 2, 2, 2, 3, 3 };
                break;
            case 2:
                treeSizes = new int[] { 1, 1, 1, 1, 2, 2, 2, 2 };
                break;
            default:
                treeSizes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
                break;
        }
        MixArray(treeSizes);
        for(int i = 0; i < treeLocations.Count(); i++)
        {
            switch (treeSizes[i])
            {
                case 1:
                    Instantiate(treeOneButton, treeLocations[i], Quaternion.identity);
                    break;
                case 2:
                    Instantiate(treeTwoButton, treeLocations[i], Quaternion.identity);
                    break;
                default:
                    Instantiate(treeThreeButton, treeLocations[i], Quaternion.identity);
                    break;
            }
        }
    }

    private void MixArray(int[] array)
    {
        for (int n = array.Length - 1; n > 0; --n)
        {
            int k = Random.Range(0, n + 1);
            int temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    private void SetGameLimits()//Game limits added based on Deathy Number, can remove the life factor but I believe the souls ammount should adjust
    {
        switch (deathyNum)
        {
            case 4:
                livesRemaining = 3;
                soulLimit = 30;
                break;
            case 3:
                livesRemaining = 3;
                soulLimit = 25;
                break;
            case 2:
                livesRemaining = 4;
                soulLimit = 20;
                break;
            default:
                livesRemaining = 5;
                soulLimit = 15;
                break;
        }
    }

    private bool GameState()
    {
        if (soulsConsumed >= soulLimit || livesRemaining <= 0)
            return false;
        else
            return true;
    }

    private static void HandleGameConclusion()
    {
        //Fancy display here
    }

    public static void SoulConsumed()
    {
        soulsConsumed++;
    }

    public static void SoulEjected(int soulsEjected)//When players are hit, can remove more then 1
    {
        soulsConsumed -= soulsEjected;
    }

    public static void loseLife()
    {
        livesRemaining--;
    }
}
