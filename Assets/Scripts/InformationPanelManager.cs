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
            informationPanel.SetActive(false);
        }
    }

    public void AddItem()
    {
        if(displayedItem != null)
        {
            inventory.AddToBackpack(displayedItem);
            informationPanel.SetActive(false);
        }
        else
        {
            Debug.Log("There is no item to add!");
        }
        
        // TODO: 
        // 1. Show succesfull add item message
        // 2. Disable buttons
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
        gameObject.SetActive(false);
    }

    public void SetItem(Item item)
    {
        displayedItem = item;
    }
}
