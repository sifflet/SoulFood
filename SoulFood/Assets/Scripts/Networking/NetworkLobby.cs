using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobby : NetworkLobbyManager {

    public GameObject Collector;
    public GameObject Guard;
    public GameObject cameraRigPrefab;
    int counter = 0;

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {

        GameObject playerPrefab = null;
        if (this.lobbySlots[counter].GetComponent<LobbyPlayer>().PlayerType == "Guard") //Currently NPC
        {
            playerPrefab = (GameObject)Instantiate(Guard);
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;
            playerPrefab.AddComponent<CollectorDriver>();
            CollectorDriver driver = playerPrefab.GetComponent<CollectorDriver>();
            driver.Setup(playerPrefab, cameraInstance, playerPrefab.transform);
        }
            
        if (this.lobbySlots[counter].GetComponent<LobbyPlayer>().PlayerType == "Collector") //Currently NPC
        {
            playerPrefab = (GameObject)Instantiate(Collector);
            GameObject cameraInstance = Instantiate(cameraRigPrefab, Vector3.zero, cameraRigPrefab.transform.rotation) as GameObject;
            playerPrefab.AddComponent<CollectorDriver>();
            CollectorDriver driver = playerPrefab.GetComponent<CollectorDriver>();
            driver.Setup(playerPrefab, cameraInstance, playerPrefab.transform);
        }

        counter++;
        return playerPrefab;
    }

}
