using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Slot : MonoBehaviour, IPointerClickHandler
{
    private double PERCENT = 0.50;
    private Stack<Item> items;

    public Stack<Item> Items
    {
        get { return items; }
        set { items = value; }
    }
    public Text stackText;
    public Sprite slotEmpty;
    public Sprite slotHighLight;
    public bool IsEmpty
    {
        get{ return items.Count == 0; }
    }
    public bool IsAvailable
    {
        get { return CurrentItem.maxSize > items.Count; }
    }
    public Item CurrentItem
    {
        get { return items.Peek(); }
    }
	// Use this for initialization
	void Start () 
    {
        items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform textRect = stackText.GetComponent<RectTransform>();
        int textScaleFactor = (int)(slotRect.sizeDelta.x * PERCENT);

        stackText.resizeTextMaxSize = textScaleFactor;
        stackText.resizeTextMinSize = textScaleFactor;

        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }
	
	// Update is called once per frame
	void Update () 
    {}

    public void AddItem(Item item)
    {
        items.Push(item);
        if (items.Count > 1)
        {
            stackText.text = items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);
        stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;
        SpriteState state = new SpriteState();
        state.highlightedSprite = highlight;
        state.pressedSprite = neutral;
        GetComponent<Button>().spriteState = state;
    }

    void UseItem()
    {
        if(!IsEmpty)
        {
            items.Pop().Use();
            stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
            if(IsEmpty)
            {
                ChangeSprite(slotEmpty, slotHighLight);
                Inventory.EmptySlots++;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.MoveItem(gameObject);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void ClearSlot()
    {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighLight);
        stackText.text = string.Empty;
    }
}
