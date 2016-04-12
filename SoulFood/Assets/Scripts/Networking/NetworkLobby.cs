using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobby : NetworkLobbyManager {

    public GameObject Collector;
    public GameObject Guard;
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
            playerPrefab = (GameObject)Instantiate(Guard);
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;
            playerPrefab.AddComponent<GuardDriver>();
            driver = playerPrefab.GetComponent<GuardDriver>();
			driver.Setup(playerPrefab, cameraInstance, playerPrefab.transform, soulPrefab);
			playerPrefab.transform.position = spawns.transform.Find("Guard0").position;
            playerPrefab.name = "Guard " + guardCounter;
            guardCounter++;
            (driver as GuardDriver).IsLeader = true;
            //driver.SetControlledByAI(false);
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
            //driver.SetControlledByAI(false);
        }
        counter++;
        return playerPrefab;
    }

}
