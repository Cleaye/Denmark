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

    public bool development = false; 
    public bool generateBasedOnTime = true;

    public List<int> date; 

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
            playerInventory.ReceiveRecipePostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();

            if(development)
                newCard = true;
        }
    }

    public void ReceiveGardenPostcard()
    {
        if(!playerInventory.HasGardenPostcard())
        {
            hasGardenCard = true;
            playerInventory.ReceiveGardenPostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();

            if(development)
                newCard = true;
        }
    }

    public void ReceiveMusicPostcard()
    {
        if(!playerInventory.HasMusicPostcard())
        {
            hasMusicCard = true;
            playerInventory.ReceiveMusicPostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();

            if(development)
                newCard = true;
        }
    }

    public void ReceiveFactPostcard()
    {
        if(!playerInventory.HasFactPostcard())
        {
            hasFactCard = true;
            playerInventory.ReceiveFactPostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();

            if(development)
                newCard = true;
        }
    }

    public void ReceiveLegoPostcard()
    {
        if(!playerInventory.HasLegoPostcard())
        {
            hasLegoCard = true;
            playerInventory.ReceiveLegoPostcard(true);
            playerInventory.ReceiveNewcard(true);
            playerInventory.Save();

            if(development)
                newCard = true;
        }
    }

    // Add random items here from time to time
    void Update()
    {
        List<int> tempTime = new List<int>();
        var time = System.DateTime.Now;
        tempTime.Add(time.Year);
        tempTime.Add(time.Month);
        tempTime.Add(time.Day);
        tempTime.Add(time.Hour);

        List<int> lastPlayed = playerInventory.GetLastTime();

        if(generateBasedOnTime)
        {
            // New day, so reset items
            if(lastPlayed.Count != 0 && lastPlayed[2] < tempTime[2])
            {
                playerInventory.UpdateFirst(false);
                playerInventory.UpdateSecond(false);
                playerInventory.UpdateThird(false);
                playerInventory.UpdateTime(tempTime);
            }

            // Player playing for the first time
            if(lastPlayed.Count == 0)
            {
                AddToDiscoveredItems(RandomItemGenerator());
                playerInventory.UpdateTime(tempTime);
                playerInventory.UpdateFirst(true);
                playerInventory.Save();
            }

            // First item of the day
            if(!playerInventory.GetFirst())
            {
                AddToDiscoveredItems(RandomItemGenerator());
                playerInventory.UpdateFirst(true);
                playerInventory.Save();
            }

            if(!playerInventory.GetSecond() && tempTime[3] > 12)
            {
                AddToDiscoveredItems(RandomItemGenerator());
                playerInventory.UpdateSecond(true);
                playerInventory.Save();
            }

            if(!playerInventory.GetThird() && tempTime[3] > 18) {
                AddToDiscoveredItems(RandomItemGenerator());
                playerInventory.UpdateThird(true);
                playerInventory.Save();
            }
        }

        if(Input.GetKeyDown("space"))
            AddToDiscoveredItems(RandomItemGenerator());

        // Reset
        if(Input.GetKeyDown("1"))
            Reset();
        
    }

    public Item RandomItemGenerator()
    {
        List<int> availableItems = new List<int>();
        List<int> currentlyOwnedItems = new List<int>();

        try
        {
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
        catch
        {
            int randomNumber = Random.Range(0, database.Items.Length);
            return database.GetItem[randomNumber];
        }

        
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

        playerInventory.UpdateFirst(false);
        playerInventory.UpdateSecond(false);
        playerInventory.UpdateThird(false);
        playerInventory.ResetTime();

        onItemChangedCallback.Invoke();
        
        playerInventory.Save();
    }

    public void AddRandomItem()
    {
        AddToDiscoveredItems(RandomItemGenerator());
    }
}
