using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class AddItemManager : MonoBehaviour
{
    public InputField itemName;
    public InputField price;
    public Text errorText;
    public GameObject viewStoreScreen;
    public SQLManager sqlManager;
    public StoreSceneManager storeSceneManager;

    public void Awake()
    {
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
    }

    public void XButton()
    {
        itemName.text = "";
        price.text = "";
        errorText.text = "";
        gameObject.SetActive(false);
        viewStoreScreen.SetActive(true);
    }

    public void CreateItemButton()
    {
        if (itemName.text == "")
        {
            errorText.text = "Item name must not be blank!";
        } else if (price.text == "")
        {
            errorText.text = "Item price must not be blank!";
        } else
        {
            string hexItemName = sqlManager.stringToHex(itemName.text.ToUpper());
            float truePrice = float.Parse(price.text, CultureInfo.InvariantCulture.NumberFormat);
            List<string> itemNames = storeSceneManager.itemNames;
            if (itemNames.Contains(hexItemName))
            {
                errorText.text = "Item name already exists!";
            }
            else
            {
                sqlManager.ExecuteSQLCommand("INSERT OR REPLACE INTO Items (itemname) VALUES ( \'" + hexItemName + "\')");
                sqlManager.ExecuteSQLCommand("INSERT INTO ItemPrices (itemname, addressid, price) VALUES ( \'" + hexItemName + "\', " + sqlManager.addressId + ", " + truePrice + ")");
                sqlManager.SceneSwitch("StoreScene", "StoreScene");
            }
        }
    }
}
