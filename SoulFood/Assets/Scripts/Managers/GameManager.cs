using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    public GameObject deathyPrefab;
    //public GameObject guardPrefab;
    public GameObject cameraRigPrefab;
    public GameObject soulPrefab;
    public GameObject guardsPrefab;

    public GameObject treeOneButton;
    public GameObject treeTwoButton;
    public GameObject treeThreeButton;

    SyncListInt finalTreeSizes = new SyncListInt();

    /* NPC variables */
    public const float COLLISION_RANGE = 1.25f;

    private static float gameTimer = 1f; // [minutes] * 60 seconds/minute. Only modify minutes.
    [SyncVar (hook = "updateLifeHUD")] private int livesRemaining;
    [SyncVar(hook = "updateSoulHUD")] private int soulsConsumed;
    private int soulLimit;

    private static List<Node> nodes;

    private int collectorNum = 4;
    private const int GUARDS_NUM = 2;

	private static Button[] allButtons;

	public static Button[] AllButtons { get; set; }
    public static List<Node> AllNodes { get { return nodes; } }
    public static List<NPCDriver> Collectors { get; set; }
    public static List<NPCDriver> Guards { get; set; }
    public static List<NPCDriver> AllNPCs
    {
        get
        {
            List<NPCDriver> allNPCs = new List<NPCDriver>(Collectors);
            allNPCs.AddRange(Guards);
            return allNPCs;
        }
    }
    /*
    [Command]
    void CmdGetOtherGuardAuthority()
    {
        if (isServer)
        {
            NetworkConnection conn = Guards[0].Instance.GetComponent<NetworkIdentity>().connectionToClient;
            Guards[1].GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
        }
    }
    */
    void Start()
    {
        if (isServer)
        {
            randomizeTrees();
        }

        InitializeNodes();
        InitializeGraph();
        SpawnTrees();
        SetGameLimits();

        Collectors = new List<NPCDriver>();
        Guards = new List<NPCDriver>();

        #region without networking
        /*
        SpawnAllNpcs();
        SetupNPCStateMachines();
        
        (Guards[0] as GuardDriver).IsLeader = true;
        //Deathies[0].SetControlledByAI(false); // human controlled
        Guards[0].SetControlledByAI(false);
        Guards[1].SetControlledByAI(false);
        */
        #endregion

        #region with networking
        GetNetworkNPCs();
        SpawnAllNpcs();
        (Guards[0] as GuardDriver).IsLeader = true;
        SetupNPCStateMachines();
        #endregion

        HeadsUpDisplay.Initialize(soulsConsumed, soulLimit, livesRemaining, gameTimer);

		AllButtons = FindObjectsOfType(typeof(Button)) as Button[];
    }

    void Update()
    {
        if (GameState())
            UpdateNPCs();
        else
            HandleGameConclusion();

        gameTimer += Time.deltaTime;
        HeadsUpDisplay.UpdateHUDGameTimer(gameTimer);

        // TO TEST GAME FLOW
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            livesRemaining = 1;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            soulsConsumed = 18;
            SoulConsumed();
            soulLimit = 20;
        }
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
        for (int i = Collectors.Count; i < collectorNum; i++)
        {
            Transform spawnPoint = GameObject.Find("Collect" + (i)).transform;
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y = deathyPrefab.transform.position.y;
            GameObject npcInstance = Instantiate(deathyPrefab, spawnPosition, spawnPoint.rotation) as GameObject;
            npcInstance.name = "Collector " + i;
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;

            npcInstance.AddComponent<CollectorDriver>();
            CollectorDriver driver = npcInstance.GetComponent<CollectorDriver>();
			driver.Setup(npcInstance, cameraInstance, spawnPoint, soulPrefab);
            Collectors.Add(driver);
            NetworkServer.Spawn(npcInstance);
        }

        if (Guards.Count == 0)
        {
            GameObject combinedGuards = Instantiate(guardsPrefab, Vector3.zero, Quaternion.identity) as GameObject;

            int childrenCount = combinedGuards.transform.childCount;
            for (int i = Guards.Count; i < GUARDS_NUM; ++i)
            {
                Transform spawnPoint = GameObject.Find("Guard" + i).transform;

                GameObject npcInstance = combinedGuards.transform.GetChild(i).gameObject;
                npcInstance.transform.position = spawnPoint.position;

                npcInstance.name = "Guard " + i;
                GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;

                npcInstance.AddComponent<GuardDriver>();
                GuardDriver driver = npcInstance.GetComponent<GuardDriver>();
				driver.Setup(npcInstance, cameraInstance, spawnPoint, soulPrefab);
				Guards.Add(driver);
                NetworkServer.Spawn(npcInstance);
            }
        }
    }
    
    private void GetNetworkNPCs()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.name.Contains("Collector"))
            {
                NPCDriver driver = obj.GetComponent<CollectorDriver>();
                Collectors.Add(driver);
                AllNPCs.Add(driver);
                driver.Sacrebleu();
            }
            else if (obj.name.Contains("CombinedGuards"))
            {
                foreach (Transform child in obj.transform)
                {
                    if(!child.name.Contains("Guard")) continue;

                    NPCDriver driver = child.GetComponent<GuardDriver>();
                    Guards.Add(driver);
                    AllNPCs.Add(driver);
                    driver.Sacrebleu();
                }
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

    private void randomizeTrees()
    {
        Vector3[] treeLocations = { new Vector3(-25f, 0f, -23.35f), new Vector3(-32f, 0f, -0.18f),  new Vector3(20f, 0f, -28.2f),   new Vector3(7f, 0f, -12.1f),
            new Vector3(29f, 0f, 12.3f),    new Vector3(-19.5f, 0f, 21.8f), new Vector3(-13f, 0f, 37.4f),   new Vector3(30f, 0f, 37.4f)};
        int[] treeSizes;
        switch (collectorNum)
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
        finalTreeSizes = new SyncListInt();
        
        for (int i = 0; i < treeLocations.Count(); i++)
        {
            finalTreeSizes.Add(treeSizes[i]);
        }
    }

    private void SpawnTrees()
    {
        Vector3[] treeLocations = { new Vector3(-25f, 0f, -23.35f), new Vector3(-32f, 0f, -0.18f),  new Vector3(20f, 0f, -28.2f),   new Vector3(7f, 0f, -12.1f),
            new Vector3(29f, 0f, 12.3f),    new Vector3(-19.5f, 0f, 21.8f), new Vector3(-13f, 0f, 37.4f),   new Vector3(30f, 0f, 37.4f)};
        GameObject tree;
        for (int i = 0; i < finalTreeSizes.Count; i++)
        {
            switch (finalTreeSizes[i])
            {
                case 1:
                    tree = (GameObject)Instantiate(treeOneButton, treeLocations[i], Quaternion.identity);
                    break;
                case 2:
                    tree = (GameObject)Instantiate(treeTwoButton, treeLocations[i], Quaternion.identity);
                    break;
                default:
                    tree = (GameObject)Instantiate(treeThreeButton, treeLocations[i], Quaternion.identity);
                    break;
            }
            NetworkServer.Spawn(tree);
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
        switch (collectorNum)
        {
            case 4:
                livesRemaining = 13;
                soulLimit = 50;
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
        if (soulsConsumed >= soulLimit || livesRemaining <= 0 || gameTimerEnded())
            return false;
        else
            return true;
    }

    private static void HandleGameConclusion()
    {
        //Fancy display here
        Application.LoadLevel("GameOver");
    }

    public void SoulConsumed()
    {
        ++soulsConsumed;
    }

    public void SoulEjected(int soulsEjected) //When players are hit, can remove more then 1
    {
        soulsConsumed -= soulsEjected;
    }

    public void loseLife()
    {
        --livesRemaining;
        
    }

    private void updateSoulHUD(int souls)
    {
        HeadsUpDisplay.UpdateHUDSoulsCollected(souls, soulLimit);
    }

    private void updateLifeHUD(int lives)
    {
        HeadsUpDisplay.UpdateHUDCollectorRemainingLives(lives);
    }

    public bool gameTimerEnded()
    {
        return gameTimer <= 0.0f;
    }
}