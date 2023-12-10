using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroceryItemButtonManager : MonoBehaviour
{
    public string itemName;
    public int count;
    public Text itemText;
    public Text countText;
    public SQLManager sqlManager;
    public GameObject compareItemPopup;
    public GameObject groceryListScreen;

    private CompareItemManager compareItemManager;

    public void DeleteButton()
    {
        sqlManager.ExecuteSQLCommand("DELETE FROM GroceryList WHERE itemname = \'" + itemName + "\'");
        sqlManager.SceneSwitch("GroceryListScene", "GroceryListScene");
    }

    public void CompareButton()
    {
        groceryListScreen.SetActive(false);
        compareItemPopup.SetActive(true);
        compareItemManager = compareItemPopup.GetComponent<CompareItemManager>();
        compareItemManager.itemText.text = itemText.text;
        compareItemManager.itemName = itemName;
        compareItemManager.sqlManager = sqlManager;
        compareItemManager.loadItemInfo();
    }
}
