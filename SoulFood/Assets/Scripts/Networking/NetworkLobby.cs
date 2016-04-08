using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobby : NetworkLobbyManager {

    public GameObject Collector;
    public GameObject Guard;
    public GameObject cameraRigPrefab;
    private int numOfCollectors;
    int counter = 0;

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject spawns = GameObject.FindGameObjectWithTag("Respawn");
        GameObject playerPrefab = null;
        if (this.lobbySlots[counter].GetComponent<LobbyPlayer>().PlayerType == "Guard") //Currently NPC
        {
            playerPrefab = (GameObject)Instantiate(Guard);
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;
            playerPrefab.AddComponent<CollectorDriver>();
            CollectorDriver driver = playerPrefab.GetComponent<CollectorDriver>();
            driver.Setup(playerPrefab, cameraInstance, playerPrefab.transform);
            playerPrefab.transform.position = spawns.transform.FindChild("Guard0").position;
        }
            
        if (this.lobbySlots[counter].GetComponent<LobbyPlayer>().PlayerType == "Collector") //Currently NPC
        {
            playerPrefab = (GameObject)Instantiate(Collector);
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;
            playerPrefab.AddComponent<CollectorDriver>();
            CollectorDriver driver = playerPrefab.GetComponent<CollectorDriver>();
            driver.Setup(playerPrefab, cameraInstance, playerPrefab.transform);
            playerPrefab.transform.position = spawns.transform.FindChild("Collect" + counter).position;
        }

        counter++;
        return playerPrefab;
    }

}
