using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("LevelProcedural");
    }

    public void Quit()
    {
       Application.Quit();
    }
}
