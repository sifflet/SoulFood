using UnityEngine;
using System.Collections;

public class Soul : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    public void getConsumed(Vector3 playerPosition)//to be used for the animation of the soul getting consumed
    {
        //Fancy soul movement here
        Invoke("disableSoul", 0.5f);
    }

    void disableSoul()
    {
        Destroy(this);
    }
}
