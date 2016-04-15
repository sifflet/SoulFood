using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GameManager.HasGuardsWon)
		{
			transform.GetChild(2).gameObject.SetActive(true);
			transform.GetChild(4).gameObject.SetActive(true);
		}
		else
		{
			transform.GetChild(3).gameObject.SetActive(true);
			transform.GetChild(5).gameObject.SetActive(true);
		}
	}
}
