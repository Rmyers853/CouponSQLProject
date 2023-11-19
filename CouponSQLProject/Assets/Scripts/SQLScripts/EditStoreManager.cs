using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
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

    public void loadStoreInfo()
    {
        IDbConnection dbConnection = sqlManager.CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT * FROM StoresTable WHERE addressId = " + currentAddressId;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            storeName.text = hexToString(dataReader.GetString(1));
            distance.text = dataReader.GetFloat(2).ToString();
        }
        dbConnection.Close();
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
            string hexStoreName = stringToHex(storeName.text);
            float trueDistance = float.Parse(distance.text, CultureInfo.InvariantCulture.NumberFormat);
            string hexStreetName = stringToHex(streetName);
            string hexCity = stringToHex(city);
            string hexState = stringToHex(state);
            string hexCountry = stringToHex(country);
            sqlManager.ExecuteSQLCommand("REPLACE INTO StoresTable (addressid, storename, distance) VALUES ( " + currentAddressId + ", " + "\'" + hexStoreName + "\', " + trueDistance + ")");
            sqlManager.ExecuteSQLCommand("REPLACE INTO Addresses (addressid, streetnum, streetname, city, state, country, zipcode) VALUES( "
                                          + currentAddressId + ", " + streetNum + ", " + "\'" + hexStreetName + "\', " + "\'" + hexCity + "\', "
                                          + "\'" + hexState + "\', " + "\'" + hexCountry + "\', " + zipcode + ")");
            gameObject.SetActive(false);
            SceneManager.LoadScene("TableScene");
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

    public String stringToHex(string stringToConvert)
    {
        byte[] ba = Encoding.Default.GetBytes(stringToConvert);
        return BitConverter.ToString(ba).Replace("-", "");
    }

    public String hexToString(string hexString)
    {
        byte[] raw = new byte[hexString.Length / 2];
        for (int i = 0; i < raw.Length; i++)
        {
            raw[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }

        return Encoding.ASCII.GetString(raw);
    }
}
