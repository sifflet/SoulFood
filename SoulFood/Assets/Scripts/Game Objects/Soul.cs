using UnityEngine;
using System.Collections;

public class Soul : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    public void getConsumed(Vector3 playerPosition)//to be used for the animation of the soul getting consumed
    {
        Invoke("disableSoul", 0.5f);
    }

    public void getEjected(Vector3 playerPosition)//to be used for the animation of the soul getting spit out
    {
        gameObject.SetActive(true);
    }

    void disableSoul()
    {
        gameObject.SetActive(false);
    }
}
