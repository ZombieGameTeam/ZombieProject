using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Slot : MonoBehaviour 
{
    private double PERCENT = 0.50;
    private Stack<Item> items;
    public Text stackText;
    public Sprite slotEmpty;
    public bool IsEmpty
    {
        get
        {
            return items.Count == 0;
        }
    }
	// Use this for initialization
	void Start () 
    {
        items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform textRect = GetComponent<RectTransform>(); /* This should be get component in children */
        int textScaleFactor = (int)(slotRect.sizeDelta.x * PERCENT);

        stackText.resizeTextMaxSize = textScaleFactor;
        stackText.resizeTextMinSize = textScaleFactor;

        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);


    }
	
	// Update is called once per frame
	void Update () {}

    public void AddItem(Item item)
    {
        items.Push(item);
        if (items.Count > 1)
        {
            stackText.text = items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;
        SpriteState state = new SpriteState();
        state.highlightedSprite = highlight;
        state.pressedSprite = neutral;
        GetComponent<Button>().spriteState = state;
    }
}
