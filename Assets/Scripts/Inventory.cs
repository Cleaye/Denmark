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

    public bool hasRecipeCard = false;
    public bool hasGardenCard = false;
    
    List<int> usedItems = new List<int>();
    Item swappableItem;

    void Awake ()
    {
        var items = Resources.LoadAll("Recipes", typeof(Item));
        if(playerInventory.Load())
        {
            backpackItems = playerInventory.GetBackpackItemsFromDatabase();
            discoveredItems = playerInventory.GetDiscoverItemsFromDatabase();
            hasRecipeCard = playerInventory.HasRecipePostcard();
            hasGardenCard = playerInventory.HasGardenPostcard();
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
        playerInventory.Save();

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
        playerInventory.Save();

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void SetSwapItem(Item item)
    {
        swappableItem = item;
        playerInventory.Save();
    }

    public bool SwapItemsInBackpack(Item itemToSwap)
    {
        int index = backpackItems.IndexOf(itemToSwap);
        if(index != -1 && swappableItem != null)
        {
            backpackItems[index] = swappableItem;
            playerInventory.Save();
            return true;
        }
        
        return false;
    }
    #endregion

    public void ReceiveRecipePostcard()
    {
        hasRecipeCard = true;
        playerInventory.ReceiveRecipePostcard(true);
        playerInventory.Save();
    }

    public void ReceiveGardenPostcard()
    {
        hasGardenCard = true;
        playerInventory.ReceiveGardenPostcard(true);
        playerInventory.Save();
    }

    // Add random items here from time to time
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            Item randomItem = RandomItemGenerator();
            AddToDiscoveredItems(randomItem);
        }

        // Reset
        if(Input.GetKeyDown("1"))
            Reset();

    }

    public Item RandomItemGenerator()
    {
        List<int> availableItems = new List<int>();

        foreach(Item item in backpackItems)
        {
            if(!usedItems.Contains(database.GetId[item]))
                usedItems.Add(database.GetId[item]);
        }

        foreach(Item item in discoveredItems)
        {
            if(!usedItems.Contains(database.GetId[item]))
                usedItems.Add(database.GetId[item]);
        }

        for(int i = 0; i < database.Items.Length; i++)
        {
            if(usedItems.Contains(i))
                continue;
            else
                availableItems.Add(i);
        }

        int randomNumber = Random.Range(0, availableItems.Count);
        return database.GetItem[randomNumber];
    }

    public void Reset()
    {
        backpackItems = new List<Item>();
        discoveredItems = new List<Item>();
        playerInventory.UpdateBackpackInventory(backpackItems);
        playerInventory.UpdateDiscoverInventory(discoveredItems);

        hasRecipeCard = false;
        hasGardenCard = false;

        playerInventory.ReceiveRecipePostcard(false);
        playerInventory.ReceiveGardenPostcard(false);

        onItemChangedCallback.Invoke();
        
        playerInventory.Save();
    }
}
