using UnityEngine;
using System.Collections;

public class DemoAsNPCsButton : MonoBehaviour {

	
	private string buttonName;
	
	// Use this for initialization
	void Start () {
		buttonName = this.name;
	}
	
	public void LoadOnClick()
	{
		NetworkLobby networkLobby = FindObjectOfType(typeof(NetworkLobby)) as NetworkLobby;

		if (buttonName == "Button_AllNPC")
			networkLobby.DemoAsNPCs = true;
	}
}
