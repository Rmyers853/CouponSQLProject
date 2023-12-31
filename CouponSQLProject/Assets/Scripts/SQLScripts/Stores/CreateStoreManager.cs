using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CreateStoreManager : MonoBehaviour
{
    public GameObject tableScreen;
    public SQLManager sqlManager;
    public TableSceneManager tableSceneManager;
    public InputField storeName;
    public InputField distance;
    public Text errorText;
    public GameObject addressPopup;
    public GameObject createStorePopup;

    public int streetNum;
    public string streetName;
    public string city;
    public string state;
    public string country;
    public int zipcode;

    public void Awake()
    {
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
    }

    public void XButton()
    {
        storeName.text = "";
        distance.text = "";
        errorText.text = "";
        gameObject.SetActive(false);
        tableScreen.SetActive(true);
    }

    public void AddressButton()
    {
        addressPopup.SetActive(true);
        errorText.text = "";
        addressPopup.GetComponent<AddressManager>().isCreateStore = true;
        addressPopup.GetComponent<AddressManager>().initializeValues();
        if (storeName.text != "")
        {
            addressPopup.GetComponent<AddressManager>().storeName = storeName.text;
        }
        if (distance.text != "")
        {
            addressPopup.GetComponent<AddressManager>().distance = float.Parse(distance.text, CultureInfo.InvariantCulture.NumberFormat);
        }
        createStorePopup.SetActive(false);
    }

    public void CreateStoreButton()
    {
        addressPopup.GetComponent<AddressManager>().isCreateStore = true;
        if (storeName.text == "")
        {
            errorText.text = "Store name must not be blank!";
        } else if (distance.text == "")
        {
            errorText.text = "Distance must not be blank!";
        }
        else if (addressPopup.GetComponent<AddressManager>().checkIfInitialized() == false)
        {
            errorText.text = "Address must not be blank!";
        }
        else
        {
            string hexStoreName = sqlManager.stringToHex(storeName.text.ToUpper());
            float trueDistance = float.Parse(distance.text, CultureInfo.InvariantCulture.NumberFormat);
            string hexStreetName = sqlManager.stringToHex(streetName);
            string hexCity = sqlManager.stringToHex(city);
            string hexState = sqlManager.stringToHex(state);
            string hexCountry = sqlManager.stringToHex(country);
            List<int> addressIds = tableSceneManager.addressIds;
            addressIds.Sort();
            int newAddressId = 0;
            int addressIdIndex = 0;
            if (addressIds.Count != 0)
            {
                while (addressIds[addressIdIndex] == newAddressId)
                {
                    addressIdIndex++;
                    newAddressId++;
                    if (addressIdIndex == addressIds.Count)
                    {
                        break;
                    }
                }
            }
            sqlManager.ExecuteSQLCommand("INSERT INTO Addresses (addressid, streetnum, streetname, city, state, country, zipcode) VALUES ( "
                                          + newAddressId + ", " + streetNum + ", " + "\'" + hexStreetName + "\', " + "\'" + hexCity + "\', "
                                          + "\'" + hexState + "\', " + "\'" + hexCountry + "\', " + zipcode + ")");
            sqlManager.ExecuteSQLCommand("INSERT INTO StoresTable (addressid, storename, distance) VALUES ( " + newAddressId + ", " + "\'" + hexStoreName + "\', " + trueDistance + ")");
            sqlManager.SceneSwitch("TableScene", "TableScene");
        }
    }
}
