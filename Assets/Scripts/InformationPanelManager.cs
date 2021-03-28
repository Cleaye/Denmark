using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanelManager : MonoBehaviour
{
    public GameObject informationPanel;
    public GameObject interactablesPanel;

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
        }
        else
        {
            Debug.Log("There is no item to add!");
        }
        
        // TODO: 
        // 1. Show succesfull add item message
        // 2. Disable buttons
    } 

    public void DeleteItem(Item displayedItem)
    {
        // Delete from item panel and close window
    }

    public void SetItem(Item item)
    {
        displayedItem = item;
    }
}
