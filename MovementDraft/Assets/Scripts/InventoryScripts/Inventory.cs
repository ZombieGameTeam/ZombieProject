using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : Singleton<Inventory> 
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

    private static int emptySlots;

    private static Slot from;
    private static Slot to;

    public static int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }
    // Use this for initialization
    void Start()
    {
        CreateLayout();
    }

    // Update is called once per frame
    void Update() {}

    /* 
     * In the method "MoveItem(GameObject clicked)",
     * in it's current form, I get:
     * 
     * ---
     * 
     * ArgumentException: failed to convert parameters
System.Reflection.MonoCMethod.Invoke (System.Object obj, BindingFlags invokeAttr, System.Reflection.Binder binder, System.Object[] parameters, System.Globalization.CultureInfo culture) (at /Users/builduser/buildslave/mono-runtime-and-classlibs/build/mcs/class/corlib/System.Reflection/MonoMethod.cs:484)
System.Reflection.MonoCMethod.Invoke (BindingFlags invokeAttr, System.Reflection.Binder binder, System.Object[] parameters, System.Globalization.CultureInfo culture) (at /Users/builduser/buildslave/mono-runtime-and-classlibs/build/mcs/class/corlib/System.Reflection/MonoMethod.cs:528)
System.Reflection.ConstructorInfo.Invoke (System.Object[] parameters) (at /Users/builduser/buildslave/mono-runtime-and-classlibs/build/mcs/class/corlib/System.Reflection/ConstructorInfo.cs:77)
     * 
     * ---
     */
    //public void MoveItem(GameObject clicked) 
    public static void MoveItem(GameObject clicked)
    {
        if(from == null)
        {
            if(!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;
            }
        }
        else if(to == null)
        {
            to = clicked.GetComponent<Slot>();
        }
        if (from != null && to != null)
        {
            Stack<Item> tempTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);
            if ( tempTo.Count == 0 )
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tempTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
        }
    }

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
        if(item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot temp = slot.GetComponent<Slot>();
                if (!temp.IsEmpty)
                {
                    if(temp.CurrentItem.type == item.type && temp.IsAvailable)
                    {
                        temp.AddItem(item);
                        return true;
                    }
                }
            }
            if(emptySlots > 0)
            {
                PlaceEmpty(item);
                return true;
            }
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
