using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreSceneManager : MonoBehaviour
{
    public Transform contentContainer;
    public GameObject itemPrefab;
    public SQLManager sqlManager;
    public GameObject addItemPopup;
    public GameObject editItemPopup;
    public Text storeName;
    public GameObject viewStoreScreen;
    public GameObject addressPopup;
    public List<string> itemNames;
    public List<float> prices;

    private void Start()
    {
        addItemPopup.SetActive(false);
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
        storeName.text = sqlManager.storeName;
        itemNames = new List<string>();
        prices = new List<float>();
        sqlManager.ReadSQLValues("SELECT itemname FROM ItemPrices WHERE addressid = " + sqlManager.addressId + " ORDER BY itemname;", itemNames, 0);
        sqlManager.ReadSQLValues("SELECT itemname, price FROM ItemPrices WHERE addressid = " + sqlManager.addressId + " ORDER BY itemname;", prices, 1);
        PopulateStoreScrollView();
    }

    private void PopulateStoreScrollView()
    {
        for (int i = 0; i < itemNames.Count; i++)
        {
            var item = Instantiate(itemPrefab);

            item.GetComponent<ItemButtonScript>().itemText.text = sqlManager.hexToString(itemNames[i]);
            item.GetComponent<ItemButtonScript>().priceText.text = "$" + prices[i].ToString();
            item.GetComponent<ItemButtonScript>().itemName = itemNames[i];
            item.GetComponent<ItemButtonScript>().addressId = sqlManager.addressId;
            item.GetComponent<ItemButtonScript>().sqlManager = sqlManager;
            item.GetComponent<ItemButtonScript>().editItemPopup = editItemPopup;
            item.GetComponent<ItemButtonScript>().viewStoreScreen = viewStoreScreen;

            item.transform.SetParent(contentContainer);
            item.transform.localScale = Vector2.one;
        }
    }

    public void BackButton()
    {
        sqlManager.SceneSwitch("StoreScene", "TableScene");
    }

    public void AddressButton()
    {
        viewStoreScreen.SetActive(false);
        addressPopup.SetActive(true);
    }

    public void AddItemButton()
    {
        viewStoreScreen.SetActive(false);
        addItemPopup.SetActive(true);
    }
}
