using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFailedMenu : MonoBehaviour
{    
    public GameObject pauseMenuUI;
    public GameObject proccessControllerOB;
    private ProccessController proccessController;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        proccessController = proccessControllerOB.GetComponent<ProccessController>();
    }

    public void ActivateScreen()
    {
        FindObjectOfType<AudioController>().PlaySoundByName("GameOver");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        canvas.SetActive(false);
        FindObjectOfType<AudioController>().PauseSoundWithName("EngineSound");
    }

    public void LoadLastSavedgame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        canvas.SetActive(true);
        proccessController.LoadGame();
    }
}
