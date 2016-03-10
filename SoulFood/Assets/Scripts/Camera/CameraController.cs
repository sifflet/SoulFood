using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private GameObject[] guards;

    private float smoothness = 3.0f;

    void Start()
    {
        this.player = GameObject.Find("Player");
        //this.guards = GameObject.FindGameObjectsWithTag("Guard");
    }

    void Update()
    {
        this.transform.position = new Vector3(this.player.transform.position.x, this.transform.position.y, this.player.transform.position.z - 7.5f);
    }
}