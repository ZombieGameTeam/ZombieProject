using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    PlayerController playerController;
    //EnemyModel enemyModel;
    private NavMeshAgent nav;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        if (playerController.isDead() == false)
        {
            nav.SetDestination(player.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
}
