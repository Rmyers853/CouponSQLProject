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
        else
        {
            string hexStoreName = stringToHex(storeName.text);
            float trueDistance = float.Parse(distance.text, CultureInfo.InvariantCulture.NumberFormat);
            sqlManager.ExecuteSQLCommand("REPLACE INTO StoresTable (addressid, storename, distance) VALUES ( " + currentAddressId + ", " + "\'" + hexStoreName + "\', " + trueDistance + ")");
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
