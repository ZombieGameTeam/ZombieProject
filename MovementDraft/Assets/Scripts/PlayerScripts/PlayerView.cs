using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour 
{
    private PlayerModel playerModel;
    private Slider healthBar;

    void Awke()
    {
        /* It won't work here, even with script execution order set */
        //playerModel = GetComponent<PlayerModel>();
    }
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () 
    {
        playerModel = GetComponent<PlayerModel>();
        healthBar = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Slider>(); /* Workaround, needs rrefactoring */
        if (healthBar != null && playerModel != null)
        {
            /*
            print(healthBar.value);
            print(playerModel.currentHealth);
            print("-----");
            */
            healthBar.value = playerModel.currentHealth/100;
        }
    }
}
