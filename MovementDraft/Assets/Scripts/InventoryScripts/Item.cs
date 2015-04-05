using UnityEngine;
using System.Collections;

/*
 * Move it in it's own file
 * 
 * using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType {

	public string name;
	public GameObject tilleVisualPrefab;

	public float movementCost = 1f;
	public bool isWalkable = true;
}
 */
public enum ItemType
{
    HEALTH,
    STAMINA
};

public class Item : MonoBehaviour
{
    public ItemType type;
    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;
    public int maxSize;

    public void Use()
    {
        switch (type)
        {
            case ItemType.HEALTH:
                Debug.Log("Used health item.");
                break;
            case ItemType.STAMINA:
                Debug.Log("Used stamina item.");
                break;
            default:
                break;
        }
    }
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }
}

