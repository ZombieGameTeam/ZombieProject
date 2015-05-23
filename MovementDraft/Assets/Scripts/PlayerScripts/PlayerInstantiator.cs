using UnityEngine;
using System.Collections;

public class PlayerInstantiator : Singleton<PlayerInstantiator> 
{
    public GameObject playerModel;
    public GameObject dungeon;
    public Vector3 location;
    protected PlayerInstantiator() { }
	// Use this for initialization
    void Start() 
    {   
		transform.position = DrawMap.Instance.getStartPosition ();

        GameObject player = (GameObject)Instantiate(playerModel, location, Quaternion.identity);
        player.name = "Player";
    }
}
