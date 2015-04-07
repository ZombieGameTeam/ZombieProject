using UnityEngine;
using System.Collections;

public class PlayerInstantiator : Singleton<PlayerInstantiator> 
{
    public GameObject playerModel;
    public Vector3 location;
    protected PlayerInstantiator() { }
	// Use this for initialization
    void Start() 
    {
        GameObject player = (GameObject)Instantiate(playerModel, location, Quaternion.identity);
        player.name = "Player";
    }
}
