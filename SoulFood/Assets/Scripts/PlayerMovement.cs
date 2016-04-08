using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Simpply attached for testing, remove in final build
public class PlayerMovement : NetworkBehaviour {

    float rotationSpeed = 150f;
    float movementSpeed = 3f;
    float shotDelay = 1f;
    float buffTime = 0f;
    
    Rigidbody rigid;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
            return;

        float horiz = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        float vert = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Rotate(0, horiz, 0);
        transform.Translate(0, 0, vert);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        shotDelay -= Time.deltaTime;
        buffTime -= Time.deltaTime;
    }

}
