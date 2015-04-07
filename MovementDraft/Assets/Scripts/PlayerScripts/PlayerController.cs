using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour 
{
    /* Self references */
    private PlayerModel playerModel;
    private PlayerController playerController;
    private Rigidbody playerRigidbody;
    /* --- */

    /* Enemy references */
    private GameObject selectedEnemy;
    private Transform enemy;
    private GameObject[] enemies;
    private EnemyController enemyController;
    /* --- */

    /* Attack variables */
    public float timeBetweenAttacks = 0.15f;
    public float timeBetweenBullets = 0.15f;
    public float attackDamage = 50.0f;
    private AudioSource gunAudio;
    private float timer;
    private float effectsDisplayTime = 0.2f;        
    private bool enemyInRange = false;
    /* --- */

    /* Shoot variables */
    private float range = 100f;
    private Light gunLight;
    private LineRenderer gunLine;
    private ParticleSystem gunParticles;
    private Ray shootRay;
    private RaycastHit shootHit;
    private int shootableMask; 
    /* --- */

    /* Movement Variables */
    public float speed = 6f;
    private Vector3 movement;
    private int floorMask;
    private float camRayLength = 100f;
    /* --- */

    /* Animation variables */
    private Animator playerAnimator;
    private string animationName;
    /* --- */

    /* Inventory Variables */
    private Inventory inventory;
    /* --- */

    /* Defines */
    private string PICKUP = "PickUp";
    /* --- */

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
        shootableMask = LayerMask.GetMask("Enemy");
        playerRigidbody = GetComponent<Rigidbody>();
        // Get reference to player model
        playerModel = GetComponent<PlayerModel>();

        gunParticles = GetComponentInChildren<ParticleSystem>();
        gunLine = GetComponentInChildren<LineRenderer>();
        gunLight = GetComponentInChildren<Light>();

        gunAudio = GetComponentInChildren<AudioSource>();
        /*Get inventory*/
        inventory = (Inventory)GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }

	// Use this for initialization
	void Start () 
    {
        enemy = null;
        enemies = null;
        selectedEnemy = null;
        Animation();
    }

    void Update()
    {
        GetEnemyList();
        if(selectedEnemy == null)
        {
            print("Getting enemy");
            GetEnemy();
        }
        /* Must find a way to cache it, move it from here */
        if (Input.GetKey("f"))
        {
            SelectEnemy();
        }
        if (Input.GetMouseButton(0))
        {
            Action();
        }
        UpdateTimer();
        CheckEffects();
    }

    void GetEnemyList()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void GetEnemy()
    {
        selectedEnemy = GameObject.FindGameObjectWithTag("Enemy");
        if (selectedEnemy != null)
        {
            selectedEnemy.GetComponent<EnemyController>().becomeSelected();
            enemy = selectedEnemy.transform;
            enemyController = enemy.GetComponent<EnemyController>();
            print("Got an enemy");
        }
    }

    void SelectEnemy()
    {
        int index = UnityEngine.Random.Range(0, enemies.Length);
        //print("F pressed");
        //print("Enemy Secected - F. Secected: " + index.ToString());
        if (selectedEnemy != null)
        {
            selectedEnemy.GetComponent<EnemyController>().deselect();
            selectedEnemy.name = "Enemy";
        }
        selectedEnemy =  enemies[index];
        selectedEnemy.name = "SelectedEnemy";
        selectedEnemy.GetComponent<EnemyController>().becomeSelected();
        enemy = selectedEnemy.transform; 
        enemyController = enemy.GetComponent<EnemyController>();
    }
    void Action()
    {
        //print("Action 0");
        if (selectedEnemy != null)
        {
            //print("Action 1");
            Attack();
        }
        //catch (Exception e) { print("In Player Controller action: " + e.ToString()); }
    }
	// Update is called once per frame
	void FixedUpdate () 
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h,v);
        Aim();
	}

    void CheckEffects()
    {
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    void Aim()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;
        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;
            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;
            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotation);
        }
    }
    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);
        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;
        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Animation()
    {
        animationName = "Basic_Run_02";
        playerAnimator = GameObject.FindGameObjectWithTag("Player").transform.GetComponentInChildren<Animator>();
        playerAnimator.speed = 1f;
        playerAnimator.Play(animationName);
    }

    public bool isDead()
    {
        if(playerModel.currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public void applyDamage(Damage damage)
    {
        playerModel.currentHealth -= damage.baseDamage;
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    void Attack()
    {
        //print("Attack 0");
        if (selectedEnemy != null && enemy != null && enemyController != null)
        {
            //print("Attack 1");
            if (timer >= timeBetweenAttacks)
            {
                //print("Attack 2");
                timer = 0f;
                if (enemyController.isDead() == false && enemyInRange == true)
                {
                    //print("Shoot.");
                    Shoot();
                }
                else
                {
                    print("");
                    //print("Enemy Controller: "+enemyController.isDead().ToString());
                    //print("Enemy In Range:"+enemyInRange.ToString());
                }
            }
            else
            {
                print("");
                //print("Time passed: "+timer.ToString()+" out of "+timeBetweenAttacks.ToString());
            }
        }
    }

    void Shoot()
    {
        timer = 0f;
        gunLight.enabled = true;
        gunLine.enabled = true;
        gunLine.SetPosition(0, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;
        gunAudio.Play();

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            //print("Shoot 0");
            EnemyController enemyTest = shootHit.collider.GetComponentInParent<EnemyController>();
            if (enemyTest != null)
            {
                enemyController.applyDamage(new Damage(attackDamage), shootHit.point);
            }

            gunLine.SetPosition(1, new Vector3(shootHit.point.x,shootHit.point.y+1,shootHit.point.z));
        }
        else
        {
            gunLine.SetPosition(1, new Vector3((shootRay.origin + shootRay.direction * range).x, (shootRay.origin + shootRay.direction * range).y + 1, (shootRay.origin + shootRay.direction * range).z));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        enemyInRange = true;
        /*
        if (other.gameObject.transform == enemy)
        {
            enemyInRange = true;
        }
         * */
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == enemy)
        {
            enemyInRange = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print("On Collision 0");
        Collider other = collision.collider;
        if (other.gameObject.tag == PICKUP)
        {
            print("On Collision 1");
            if (inventory.AddItem(other.GetComponent<Item>()))
            {
                print("On Collision 2");
                other.gameObject.SetActive(false);
            }
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }
}
