using UnityEngine;
using System.Collections;

public class Soul : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    public void IsConsumed(Vector3 playerPosition)//to be used for the animation of the soul getting consumed
    {
		DisableSoul();
    }

    void DisableSoul()
    {
        Destroy(this.gameObject);
    }
}
