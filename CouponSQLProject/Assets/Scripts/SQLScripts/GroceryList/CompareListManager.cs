using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompareListManager : MonoBehaviour
{
    public SQLManager sqlManager;
    public Transform contentContainer;
    public GameObject itemPrefab;
    public GameObject groceryScreen;
    public Text errorText;
    public List<string> storeNames;
    public List<float> prices;

    public void loadItemInfo()
    {
        storeNames = new List<string>();
        prices = new List<float>();
        sqlManager.CompareStores(storeNames, prices);
        PopulateStoreScrollView();
    }

    private void PopulateStoreScrollView()
    {
        if (storeNames.Count != 0)
        {
            for (int i = 0; i < storeNames.Count; i++)
            {
                var item = Instantiate(itemPrefab);

                item.GetComponent<ItemButtonScript>().itemText.text = sqlManager.hexToString(storeNames[i]);
                item.GetComponent<ItemButtonScript>().priceText.text = "$" + prices[i].ToString();
                item.GetComponent<ItemButtonScript>().priceText.transform.position = new Vector3(0, 0, 0);
                item.GetComponent<ItemButtonScript>().priceText.rectTransform.sizeDelta = new Vector2(890, 100);
                item.GetComponent<ItemButtonScript>().itemName = storeNames[i];
                item.GetComponent<ItemButtonScript>().sqlManager = sqlManager;
                item.GetComponent<ItemButtonScript>().editButton.SetActive(false);
                item.GetComponent<ItemButtonScript>().deleteButton.SetActive(false);

                item.transform.SetParent(contentContainer);
                item.transform.localScale = Vector2.one;
            }
        } else
        {
            errorText.text = "No stores contain full grocery list!";
        }
    }

    public void XButton()
    {
        errorText.text = "";
        gameObject.SetActive(false);
        groceryScreen.SetActive(true);
    }
}
