using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    public ItemDatabase database;
    public PlayerInventory playerInventory;
    Item swappableItem;

    void Awake ()
    {
        var items = Resources.LoadAll("Recipes", typeof(Item));
        if(playerInventory.Load())
        {
            backpackItems = playerInventory.GetBackpackItemsFromDatabase();
            discoveredItems = playerInventory.GetDiscoverItemsFromDatabase();
        }

        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    public List<Item> backpackItems = new List<Item>();
    public List<Item> discoveredItems = new List<Item>();
    public int backpackSpace = 6;
    public int discoverSpace = 3;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    #region Add__Remove_Swap_Items
    // Backpack Items
    public bool AddToBackpack (Item item) 
    {
        if (backpackItems.Count >= backpackSpace) 
            return false;

        backpackItems.Add(item);
        playerInventory.UpdateBackpackInventory(backpackItems);
        playerInventory.Save();

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        return true;
    }

    public void RemoveFromBackpack (Item item)
    {
        backpackItems.Remove(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    // Discovered Items
    public void AddToDiscoveredItems (Item item) 
    {
        if (discoveredItems.Count >= discoverSpace) 
            return;

        discoveredItems.Add(item);
        playerInventory.UpdateDiscoverInventory(discoveredItems);
        playerInventory.Save();

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveFromDiscoveredItems (Item item)
    {
        discoveredItems.Remove(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void SetSwapItem(Item item)
    {
        swappableItem = item;
    }

    public bool SwapItemsInBackpack(Item itemToSwap)
    {
        int index = backpackItems.IndexOf(itemToSwap);
        if(index != -1 && swappableItem != null)
        {
            backpackItems[index] = swappableItem;
            return true;
        }
        
        return false;
    }
    #endregion

    // Add random items here from time to time
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            var items = Resources.LoadAll("Recipes", typeof(Item));
            AddToDiscoveredItems((Item)items[0]);
        }
    }

    public void RandomItemGenerator()
    {
        var items = Resources.LoadAll("Recipes", typeof(Item));
    }
}
