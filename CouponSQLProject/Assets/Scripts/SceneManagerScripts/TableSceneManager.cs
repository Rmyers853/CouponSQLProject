using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TableSceneManager : MonoBehaviour
{
    public Transform contentContainer;
    public GameObject storePrefab;
    public GameObject createStorePopup;
    private SQLManager sqlManager;
    public GameObject tableScreen;
    public GameObject editStorePopup;
    public GameObject addressPopup;

    private List<String> storeNames;
    public List<int> addressIds;

    /*private void Awake()
    {
        SceneManager.LoadScene("SQLManagerScene", LoadSceneMode.Additive);
    }*/

    private void Start()
    {
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
        tableScreen.SetActive(true);
        addressPopup.SetActive(false);
        createStorePopup.SetActive(false);

        storeNames = new List<String>();
        addressIds = new List<int>();

        sqlManager.ReadSQLValuesIntegers("SELECT addressid FROM StoresTable;", addressIds, 0);

        sqlManager.ReadSQLValuesStrings("SELECT storename FROM StoresTable;", storeNames, 0);

        PopulateStoreScrollView();
    }

    private void PopulateStoreScrollView()
    {
        for (int i = 0; i < storeNames.Count; i++)
        {
            var storeItem = Instantiate(storePrefab);

            //storeItem.GetComponentInChildren<Text>().text = sqlManager.hexToString(storeNames[i]);
            storeItem.GetComponent<StoreButtonScript>().storeButtonText.text = sqlManager.hexToString(storeNames[i]);
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

    public void BackButton()
    {
        sqlManager.SceneSwitch("TableScene", "HomeScene");
    }

    public void XButton()
    {
        Application.Quit();
    }
}
