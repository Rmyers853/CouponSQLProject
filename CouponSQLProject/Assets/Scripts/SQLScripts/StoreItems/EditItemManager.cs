using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class EditItemManager : MonoBehaviour
{
    public SQLManager sqlManager;
    public GameObject storeScreen;
    public Text itemName;
    public InputField price;
    public Text errorText;
    public GameObject editPopup;
    public int currentAddressId;
    public string currentItemName;
    public StoreSceneManager storeSceneManager;

    public void Awake()
    {
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
    }

    public void loadItemInfo()
    {
        string[] itemInfo = sqlManager.ReadSQLValuesItemsTable(currentAddressId, currentItemName);
        itemName.text = sqlManager.hexToString(currentItemName);
        price.text = itemInfo[0];
    }

    public void XButton()
    {
        itemName.text = "";
        price.text = "";
        errorText.text = "";
        gameObject.SetActive(false);
        storeScreen.SetActive(true);
    }

    public void SaveButton()
    {

        if (price.text == "")
        {
            errorText.text = "Price must not be blank!";
        }
        else
        {
            string hexItemName = sqlManager.stringToHex(itemName.text.ToUpper());
            float truePrice = float.Parse(price.text, CultureInfo.InvariantCulture.NumberFormat);
            List<string> itemNames = storeSceneManager.itemNames;
            if (itemNames.Contains(hexItemName) && hexItemName != currentItemName)
            {
                errorText.text = "Item name already exists!";
            }
            else
            {
                sqlManager.ExecuteSQLCommand("REPLACE INTO ItemPrices (itemname, addressid, price) VALUES ( \'" + hexItemName + "\', " + currentAddressId + ", " + truePrice + ")");
                gameObject.SetActive(false);
                sqlManager.SceneSwitch("StoreScene", "StoreScene");
            }
        }
    }
}
