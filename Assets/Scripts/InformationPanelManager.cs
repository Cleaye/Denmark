using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject interactablesPanel;
    [SerializeField] private GameObject messageBox;
    [SerializeField] private GameObject backpackMessage;
    [SerializeField] private GameObject interactionButtons;
    [SerializeField] private GameObject linkButtons;
    [SerializeField] private GameObject swapInventory;

    Item displayedItem;
    Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            interactablesPanel.SetActive(true);
            interactionButtons.SetActive(true);
            informationPanel.SetActive(false);
            linkButtons.SetActive(false);
        }
    }

    public void AddItem()
    {
        if(displayedItem != null)
        {
            if(inventory.AddToBackpack(displayedItem))
                backpackMessage.SetActive(true);
            else
                swapInventory.SetActive(true);
        }
    } 

    public void DeleteItemFromDiscoveredItems()
    {
        // Delete from item panel and close window
        if(displayedItem != null)
        {
            inventory.RemoveFromDiscoveredItems(displayedItem);
            informationPanel.SetActive(false);
        }
        else
        {
            Debug.Log("There is no item to delete!");
        }
    }

    public void DeleteItemFromBackpack()
    {
        // Delete from item panel and close window
        if(displayedItem != null)
        {
            inventory.RemoveFromBackpack(displayedItem);
            informationPanel.SetActive(false);
            CloseMessageBox();
        }
        else
        {
            Debug.Log("There is no item to delete!");
        }
    }

    public void SetSwapItemInInventory()
    {
        inventory.SetSwapItem(displayedItem);
    }

    public void OpenLink()
    {
        Application.OpenURL(displayedItem.link);
    }

    public void OpenMessageBox()
    {
        messageBox.SetActive(true);
    }

    public void CloseMessageBox()
    {
        messageBox.SetActive(false);
    }

    public void CloseInformationPanel()
    {
        messageBox.SetActive(false);
        backpackMessage.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetItem(Item item)
    {
        displayedItem = item;
    }
}
