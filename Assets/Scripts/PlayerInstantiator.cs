using UnityEngine;
using System.Collections;

public class PlayerInstantiator : Singleton<PlayerInstantiator> 
{
    public GameObject playerModel;
    public Vector3 instantationCoordinates;

    protected PlayerInstantiator() { }

    // Use this for initialization
    void Start()
    {
        Instantiate(playerModel, instantationCoordinates, Quaternion.identity);
    }
}
