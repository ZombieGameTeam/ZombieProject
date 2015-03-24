using UnityEngine;
using System.Collections;

public class EnemySpawner : Singleton<PlayerInstantiator>  
{
    private PlayerController playerController;

    public GameObject enemy;
    public float spawnTime = 3f;
    public Vector3 spawnPoint;

    void Awake()
    {
        /* It won't work here, even with script execution order set */
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        InvokeRepeating("Spawn", 0, spawnTime);
    }
    void Spawn()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController.isDead() == true)
        {
            return;
        }
        Instantiate(enemy, spawnPoint, Quaternion.identity);
    }
}

