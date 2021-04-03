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

    [SerializeField] private GameObject messageBox;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject interactionButtons;
    [SerializeField] private GameObject linkButton;

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
        UpdateUI();
    }

    public void OpenItem(InventorySlot slot) 
    {
        if(slot != null)
        {
            itemImage.sprite = slot.item.itemImage;
            itemInformation.text = slot.item.itemInformation;
            itemTitle.text = slot.item.name;
            panelScript.SetItem(slot.item);
            informationPanel.SetActive(true);
            inventory.RemoveFromDiscoveredItems(slot.item);
        }
    }

    public void OpenHighlightedItem()
    {
        interactionButtons.SetActive(false);
        if(highlightedItemSlot.item.link != "")
            linkButton.SetActive(true);

        OpenItem(highlightedItemSlot);
    }

    public void DeleteHighlightedItem()
    {
        if(highlightedItemSlot != null)
        {
            inventory.RemoveFromBackpack(highlightedItemSlot.item);
            highlightedItemIcon.sprite = null;
            highlightedItemSlot = null;
        }
    }

    public void HighlightItem(InventorySlot slot)
    {
        highlightedItemSlot = slot;
        highlightedItemIcon.sprite = slot.item.icon;
    }

    public void OpenMessageBox(GameObject messageBox)
    {
        if(highlightedItemSlot != null)
            messageBox.SetActive(true);
    }

    public void CloseMessageBox(GameObject messageBox)
    {
        messageBox.SetActive(false);
    }
    
    public void SwapItems(GameObject messageBox)
    {
        if(inventory.SwapItemsInBackpack(highlightedItemSlot.item))
        {
            messageBox.SetActive(true);
        }
    }

    public void UpdateUI() 
    {
        if(discoverySlots != null)
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
        }

        if(backpackSlots != null)
        {
            for(int i = 0; i < backpackSlots.Length; i++)
            {
                if (i < inventory.backpackItems.Count)
                {
                    backpackSlots[i].AddItem(inventory.backpackItems[i]);
                } else {
                    backpackSlots[i].ClearSlot();
                }
            }
        }
    }
}
