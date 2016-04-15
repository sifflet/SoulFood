using UnityEngine;
using System.Collections;

public class SelectPlayerTypeButtons : MonoBehaviour {
	
	private string buttonName;

	// Use this for initialization
	void Start () {
		buttonName = this.name;
	}

	public void LoadOnClick()
	{
		LobbyPlayer lobbyPlayer = FindObjectOfType(typeof(LobbyPlayer)) as LobbyPlayer;
		
		switch (buttonName) 
		{
		case "Button_Collector":
			lobbyPlayer.PlayerType = "Collector";
			break;
		case "Button_Guard":
			lobbyPlayer.PlayerType = "Guard";
			break;
		}
	}
}
