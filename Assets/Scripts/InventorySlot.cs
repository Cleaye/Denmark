using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public GameObject icon; 
    Item item; 

    public void AddItem(Item newItem) 
    {
        item = newItem;
        SpriteRenderer sr = icon.GetComponent<SpriteRenderer>();
        sr.sprite = item.icon;
    }

    public void ClearSlot()
    {
        item = null;
        SpriteRenderer sr = icon.GetComponent<SpriteRenderer>();
        sr.sprite = null;
    }
}
