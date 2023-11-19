using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreButtonScript : MonoBehaviour
{
    public int addressId;
    public SQLManager sqlManager;
    public GameObject editStorePopup;
    public GameObject tableScreen;
    private EditStoreManager editStoreManager;

    public void DeleteButton()
    {
        sqlManager.ExecuteSQLCommand("DELETE FROM StoresTable WHERE addressId = " + addressId);
        sqlManager.ExecuteSQLCommand("DELETE FROM Addresses WHERE addressId = " + addressId);
        SceneManager.LoadScene("TableScene");
    }

    public void EditButton()
    {
        tableScreen.SetActive(false);
        editStorePopup.SetActive(true);
        editStoreManager = editStorePopup.GetComponent<EditStoreManager>();
        editStoreManager.currentAddressId = addressId;
        editStoreManager.loadStoreInfo();
    }
}
