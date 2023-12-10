using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonScript : MonoBehaviour
{
    public int addressId;
    public string itemName;
    public SQLManager sqlManager;
    public GameObject editItemPopup;
    public GameObject viewStoreScreen;
    private EditItemManager editItemManager;
    public Text itemText;
    public Text priceText;
    public GameObject editButton;
    public GameObject deleteButton;

    public void DeleteButton()
    {
        sqlManager.ExecuteSQLCommand("DELETE FROM ItemPrices WHERE addressId = " + addressId + " AND itemname = \'" + itemName + "\'");
        sqlManager.SceneSwitch("StoreScene", "StoreScene");
    }

    public void EditButton()
    {
        viewStoreScreen.SetActive(false);
        editItemPopup.SetActive(true);
        editItemManager = editItemPopup.GetComponent<EditItemManager>();
        editItemManager.currentAddressId = addressId;
        editItemManager.currentItemName = itemName;
        editItemManager.loadItemInfo();
    }
}
