using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAddressManager : MonoBehaviour
{
    public SQLManager sqlManager;
    public GameObject storeScreen;
    public Text storeName;
    public Text streetNum;
    public Text streetName;
    public Text city;
    public Text state;
    public Text country;
    public Text zipcode;

    public void Start()
    {
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
        string[] values = sqlManager.ReadSQLValuesAddress(sqlManager.addressId);
        storeName.text = sqlManager.storeName;
        streetNum.text = values[0];
        streetName.text = values[1];
        city.text = values[2];
        state.text = values[3];
        country.text = values[4];
        zipcode.text = values[5];
    }

    public void XButton()
    {
        gameObject.SetActive(false);
        storeScreen.SetActive(true);
    }
}
