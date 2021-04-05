using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "")]
public class PlayerInventory : ScriptableObject, ISerializationCallbackReceiver
{
    private string savePath = "/inventory.Save";

    public bool recipePostCard = false;
    public bool gardenPostCard = false;
    public bool musicPostCard = false;
    public bool factPostCard = false;
    public bool legoPostCard = false;
    public bool newCard = false;
    public List<Item> backpackInventory = new List<Item>();
    public List<Item> discoverInventory = new List<Item>();
    public ItemDatabase database;

    public List<int> lastOpenedDate = new List<int>();
    bool receivedFirst = false;
    bool receivedSecond = false;
    bool receivedThird = false;

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public bool Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
            return true;
        }
        return false;
    }

    public void UpdateBackpackInventory(List<Item> inventory)
    {
        backpackInventory = inventory;
    }

    public void UpdateDiscoverInventory(List<Item> inventory)
    {
        discoverInventory = inventory;
    }

    public void OnAfterDeserialize()
    {
        for(int i = 0; i < backpackInventory.Count; i++)
            backpackInventory[i] = database.GetItem[backpackInventory[i].id];

        for(int i = 0; i < discoverInventory.Count; i++)
            discoverInventory[i] = database.GetItem[discoverInventory[i].id];
    }

    public void OnBeforeSerialize()
    {
    }

    public List<Item> GetBackpackItemsFromDatabase()
    {
        return backpackInventory;
    }

    public List<Item> GetDiscoverItemsFromDatabase()
    {
        return discoverInventory;
    }

    public bool HasRecipePostcard()
    {
        return recipePostCard;
    }

    public bool HasGardenPostcard()
    {
        return gardenPostCard;
    }

    public bool HasMusicPostcard()
    {
        return musicPostCard;
    }

    public bool HasFactPostcard()
    {
        return factPostCard;
    }

    public bool HasLegoPostcard()
    {
        return legoPostCard;
    }

    public bool HasNewPostcard()
    {
        return newCard;
    }

    public void ReceiveRecipePostcard(bool receiveStatus)
    {
        recipePostCard = receiveStatus;
    }

    public void ReceiveGardenPostcard(bool receiveStatus)
    {
        gardenPostCard = receiveStatus;
    }

    public void ReceiveMusicPostcard(bool receiveStatus)
    {
        musicPostCard = receiveStatus;
    }

    public void ReceiveFactPostcard(bool receiveStatus)
    {
        factPostCard = receiveStatus;
    }

    public void ReceiveLegoPostcard(bool receiveStatus)
    {
        legoPostCard = receiveStatus;
    }

    public void ReceiveNewcard(bool receiveStatus)
    {
        newCard = receiveStatus;
    }

    public void UpdateTime(List<int> date)
    {
        lastOpenedDate = date;
    }

    public void ResetTime()
    {
        lastOpenedDate = new List<int>();
    }

    public void UpdateFirst(bool update)
    {
        receivedFirst = update;
    }

    public void UpdateSecond(bool update)
    {
        receivedSecond = update;
    }

    public void UpdateThird(bool update)
    {
        receivedThird = update;
    }

    public List<int> GetLastTime()
    {
        return lastOpenedDate;
    }

    public bool GetFirst()
    {
        return receivedFirst;
    }

    public bool GetSecond()
    {
        return receivedSecond;
    }

    public bool GetThird()
    {
        return receivedThird;
    }
}
