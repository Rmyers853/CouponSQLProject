using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System;

public class CreateStoreManager : MonoBehaviour
{
    public GameObject tableScreen;
    public SQLManager sqlManager;
    public TableSceneManager tableSceneManager;
    public InputField storeName;
    public InputField distance;
    public Text errorText;

    public void XButton()
    {
        storeName.text = "";
        distance.text = "";
        errorText.text = "";
        gameObject.SetActive(false);
        tableScreen.SetActive(true);
    }

    public void CreateStoreButton()
    {
        if (storeName.text == "")
        {
            errorText.text = "Store name must not be blank!";
        } else if (distance.text == "")
        {
            errorText.text = "Distance must not be blank!";
        }
        else
        {
            string hexStoreName = stringToHex(storeName.text);
            float trueDistance = float.Parse(distance.text, CultureInfo.InvariantCulture.NumberFormat);
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
            sqlManager.ExecuteSQLCommand("INSERT INTO StoresTable (addressid, storename, distance) VALUES ( " + newAddressId + ", " + "\'" + hexStoreName + "\', " + trueDistance + ")");
            SceneManager.LoadScene("TableScene");
        }
    }

    public String stringToHex(string stringToConvert)
    {
        byte[] ba = Encoding.Default.GetBytes(stringToConvert);
        return BitConverter.ToString(ba).Replace("-", "");
    }
}
