using UnityEngine;
using System.Collections;

public class NewCameraMovement : MonoBehaviour {

    // Update is called once per frame
	void Update ()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform; //Does not work in Start or Awake
        this.transform.position = new Vector3(player.position.x, transform.position.y, player.transform.position.z);
	}
}
