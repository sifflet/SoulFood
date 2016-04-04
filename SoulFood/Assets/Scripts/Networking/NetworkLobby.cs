using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobby : NetworkLobbyManager {

    public GameObject Collector;
    public GameObject Guard;


    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject playerPrefab = (GameObject)Instantiate(Collector);
        return playerPrefab;
    }

}
