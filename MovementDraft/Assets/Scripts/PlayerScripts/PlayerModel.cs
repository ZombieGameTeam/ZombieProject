using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Weapon
{
	PISTOL,
	MACHINEGUN
}

public class PlayerModel : MonoBehaviour 
{
    public float currentHealth = 10000.0f; /* Refactor: Make it private and use getters and setters */
	public float currentStamina = 100.0f;
	public float currentHunger = 0.0f; /* Hunger starts at 0 and increases */
	public float currentThirst = 0.0f; /* Thirst starts at 0 and increases */
	public int currentAmmon = 10;
	public Weapon currentWeapon = Weapon.PISTOL;
	public Dictionary<Weapon,int> damageDictionary;

	// Use this for initialization
	void Start () 
	{
		currentAmmon = 10;
		damageDictionary = new Dictionary<Weapon, int> ();
		damageDictionary.Add (Weapon.PISTOL, 30);
		damageDictionary.Add (Weapon.MACHINEGUN, 50);
		currentWeapon = Weapon.PISTOL;
	}
	
	// Update is called once per frame
    void Update() {}
}
