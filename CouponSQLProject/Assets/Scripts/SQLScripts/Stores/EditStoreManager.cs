using System.Data;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditStoreManager : MonoBehaviour
{
    public GameObject tableScreen;
    public SQLManager sqlManager;
    public InputField storeName;
    public InputField distance;
    public Text errorText;
    public int currentAddressId;
    public GameObject addressPopup;
    public GameObject editPopup;

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

    public void loadStoreInfo()
    {
        string[] storeNameInfo = sqlManager.ReadSQLValuesStoresTable(currentAddressId);
        storeName.text = storeNameInfo[0];
        distance.text = storeNameInfo[1];
    }

    public void AddressButton()
    {
        addressPopup.SetActive(true);
        addressPopup.GetComponent<AddressManager>().isCreateStore = false;
        addressPopup.GetComponent<AddressManager>().currentAddressId = currentAddressId;
        addressPopup.GetComponent<AddressManager>().initializeValues();
        editPopup.SetActive(false);
    }

    public void SaveButton()
    {

        if (storeName.text == "")
        {
            errorText.text = "Store name must not be blank!";
        }
        else if (distance.text == "")
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
            sqlManager.ExecuteSQLCommand("REPLACE INTO StoresTable (addressid, storename, distance) VALUES ( " + currentAddressId + ", " + "\'" + hexStoreName + "\', " + trueDistance + ")");
            sqlManager.ExecuteSQLCommand("REPLACE INTO Addresses (addressid, streetnum, streetname, city, state, country, zipcode) VALUES( "
                                          + currentAddressId + ", " + streetNum + ", " + "\'" + hexStreetName + "\', " + "\'" + hexCity + "\', "
                                          + "\'" + hexState + "\', " + "\'" + hexCountry + "\', " + zipcode + ")");
            gameObject.SetActive(false);
            sqlManager.SceneSwitch("TableScene", "TableScene");
            //SceneManager.LoadScene("TableScene");
        }
    }

    public void XButton()
    {
        storeName.text = "";
        distance.text = "";
        errorText.text = "";
        gameObject.SetActive(false);
        tableScreen.SetActive(true);
    }
}
