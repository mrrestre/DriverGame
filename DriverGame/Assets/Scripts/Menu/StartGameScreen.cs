using UnityEngine;

public class StartGameScreen : MonoBehaviour
{
    public GameObject canvas;
    public GameObject UIElement;

    // Start is called before the first frame update
    void Start()
    {
        UIElement.SetActive(true);
        canvas.SetActive(false);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            canvas.SetActive(true);
            Time.timeScale = 1f;
            UIElement.SetActive(false);
            FindObjectOfType<CarSoundController>().TurnOnTheCar();
            Destroy(this);
        }
    }
}
