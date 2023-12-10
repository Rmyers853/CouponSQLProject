using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroceryListSceneManager : MonoBehaviour
{
    public Transform contentContainer;
    public GameObject itemListPrefab;
    public SQLManager sqlManager;
    public GameObject addItemPopup;
    public GameObject compareItemPopup;
    public GameObject groceryListScreen;
    public GameObject compareListPopup;
    public List<string> itemNames;
    public List<int> counts;

    private void Start()
    {
        addItemPopup.SetActive(false);
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
        itemNames = new List<string>();
        counts = new List<int>();
        sqlManager.ReadSQLValues("SELECT itemname FROM GroceryList ORDER BY itemname;", itemNames, 0);
        sqlManager.ReadSQLValues("SELECT itemname, count FROM GroceryList ORDER BY itemname;", counts, 1);
        PopulateStoreScrollView();
    }

    private void PopulateStoreScrollView()
    {
        for (int i = 0; i < itemNames.Count; i++)
        {
            var item = Instantiate(itemListPrefab);

            item.GetComponent<GroceryItemButtonManager>().itemText.text = sqlManager.hexToString(itemNames[i]);
            item.GetComponent<GroceryItemButtonManager>().countText.text = counts[i].ToString();
            item.GetComponent<GroceryItemButtonManager>().itemName = itemNames[i];
            item.GetComponent<GroceryItemButtonManager>().count = counts[i];
            item.GetComponent<GroceryItemButtonManager>().sqlManager = sqlManager;
            item.GetComponent<GroceryItemButtonManager>().compareItemPopup = compareItemPopup;
            item.GetComponent<GroceryItemButtonManager>().groceryListScreen = groceryListScreen;

            item.transform.SetParent(contentContainer);
            item.transform.localScale = Vector2.one;
        }
    }

    public void BackButton()
    {
        sqlManager.SceneSwitch("GroceryListScene", "HomeScene");
    }

    public void AddItemButton()
    {
        groceryListScreen.SetActive(false);
        addItemPopup.SetActive(true);
    }

    public void CompareButton()
    {
        groceryListScreen.SetActive(false);
        compareListPopup.SetActive(true);
        compareListPopup.GetComponent<CompareListManager>().sqlManager = sqlManager;
        compareListPopup.GetComponent<CompareListManager>().loadItemInfo();
    }
}
