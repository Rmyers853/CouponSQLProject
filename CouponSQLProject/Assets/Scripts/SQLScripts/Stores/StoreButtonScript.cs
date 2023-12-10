using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreButtonScript : MonoBehaviour
{
    public int addressId;
    public SQLManager sqlManager;
    public GameObject editStorePopup;
    public GameObject tableScreen;
    private EditStoreManager editStoreManager;
    public Text storeButtonText;

    public void DeleteButton()
    {
        sqlManager.ExecuteSQLCommand("DELETE FROM StoresTable WHERE addressId = " + addressId);
        sqlManager.ExecuteSQLCommand("DELETE FROM ItemPrices WHERE addressId = " + addressId);
        sqlManager.ExecuteSQLCommand("DELETE FROM Addresses WHERE addressId = " + addressId);
        sqlManager.SceneSwitch("TableScene", "TableScene");
    }

    public void EditButton()
    {
        tableScreen.SetActive(false);
        editStorePopup.SetActive(true);
        editStoreManager = editStorePopup.GetComponent<EditStoreManager>();
        editStoreManager.currentAddressId = addressId;
        editStoreManager.loadStoreInfo();
    }

    public void ViewButton()
    {
        sqlManager.storeName = storeButtonText.text;
        sqlManager.addressId = addressId;
        sqlManager.SceneSwitch("TableScene", "StoreScene");
    }
}
