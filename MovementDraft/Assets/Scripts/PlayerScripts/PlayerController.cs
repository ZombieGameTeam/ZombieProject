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
    private Transform enemy;
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
    }

	// Use this for initialization
	void Start () 
    {
        Animation();
    }

    void Update()
    {
        /* Must find a way to cache it, move it from here */
        try
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
            enemyController = enemy.GetComponent<EnemyController>();
            if (enemy != null)
            {
                UpdateTimer();
                if (Input.GetMouseButton(0))
                {
                    Attack();
                }
            }
        }
        catch (Exception e) { }
        CheckEffects();
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
        if (timer >= timeBetweenAttacks)
        {
            timer = 0f;
            if (enemyController.isDead() == false && enemyInRange == true)
            {
                Shoot();
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
        if (other.gameObject.transform == enemy)
        {
            enemyInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform == enemy)
        {
            enemyInRange = false;
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }
}
