using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneManager : MonoBehaviour
{
    public SQLManager sqlManager;

    public void Start()
    {
        sqlManager = GameObject.FindGameObjectWithTag("SQLManager").GetComponent<SQLManager>();
    }

    public void GroceryButton()
    {
        sqlManager.SceneSwitch("HomeScene", "GroceryListScene");
    }

    public void TitleButton()
    {
        sqlManager.SceneSwitch("HomeScene", "TitleScene");
    }

    public void StoreButton()
    {
        sqlManager.SceneSwitch("HomeScene", "TableScene");
    }
}
