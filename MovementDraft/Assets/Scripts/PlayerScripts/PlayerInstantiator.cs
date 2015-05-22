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
        Instantiate(dungeon, Vector3.zero, Quaternion.identity);
        

        GameObject player = (GameObject)Instantiate(playerModel, location, Quaternion.identity);
        player.name = "Player";
    }
}
