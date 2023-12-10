using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompareItemManager : MonoBehaviour
{
    public Text itemText;
    public string itemName;
    public SQLManager sqlManager;
    public Transform contentContainer;
    public GameObject itemPrefab;
    public GameObject groceryScreen;
    public List<string> itemNames;
    public List<float> prices;

    public void loadItemInfo()
    {
        itemNames = new List<string>();
        prices = new List<float>();
        sqlManager.ReadSQLValuesStrings("SELECT stores.storename FROM ItemPrices items, StoresTable stores WHERE items.itemname = \'" + itemName + "\' AND stores.addressid = items.addressid ORDER BY price;", itemNames, 0);
        sqlManager.ReadSQLValuesFloats("SELECT itemname, price FROM ItemPrices WHERE itemname = \'" + itemName + "\' ORDER BY price;", prices, 1);
        PopulateStoreScrollView();
    }

    private void PopulateStoreScrollView()
    {
        for (int i = 0; i < itemNames.Count; i++)
        {
            var item = Instantiate(itemPrefab);

            item.GetComponent<ItemButtonScript>().itemText.text = sqlManager.hexToString(itemNames[i]);
            item.GetComponent<ItemButtonScript>().priceText.text = "$" + prices[i].ToString();
            item.GetComponent<ItemButtonScript>().priceText.transform.position = new Vector3(0, 0, 0);
            item.GetComponent<ItemButtonScript>().priceText.rectTransform.sizeDelta = new Vector2(890, 100);
            item.GetComponent<ItemButtonScript>().itemName = itemNames[i];
            item.GetComponent<ItemButtonScript>().sqlManager = sqlManager;
            item.GetComponent<ItemButtonScript>().editButton.SetActive(false);
            item.GetComponent<ItemButtonScript>().deleteButton.SetActive(false);

            item.transform.SetParent(contentContainer);
            item.transform.localScale = Vector2.one;
        }
    }

    public void XButton()
    {
        itemText.text = "";
        gameObject.SetActive(false);
        groceryScreen.SetActive(true);
    }
}
