using UnityEngine;
using System.Collections;

public class ReturnToLobbyButton : MonoBehaviour {

	public void LoadOnClick(int level)
	{
		Application.LoadLevel(level);
	}

}