using UnityEngine;
using System.Collections;

public class EnemyModel : MonoBehaviour 
{
    public float currentHealth = 100.0f; /* Refactor: Make it private and use getters and setters */
    private bool isSelected;

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
