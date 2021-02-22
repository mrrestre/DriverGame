using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject canvas;

    public void ActivateScreen()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        canvas.SetActive(false);
        FindObjectOfType<AudioController>().PauseSoundWithName("EngineSound");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
