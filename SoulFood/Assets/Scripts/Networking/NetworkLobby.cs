using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobby : NetworkLobbyManager {

    public GameObject Collector;
    public GameObject guardPrefab;
    public GameObject Guards;
    public GameObject cameraRigPrefab;
	public GameObject soulPrefab;
    private int numOfCollectors;
    int counter = 0;
    int guardCounter = 0;
    int collectorCounter = 0;

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject spawns = GameObject.FindGameObjectWithTag("Respawn");
        GameObject playerPrefab = null;
        NPCDriver driver = null;

        if (this.lobbySlots[counter].GetComponent<LobbyPlayer>().PlayerType == "Guard")
        {
            playerPrefab = (GameObject)Instantiate(Guards);

            int childrenCount = playerPrefab.transform.childCount;
            for (int i = 0; i < childrenCount; ++i)
            {
                GameObject npcInstance = playerPrefab.transform.GetChild(i).gameObject;
                GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;

                npcInstance.AddComponent<GuardDriver>();
                driver = npcInstance.GetComponent<GuardDriver>();
				driver.Setup(npcInstance, cameraInstance, npcInstance.transform, soulPrefab);
                npcInstance.transform.position = spawns.transform.Find("Guard" + i).position;
                npcInstance.name = "Guard " + i;

                if (i == 0) (driver as GuardDriver).IsLeader = true;

                driver.SetControlledByAI(false);
            }
        }
            
        if (this.lobbySlots[counter].GetComponent<LobbyPlayer>().PlayerType == "Collector")
        {
            playerPrefab = (GameObject)Instantiate(Collector);
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;
            playerPrefab.AddComponent<CollectorDriver>();
            driver = playerPrefab.GetComponent<CollectorDriver>();
			driver.Setup(playerPrefab, cameraInstance, playerPrefab.transform, soulPrefab);
			playerPrefab.transform.position = spawns.transform.Find("Collect" + counter).position;
            playerPrefab.name = "Collector " + collectorCounter;
            collectorCounter++;
            driver.SetControlledByAI(false);
        }
        counter++;
        return playerPrefab;
    }

}
