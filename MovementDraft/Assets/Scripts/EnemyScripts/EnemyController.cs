using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
    /* Self references */
    private EnemyModel enemyModel;
    private EnemyView enemyView;
    /* --- */

    /* Player references */
    public Transform player;
    private PlayerController playerController;
    /* --- */

    /* Attack variables */
    public float timeBetweenAttacks = 1.0f;
    public float attackDamage = 10.0f;
    private float timer;
    private bool playerInRange = false;
    /* --- */

    /* Movement Variables */
    private NavMeshAgent nav;
    /* --- */

    /* Animation variables */
    private Animator playerAnimator;
    private string animationName;
    /* --- */

    void Awake()
    {
        enemyModel = GetComponent<EnemyModel>();
        enemyView = GetComponent<EnemyView>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
    }

	// Use this for initialization
	void Start () 
    {
        StartAnimation();
    }
	
	// Update is called once per frame
	void Update () 
    {
        UpdateTimer();
        Attack();
        PathFind();
        DeathCheck();
        UpdateView();
	}

    void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    void Attack()
    {
        if (timer >= timeBetweenAttacks)
        {
            timer = 0f;
            if (playerController.isDead() == false && playerInRange == true)
            {
                playerController.applyDamage(new Damage(attackDamage));
            }
        }
    }

    void PathFind()
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

    void StartAnimation()
    {
        animationName = "Etc_Walk_Zombi_01";
        playerAnimator = GameObject.FindGameObjectWithTag("Enemy").transform.GetComponentInChildren<Animator>();
        playerAnimator.speed = 1f;
        playerAnimator.Play(animationName);
    }

    public bool isDead()
    {
        if (enemyModel.currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public void applyDamage(Damage damage,Vector3 damagePoint)
    {
        enemyModel.currentHealth -= damage.baseDamage;
    }

    void DeathCheck()
    {
        if(isDead())
        {
            enemyView.Death();
        }
    }

    void UpdateView()
    {
        return;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == player)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == player)
        {
            playerInRange = false;
        }
    }
}
