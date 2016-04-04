using UnityEngine;
using System.Collections;

public class Soul : MonoBehaviour {
	
    public void IsConsumed(Vector3 playerPosition)//to be used for the animation of the soul getting consumed
    {
		DisableSoul();
    }

    void DisableSoul()
    {
        Destroy(this.gameObject);
    }
}