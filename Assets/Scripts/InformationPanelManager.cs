using UnityEngine;
using UnityEngine.UI;

public class InformationPanelManager : MonoBehaviour
{
    public GameObject informationPanel;
    public GameObject interactablesPanel;

    Item item;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            interactablesPanel.SetActive(true);
            informationPanel.SetActive(false);
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    } 

    public void DeleteItem(Item newItem)
    {
        // Delete from item panel and close window
    }
}
