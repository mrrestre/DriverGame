using UnityEngine;

public class GetCarTanked : MonoBehaviour
{
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.findImageBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Car")
        {
            FindObjectOfType<AudioController>().PlaySoundByName("TankCar");
            healthBar.setHealthBarFull();
        }
    }
}
