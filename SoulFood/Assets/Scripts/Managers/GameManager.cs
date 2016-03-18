using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public GameObject deathyPrefab;
    public GameObject guardPrefab;
    public GameObject cameraRigPrefab;
    public GameObject treeOneButton;
    public GameObject treeTwoButton;
    public GameObject treeThreeButton;
    private Vector3[] treeLocations = { new Vector3(-25f, 2f, -23.35f), new Vector3(-32f, 2f, -0.18f),  new Vector3(20f, 2f, -28.2f),   new Vector3(7f, 2f, -12.1f),
                                        new Vector3(29f, 2f, 12.3f),    new Vector3(-19.5f, 2f, 21.8f), new Vector3(-13f, 2f, 37.4f),   new Vector3(30f, 2f, 37.4f)};
    private static List<Node> nodes;

    private static List<NPCDriver> deathies;
    private static List<NPCDriver> guards;

    private const int DEATHY_NUM = 3;
    private const int GUARDS_NUM = 2;

    public static List<Node> AllNodes { get { return nodes; } }
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

	void Start ()
    {
        InitializeNodes();
        InitializeGraph();
        SpawnTrees();
        deathies = new List<NPCDriver>();
        guards = new List<NPCDriver>();

        SpawnAllNpcs();
        SetupNPCStateMachines();

        (guards[0] as GuardDriver).IsLeader = true;
        //deathies[0].SetControlledByAI(false); // human controlled
        //guards[0].SetControlledByAI(false);
        //guards[1].SetControlledByAI(false);
	}
	
	void Update ()
    {
        UpdateNPCs();
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
        int[] treeSizes;
        switch (DEATHY_NUM)
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
        Random r = new Random();
        for (int n = array.Length - 1; n > 0; --n)
        {
            int k = Random.Range(0, n + 1);
            int temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
