using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPanelUI : MonoBehaviour
{
    public Transform backpackItemsParent;
    public Transform discoveredItemsParent;

    Inventory inventory;
    InventorySlot[] discoverySlots;
    InventorySlot[] backpackSlots;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemInformation;
    [SerializeField] private TextMeshProUGUI itemTitle;

    [SerializeField] private GameObject informationPanel;
    private InformationPanelManager panelScript;

    [SerializeField] private SpriteRenderer highlightedItemIcon;
    InventorySlot highlightedItemSlot;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        backpackSlots = backpackItemsParent.GetComponentsInChildren<InventorySlot>();
        discoverySlots = discoveredItemsParent.GetComponentsInChildren<InventorySlot>();

        panelScript = informationPanel.GetComponent<InformationPanelManager>();
    }

    public void OpenItem(InventorySlot slot) 
    {
        itemImage.sprite = slot.item.itemImage;
        itemInformation.text = slot.item.itemInformation;
        itemTitle.text = slot.item.name;
        panelScript.SetItem(slot.item);
        informationPanel.SetActive(true);
        inventory.RemoveFromDiscoveredItems(slot.item);
    }

    public void OpenHighlightedItem()
    {
        OpenItem(highlightedItemSlot);
    }

    public void HighlightItem(InventorySlot slot)
    {
        highlightedItemSlot = slot;
        highlightedItemIcon.sprite = slot.item.icon;
    }

    void UpdateUI() 
    {
        for(int i = 0; i < discoverySlots.Length; i++)
        {
            if (i < inventory.discoveredItems.Count)
            {
                discoverySlots[i].AddItem(inventory.discoveredItems[i]);
            } else {
                discoverySlots[i].ClearSlot();
            }
        }

        for(int i = 0; i < backpackSlots.Length; i++)
        {
            if (i < inventory.backpackItems.Count)
            {
                backpackSlots[i].AddItem(inventory.backpackItems[i]);
            } else {
                backpackSlots[i].ClearSlot();
            }
        }
        Debug.Log("Updating UI");
    }
}
