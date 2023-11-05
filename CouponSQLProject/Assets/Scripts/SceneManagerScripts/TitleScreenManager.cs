using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("TableScene");
    }

    public void XButton()
    {
        Application.Quit();
    }
}
