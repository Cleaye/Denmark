using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public int id;
    public string name = "New Item";
    public Sprite icon = null;
    public string itemInformation = "Information";
    public Sprite itemImage = null;
    public string link = "";
}
