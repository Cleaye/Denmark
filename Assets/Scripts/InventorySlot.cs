using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public GameObject icon; 
    public Button button;
    public Item item; 
    public GameObject highlight;

    private void Start() {
    }

    public void AddItem(Item newItem) 
    {
        item = newItem;
        SpriteRenderer sr = icon.GetComponent<SpriteRenderer>();
        sr.sprite = item.icon;
        button.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        SpriteRenderer sr = icon.GetComponent<SpriteRenderer>();
        sr.sprite = null;
        button.interactable = false;
    }
}
