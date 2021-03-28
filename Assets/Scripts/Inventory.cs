using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    void Awake ()
    {
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

    public void Add (Item item) 
    {
        if (backpackItems.Count >= backpackSpace) {
            Debug.Log("Inventory is full!");
            return;
        }

        backpackItems.Add(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Remove (Item item)
    {
        backpackItems.Remove(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
