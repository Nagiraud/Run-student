using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void Quit()
    {
       Application.Quit();
    }
}
