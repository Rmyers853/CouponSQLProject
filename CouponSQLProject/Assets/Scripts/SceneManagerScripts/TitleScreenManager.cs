using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    public DatabaseReference DBreference;
    public SQLManager sqlManager;

    public void Start()
    {
        sqlManager.CreateAndOpenTable("StoresTable");
        sqlManager.CreateAndOpenTable("Addresses");
        sqlManager.CreateAndOpenTable("ItemPrices");
        sqlManager.CreateAndOpenTable("Items");
        sqlManager.CreateAndOpenTable("GroceryList");
    }

    public void StartButton()
    {
        //DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        //StartCoroutine(testFunc());
        sqlManager.SceneSwitch("TitleScene", "HomeScene");
    }

    public void XButton()
    {
        Application.Quit();
    }

    private IEnumerator testFunc()
    {
        var newPostKey = DBreference.Child("test/").Push().Key;

        // Write the new post's data simultaneously in the posts list and the user's post list.

        //return firebase.database().ref ().update(updates);

        var DBTask = DBreference.Child("test").SetValueAsync("Hello");
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update with {DBTask.Exception}");
        }
    }
}
