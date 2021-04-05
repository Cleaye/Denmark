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
    public bool hasFactCard = false;
    public bool hasMusicCard = false;
    public bool hasLegoCard = false;
    public bool newCard = false;

    Item swappableItem;

    void Awake ()
    {
        if(playerInventory.Load())
        {
            backpackItems = playerInventory.GetBackpackItemsFromDatabase();
            discoveredItems = playerInventory.GetDiscoverItemsFromDatabase();
            hasRecipeCard = playerInventory.HasRecipePostcard();
            hasGardenCard = playerInventory.HasGardenPostcard();
            hasMusicCard = playerInventory.HasMusicPostcard();
            hasFactCard = playerInventory.HasFactPostcard();
            hasLegoCard = playerInventory.HasLegoPostcard();
            newCard = playerInventory.HasNewPostcard();
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
        if(!playerInventory.HasRecipePostcard())
        {
            hasRecipeCard = true;
            newCard = true;
            playerInventory.ReceiveRecipePostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();
        }
    }

    public void ReceiveGardenPostcard()
    {
        if(!playerInventory.HasGardenPostcard())
        {
            hasGardenCard = true;
            newCard = true;
            playerInventory.ReceiveGardenPostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();
        }
    }

    public void ReceiveMusicPostcard()
    {
        if(!playerInventory.HasMusicPostcard())
        {
            hasMusicCard = true;
            newCard = true;
            playerInventory.ReceiveMusicPostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();
        }
    }

    public void ReceiveFactPostcard()
    {
        if(!playerInventory.HasFactPostcard())
        {
            hasFactCard = true;
            newCard = true;
            playerInventory.ReceiveFactPostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();
        }
    }

    public void ReceiveLegoPostcard()
    {
        if(!playerInventory.HasLegoPostcard())
        {
            hasLegoCard = true;
            newCard = true;
            playerInventory.ReceiveLegoPostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();
        }
    }

    public void ClickedOnNewPostcard()
    {
        newCard = false;
        playerInventory.ReceiveNewcard(false);
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
        List<int> currentlyOwnedItems = new List<int>();

        foreach(Item item in backpackItems)
            currentlyOwnedItems.Add(database.GetId[item]);

        foreach(Item item in discoveredItems)
            if(!currentlyOwnedItems.Contains(database.GetId[item]))
                currentlyOwnedItems.Add(database.GetId[item]);

        for(int i = 0; i < database.Items.Length; i++)
        {
            if(currentlyOwnedItems.Contains(i))
                continue;
            else
                availableItems.Add(i);
        }

        if(availableItems.Count == 0)
            return database.GetItem[Random.Range(0, database.Items.Length)];

        int randomNumber = Random.Range(0, availableItems.Count);
        int selectedItem = availableItems[randomNumber];
        return database.GetItem[selectedItem];
    }

    public void Reset()
    {
        backpackItems = new List<Item>();
        discoveredItems = new List<Item>();
        playerInventory.UpdateBackpackInventory(backpackItems);
        playerInventory.UpdateDiscoverInventory(discoveredItems);

        hasRecipeCard = false;
        hasGardenCard = false;
        hasMusicCard = false;
        hasFactCard = false;
        hasLegoCard = false;
        newCard = false;

        playerInventory.ReceiveRecipePostcard(false);
        playerInventory.ReceiveGardenPostcard(false);
        playerInventory.ReceiveMusicPostcard(false);
        playerInventory.ReceiveFactPostcard(false);
        playerInventory.ReceiveLegoPostcard(false);
        playerInventory.ReceiveNewcard(false);

        onItemChangedCallback.Invoke();
        
        playerInventory.Save();
    }
}
