using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanelUI : MonoBehaviour
{
    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;

    public Image itemImage;
    public TextMeshPro itemInformation;
    public TextMeshPro itemTitle;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    public void OpenItem(Item item) 
    {
        itemImage.sprite = item.itemImage;
        itemInformation.text = item.itemInformation;
        itemTitle.text = item.name;
    }

    void UpdateUI() 
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.backpackItems.Count)
            {
                slots[i].AddItem(inventory.backpackItems[i]);
            } else {
                slots[i].ClearSlot();
            }
        }
        Debug.Log("Updating UI");
    }

}
