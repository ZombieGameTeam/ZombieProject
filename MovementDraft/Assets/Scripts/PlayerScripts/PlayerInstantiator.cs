using UnityEngine;
using System.Collections;

public class PlayerInstantiator : Singleton<PlayerInstantiator> 
{
    public GameObject playerModel;
    public Vector3 location;
    protected PlayerInstantiator() { }
	// Use this for initialization
    void Start() { Instantiate(playerModel, location, Quaternion.identity); }
}
