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

    [SerializeField] private GameObject recipePostcard;
    [SerializeField] private GameObject gardenPostcard;
    [SerializeField] private GameObject musicPostcard;
    [SerializeField] private GameObject factPostcard;
    [SerializeField] private GameObject legoPostcard;

    [SerializeField] private GameObject newPostcardIcon;

    private InformationPanelManager panelScript;

    [SerializeField] private SpriteRenderer highlightedItemIcon;
    
    InventorySlot highlightedItemSlot;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        if(inventory.hasRecipeCard)
            recipePostcard.SetActive(true);

        if(inventory.hasGardenCard)
            gardenPostcard.SetActive(true);

        if(inventory.hasMusicCard)
            musicPostcard.SetActive(true);

        if(inventory.hasFactCard)
            factPostcard.SetActive(true);

        if(inventory.hasLegoCard)
            legoPostcard.SetActive(true);

        if(inventory.newCard)
            newPostcardIcon.SetActive(true);
        else
            newPostcardIcon.SetActive(false);

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
            DeselectEveryHighlight();
        }
    }

    public void SendHighlightedItem()
    {
        if(highlightedItemSlot != null)
        {
            if(highlightedItemSlot.item.name.Contains("Recipe") ||
                highlightedItemSlot.item.name.Contains("Pølser") ||
                highlightedItemSlot.item.name.Contains("Frikadellerr"))
                inventory.ReceiveRecipePostcard();
            else if(highlightedItemSlot.item.name.Contains("Gardens"))
                inventory.ReceiveGardenPostcard();
            else if(highlightedItemSlot.item.name.Contains("Andersen") || 
                    highlightedItemSlot.item.name.Contains("Maps") || 
                    highlightedItemSlot.item.name.Contains("Carlsberg") ||
                    highlightedItemSlot.item.name.Contains("Oldest")||
                    highlightedItemSlot.item.name.Contains("Little"))
                inventory.ReceiveFactPostcard();
            else if(highlightedItemSlot.item.name.Contains("MØ") || 
                    highlightedItemSlot.item.name.Contains("Rune") || 
                    highlightedItemSlot.item.name.Contains("Corr"))
                inventory.ReceiveMusicPostcard();
            else if(highlightedItemSlot.item.name.Contains("LegoLand"))
                inventory.ReceiveLegoPostcard();
            
            inventory.RemoveFromBackpack(highlightedItemSlot.item);
            highlightedItemIcon.sprite = null;
            highlightedItemSlot = null;
            DeselectEveryHighlight();
        }
    }

    public void HighlightItem(InventorySlot slot)
    {
        highlightedItemSlot = slot;
        highlightedItemIcon.sprite = slot.item.icon;

        DeselectEveryHighlight();

        slot.highlight.SetActive(true);
    }

    public void DeselectEveryHighlight()
    {
        foreach(InventorySlot backpackSlot in backpackSlots)
            backpackSlot.highlight.SetActive(false);
    }

    public void ClearHighlightedItem()
    {
        highlightedItemSlot = null;
        highlightedItemIcon.sprite = null;
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
            ClearHighlightedItem();
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

        // Don't update immediately for player release
        // if(inventory.hasRecipeCard)
        //     recipePostcard.SetActive(true);
        // else
        //     recipePostcard.SetActive(false);
        // if(inventory.hasGardenCard)
        //     gardenPostcard.SetActive(true);
        // else
        //     gardenPostcard.SetActive(false);
        // if(inventory.hasMusicCard)
        //     musicPostcard.SetActive(true);
        // else
        //     musicPostcard.SetActive(false);
        // if(inventory.hasLegoCard)
        //     legoPostcard.SetActive(true);
        // else
        //     legoPostcard.SetActive(false);
        // if(inventory.hasFactCard)
        //     factPostcard.SetActive(true);
        // else
        //     factPostcard.SetActive(false);
        // if(inventory.newCard)
        //     newPostcardIcon.SetActive(true);
        // else
        //     newPostcardIcon.SetActive(false);
    }
}
