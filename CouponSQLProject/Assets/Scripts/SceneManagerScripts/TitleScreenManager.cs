using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public SQLManager sqlManager;
    public Text successText;

    public void StartButton()
    {
        sqlManager.SceneSwitch("TitleScene", "HomeScene");
    }

    public void DeleteButton()
    {
        sqlManager.DeleteAllData();
        StartCoroutine(SuccessRoutine());
    }

    private IEnumerator SuccessRoutine()
    {
        successText.text = "Delete all data success!";
        yield return new WaitForSecondsRealtime(2);
        Application.Quit();
    }
}
