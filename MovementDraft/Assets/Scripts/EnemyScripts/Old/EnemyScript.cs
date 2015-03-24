using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{
    private Animator player;
    private string animationName;
	// Use this for initialization
	void Start () 
    {
        animationName = "Etc_Walk_Zombi_01";
        player = GameObject.FindGameObjectWithTag("Enemy").transform.GetComponentInChildren<Animator>();
        player.speed = 1f;
        player.Play(animationName);
	}
	// Update is called once per frame
	void Update () {}
}
