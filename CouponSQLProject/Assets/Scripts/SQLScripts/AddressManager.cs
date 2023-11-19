using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AddressManager : MonoBehaviour
{
    public InputField streetNum;
    public InputField streetName;
    public InputField city;
    public InputField state;
    public InputField country;
    public InputField zipcode;

    public SQLManager sqlManager;
    public GameObject addressPopup;
    public GameObject createPopup;
    public GameObject editPopup;
    public Text errorText;
    public bool isCreateStore;

    public string storeName;
    public float distance;
    public int currentAddressId;

    public void loadStoreInfo()
    {
        IDbConnection dbConnection = sqlManager.CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT * FROM Addresses WHERE addressId = " + currentAddressId;
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();
        while (dataReader.Read())
        {
            streetNum.text = dataReader.GetInt32(1).ToString();
            streetName.text = hexToString(dataReader.GetString(2));
            city.text = hexToString(dataReader.GetString(3));
            state.text = hexToString(dataReader.GetString(4));
            country.text = hexToString(dataReader.GetString(5));
            zipcode.text = dataReader.GetInt32(6).ToString();
        }
        dbConnection.Close();
    }

    public bool checkIfInitialized()
    {
        bool returnValue = true;
        if (isCreateStore)
        {
            if (createPopup.GetComponent<CreateStoreManager>().streetNum == 0)
            {
                returnValue = false;
            }
            else if (createPopup.GetComponent<CreateStoreManager>().streetName == "")
            {
                returnValue = false;
            }
            else if (createPopup.GetComponent<CreateStoreManager>().city == "")
            {
                returnValue = false;
            }
            else if (createPopup.GetComponent<CreateStoreManager>().state == "")
            {
                returnValue = false;
            }
            else if (createPopup.GetComponent<CreateStoreManager>().country == "")
            {
                returnValue = false;
            }
            else if (createPopup.GetComponent<CreateStoreManager>().zipcode == 0)
            {
                returnValue = false;
            }
        }
        else
        {
            if (editPopup.GetComponent<EditStoreManager>().streetNum == 0)
            {
                returnValue = false;
            }
            else if (editPopup.GetComponent<EditStoreManager>().streetName == "")
            {
                returnValue = false;
            }
            else if (editPopup.GetComponent<EditStoreManager>().city == "")
            {
                returnValue = false;
            }
            else if (editPopup.GetComponent<EditStoreManager>().state == "")
            {
                returnValue = false;
            }
            else if (editPopup.GetComponent<EditStoreManager>().country == "")
            {
                returnValue = false;
            }
            else if (editPopup.GetComponent<EditStoreManager>().zipcode == 0)
            {
                returnValue = false;
            }
        }
        return returnValue;
    }
    // Start is called before the first frame update
    public void initializeValues()
    {
        errorText.text = "";
        if (isCreateStore)
        {
            if (checkIfInitialized())
            {
                streetNum.text = createPopup.GetComponent<CreateStoreManager>().streetNum.ToString();
                streetName.text = createPopup.GetComponent<CreateStoreManager>().streetName;
                city.text = createPopup.GetComponent<CreateStoreManager>().city;
                state.text = createPopup.GetComponent<CreateStoreManager>().state;
                country.text = createPopup.GetComponent<CreateStoreManager>().country;
                zipcode.text = createPopup.GetComponent<CreateStoreManager>().zipcode.ToString();
            }
            else
            {
                streetNum.text = "";
                streetName.text = "";
                city.text = "";
                state.text = "";
                country.text = "";
                zipcode.text = "";
            }
        }
        else
        {
            if (checkIfInitialized())
            {
                streetNum.text = editPopup.GetComponent<EditStoreManager>().streetNum.ToString();
                streetName.text = editPopup.GetComponent<EditStoreManager>().streetName;
                city.text = editPopup.GetComponent<EditStoreManager>().city;
                state.text = editPopup.GetComponent<EditStoreManager>().state;
                country.text = editPopup.GetComponent<EditStoreManager>().country;
                zipcode.text = editPopup.GetComponent<EditStoreManager>().zipcode.ToString();
            }
            else
            {
                loadStoreInfo();
            }
        }
    }

    public void XButton()
    {
        addressPopup.SetActive(false);
        if (isCreateStore)
        {
            createPopup.SetActive(true);
        } else
        {
            editPopup.SetActive(true);
        }
    }

    public void saveButton()
    {
        if (streetNum.text == "")
        {
            errorText.text = "Street Number must not be blank!";
        }
        else if (streetName.text == "")
        {
            errorText.text = "Street Name must not be blank!";
        }
        else if (city.text == "")
        {
            errorText.text = "City must not be blank!";
        }
        else if (state.text == "")
        {
            errorText.text = "State must not be blank!";
        }
        else if (country.text == "")
        {
            errorText.text = "Country must not be blank!";
        }
        else if (zipcode.text == "")
        {
            errorText.text = "Zipcode must not be blank!";
        }
        else
        {
            if (isCreateStore)
            {
                createPopup.GetComponent<CreateStoreManager>().streetNum = int.Parse(streetNum.text, CultureInfo.InvariantCulture.NumberFormat);
                createPopup.GetComponent<CreateStoreManager>().streetName = streetName.text;
                createPopup.GetComponent<CreateStoreManager>().city = city.text;
                createPopup.GetComponent<CreateStoreManager>().state = state.text;
                createPopup.GetComponent<CreateStoreManager>().country = country.text;
                createPopup.GetComponent<CreateStoreManager>().zipcode = int.Parse(zipcode.text, CultureInfo.InvariantCulture.NumberFormat);
                createPopup.GetComponent<CreateStoreManager>().storeName.text = storeName;
                createPopup.GetComponent<CreateStoreManager>().distance.text = distance.ToString();
                XButton();
            } else
            {
                editPopup.GetComponent<EditStoreManager>().streetNum = int.Parse(streetNum.text, CultureInfo.InvariantCulture.NumberFormat);
                editPopup.GetComponent<EditStoreManager>().streetName = streetName.text;
                editPopup.GetComponent<EditStoreManager>().city = city.text;
                editPopup.GetComponent<EditStoreManager>().state = state.text;
                editPopup.GetComponent<EditStoreManager>().country = country.text;
                editPopup.GetComponent<EditStoreManager>().zipcode = int.Parse(zipcode.text, CultureInfo.InvariantCulture.NumberFormat);
                editPopup.GetComponent<EditStoreManager>().loadStoreInfo();
                XButton();
            }
        }
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
