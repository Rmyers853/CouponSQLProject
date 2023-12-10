using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class AddGroceryItemManager : MonoBehaviour
{
    public InputField itemName;
    public InputField count;
    public Text errorText;
    public GameObject groceryListScreen;
    public SQLManager sqlManager;
    public GroceryListSceneManager grocerySceneManager;

    public void Awake()
    {
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
    }

    public void XButton()
    {
        itemName.text = "";
        count.text = "";
        errorText.text = "";
        gameObject.SetActive(false);
        groceryListScreen.SetActive(true);
    }

    public void AddItemButton()
    {
        if (itemName.text == "")
        {
            errorText.text = "Item name must not be blank!";
        }
        else if (count.text == "")
        {
            errorText.text = "Item count must not be blank!";
        }
        else
        {
            string hexItemName = sqlManager.stringToHex(itemName.text.ToUpper());
            int trueCount = int.Parse(count.text, CultureInfo.InvariantCulture.NumberFormat);
            List<string> itemNames = grocerySceneManager.itemNames;
            if (itemNames.Contains(hexItemName))
            {
                errorText.text = "Item name already on grocery list!";
            }
            else
            {
                string errorMessage = sqlManager.ExecuteSQLCommand("INSERT INTO GroceryList (itemname, count) VALUES ( \'" + hexItemName + "\', " + trueCount + ")");
                if (errorMessage != null)
                {
                    errorText.text = "Item not sold in any stores!";
                }
                else
                {
                    sqlManager.SceneSwitch("GroceryListScene", "GroceryListScene");
                }
            }
        }
    }
}
