using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TableSceneManager : MonoBehaviour
{
    public Transform contentContainer;
    public GameObject storePrefab;
    public GameObject createStorePopup;
    public SQLManager sqlManager;
    public GameObject tableScreen;
    public GameObject editStorePopup;

    private List<String> storeNames;
    public List<int> addressIds;

    private void Start()
    {
        tableScreen.SetActive(true);
        createStorePopup.SetActive(false);

        storeNames = new List<String>();
        addressIds = new List<int>();

        sqlManager.CreateAndOpenTable("StoresTable");

        IDbConnection dbConnection = sqlManager.CreateAndOpenDatabase();
        IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
        dbCommandReadValues.CommandText = "SELECT * FROM StoresTable";
        IDataReader dataReader = dbCommandReadValues.ExecuteReader();

        while (dataReader.Read())
        {
            addressIds.Add(dataReader.GetInt32(0));
            storeNames.Add(dataReader.GetString(1));
        }

        dbConnection.Close();
        PopulateStoreScrollView();
    }

    private void PopulateStoreScrollView()
    {
        for (int i = 0; i < storeNames.Count; i++)
        {
            var storeItem = Instantiate(storePrefab);

            storeItem.GetComponentInChildren<Text>().text = hexToString(storeNames[i]);
            storeItem.GetComponent<StoreButtonScript>().addressId = addressIds[i];
            storeItem.GetComponent<StoreButtonScript>().sqlManager = sqlManager;
            storeItem.GetComponent<StoreButtonScript>().editStorePopup = editStorePopup;
            storeItem.GetComponent<StoreButtonScript>().tableScreen = tableScreen;

            storeItem.transform.SetParent(contentContainer);
            storeItem.transform.localScale = Vector2.one;
        }
    }

    public void OpenCreateStorePopupButton()
    {
        createStorePopup.SetActive(true);
        tableScreen.SetActive(false);
    }

    public void XButton()
    {
        Application.Quit();
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
