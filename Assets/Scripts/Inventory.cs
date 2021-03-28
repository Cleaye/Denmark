using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake ()
    {
        var items = Resources.LoadAll("Recipes", typeof(Item));

        if(instance != null)
        {
            Debug.LogWarning("More than one instance of INventory found!");
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

    #region Add_And_Remove_Items
    // Backpack Items
    public void AddToBackpack (Item item) 
    {
        if (backpackItems.Count >= backpackSpace) {
            Debug.Log("Inventory is full!");
            return;
        }

        backpackItems.Add(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
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
        if (discoveredItems.Count >= discoverSpace) {
            Debug.Log("Inventory is full!");
            return;
        }

        discoveredItems.Add(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void RemoveFromDiscoveredItems (Item item)
    {
        discoveredItems.Remove(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
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
