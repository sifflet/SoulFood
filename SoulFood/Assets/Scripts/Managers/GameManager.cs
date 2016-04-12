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
    private static bool endGame = false;
    private static float endGameAnimationDuration = 3.0f;
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
            randomizeTombs();
        }

        InitializeNodes();
        InitializeGraph();
        SpawnTombs();
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
//        else
//            HandleGameConclusion();

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

        if(endGame)
        {
            endGameAnimationDuration -= Time.deltaTime;
            if(endGameAnimationDuration <= 0)
            {
                Application.LoadLevel("GameOver");
            }
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
			driver.SetSoulPrefab(soulPrefab);
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

    private void randomizeTombs()
    {
        int[] tombSizes;

        switch (collectorNum)
        {
            case 4:
				tombSizes = new int[] { 1, 1, 2, 2, 2, 3, 3, 3 };
                break;
            case 3:
				tombSizes = new int[] { 1, 1, 1, 2, 2, 2, 3, 3 };
                break;
            case 2:
				tombSizes = new int[] { 1, 1, 1, 1, 2, 2, 2, 2 };
                break;
            default:
				tombSizes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
                break;
        }
		MixArray(tombSizes);
        finalTreeSizes = new SyncListInt();

		GameObject tombSpawnPoints = GameObject.FindGameObjectWithTag("TombsSpawnPoints");
		for (int i = 0; i < tombSpawnPoints.transform.childCount; ++i)
        {
			finalTreeSizes.Add(tombSizes[i]);
        }
    }

    private void SpawnTombs()
    {
		GameObject tombSpawnPoints = GameObject.FindGameObjectWithTag("TombsSpawnPoints");
        GameObject tomb;
        for (int i = 0; i < finalTreeSizes.Count; ++i)
        {

			Transform childTransform = tombSpawnPoints.transform.GetChild(i);

            switch (finalTreeSizes[i])
            {
                case 1:
				tomb = (GameObject)Instantiate(treeOneButton, childTransform.position, childTransform.rotation);
                    break;
                case 2:
				tomb = (GameObject)Instantiate(treeTwoButton, childTransform.position, childTransform.rotation);
                    break;
                default:
				tomb = (GameObject)Instantiate(treeThreeButton, childTransform.position, childTransform.rotation);
                    break;
            }
			NetworkServer.Spawn(tomb);
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
        if (soulsConsumed >= soulLimit || livesRemaining <= 0)
            return false;
        else
            return true;
    }

    private static void HandleGameConclusion()
    {
        endGame = true;
        endGameAnimationDuration = HeadsUpDisplay.PlayGameOverAnimation();
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
}