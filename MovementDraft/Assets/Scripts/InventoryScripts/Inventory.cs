using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private RectTransform inventoryRect;

    private float inventoryWidth;
    private float inventoryHeight;

    public int slots;
    public int rows;

    public float slotPaddingLeft;
    public float slotPladdingTop;

    public float slotSize;

    public GameObject slotPrefab;

    private List<GameObject> allSlots;

    private int emptySlots;
    // Use this for initialization
    void Start()
    {
        CreateLayout();
    }

    // Update is called once per frame
    void Update() { }

    private void CreateLayout()
    {
        int columns = slots / rows;

        allSlots = new List<GameObject>();

        emptySlots = slots;

        inventoryWidth = columns * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPladdingTop) + slotPladdingTop;

        inventoryRect = GetComponent<RectTransform>();

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);
                slotRect.localPosition = inventoryRect.localPosition + new Vector3((slotPaddingLeft * (x + 1) + (slotSize * x)), (-slotPladdingTop * (y + 1) - (slotSize * y)), 1);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
                allSlots.Add(newSlot);
            }
        }
    }

    public bool AddItem(Item item)
    {
        print("The add item method is called.");
        if(item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        /* Goes through allSlots till it finds an empty slot */
        if(emptySlots > 0)
        {
            foreach(GameObject slot in allSlots)
            {
                Slot temp = slot.GetComponent<Slot>();
                try
                {
                    if (temp.IsEmpty)
                    {
                        temp.AddItem(item);
                        emptySlots--;
                        return true;
                    }
                }
                catch(UnityException e)
                {
                    print(e.ToString());
                }

            }
        }
        return false;
    }
}
