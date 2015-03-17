using UnityEngine;
using System.Collections;

public class ZombieScript : MonoBehaviour 
{
    public float attackTimer = 0.0f;
    public float coolDown = 1.0f;

    public float moveSpeed = 5.0f;
    public float rotationSpeed = 5.0f;

    private GameObject player;
    private Transform playerTransform;
// Update is called once per frame
	void Update () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.GetComponent<Transform>();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - playerTransform.position), rotationSpeed * Time.deltaTime);
        transform.position = transform.position - (playerTransform.position * moveSpeed * Time.deltaTime);

        if(attackTimer > 0)
        {
            attackTimer = attackTimer - Time.deltaTime;
        }
        if(attackTimer < 0)
        {
            attackTimer = 0;
        }
        if(attackTimer == 0)
        {
            Attack();
            attackTimer = coolDown;
        }
	}

    void Attack()
    {
        player.GetComponent<PlayerController>().applyReceivedDamage(Random.Range(5,10));
    }
}
