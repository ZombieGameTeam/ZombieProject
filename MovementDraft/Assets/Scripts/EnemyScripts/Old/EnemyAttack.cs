using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour 
{
    private Transform player;
    PlayerController playerController;

    public float timeBetweenAttacks = 1.0f;
    public float attackDamage = 10.0f;

    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
    }
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks)
        {
            Attack();
        }
    }

    void Attack()
    {
        timer = 0f;
        if (playerController.isDead() == false)
        {
            playerController.applyDamage(new Damage(attackDamage));
        }
    }
}
