using UnityEngine;
using System.Collections;

public class Statistics : Singleton<Statistics> 
{
	protected Statistics () {} // guarantee this will be always a singleton only - can't use the constructor!
	
	private static Statistics _instance;

	public int shotsFired = 0;
	public int enemyKilled = 0;
	public float bulletsToKillRatio; /* A player with a high ration has a better skill */

	public static Statistics instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<Statistics>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	
	void Awake() 
	{
		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
			{
				Destroy(this.gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(shotsFired != 0)
		{
			bulletsToKillRatio = (float)((float)(enemyKilled)/(float)(shotsFired));
			Debug.Log("bulletsToKillRatio: " + bulletsToKillRatio);
		}
	}
}
