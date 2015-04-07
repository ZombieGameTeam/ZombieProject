using UnityEngine;
using System.Collections;

public class EnemySpawner : Singleton<EnemySpawner>  
{
    private PlayerController playerController;

    public GameObject enemy;
    public float spawnTime = 3f;
    public Vector3 spawnPoint;

    private static int numberOfEmemiesSpawned;

    void Awake()
    {
        numberOfEmemiesSpawned = 0;
        /* It won't work here, even with script execution order set */
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Start()
    {
        InvokeRepeating("Spawn", 1, spawnTime);
    }
    void Spawn()
    {
        //print("Spawning enemy: "+numberOfEmemiesSpawned.ToString());
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController.isDead() == true)
        {
            print("Playeer Dead.");
            return;
        }
        GameObject instance = (GameObject)Instantiate(enemy, spawnPoint, Quaternion.identity);
        instance.name = "Enemy" + numberOfEmemiesSpawned.ToString();
        numberOfEmemiesSpawned++;
    }
}

