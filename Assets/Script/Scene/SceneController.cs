using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("LevelProcedural");
    }

    public void ToStartingScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MenuScene");
    }

    public void Quit()
    {
       Application.Quit();
    }

    public void Unfreeze()
    {
        GameManager.Instance.IsPaused = false;
        Time.timeScale = 1f;
        GameManager.Instance.pauseMenuUI.gameObject.SetActive(false);
        AudioListener.pause = false;
    }

}
