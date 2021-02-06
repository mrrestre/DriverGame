using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProccessController : MonoBehaviour
{
    //Saves the current Position of the car, it start where the car normally starts
    private Vector3 autoPosition { get; set; }

    //Saves the rotation of the car
    private Quaternion autoRotation { get; set; }

    //Saves how many coins the player has collected
    private List<GameObject> collectedCoins { get; set; }

    //Saves which missions are already done
    private List<SingleMission> doneMissions { get; set; }

    //Saves the current health of the player
    private float currentHealth { get; set; }

    //Reference to the player
    public GameObject player;

    //Reference to the sphere that moves the car
    public GameObject playerSphere;

    //Reference to the mission Controller
    public GameObject missionController;

    //Reference to the all the coins
    public List<GameObject> allCoins;

    // Start is called before the first frame update
    // Write the staring data of the game
    void Start()
    {
        autoPosition = new Vector3(184.5f, 1.12845f, 6.805f);
        autoRotation = new Quaternion(0f, 0f, 0f, 0f);
        collectedCoins = new List<GameObject>();
        currentHealth = 1f;
    }

    //Function to save the current progress
    public void SaveGame(Vector3 position, Quaternion rotation, List<GameObject> collectedCoins, List<SingleMission> doneMissions, float currentHealth)
    {
        Debug.Log("Saved Game");

        this.autoPosition = position;
        this.autoRotation = rotation;
        this.collectedCoins = collectedCoins;
        this.doneMissions = doneMissions;
        this.currentHealth = currentHealth;
    }

    //Function to load the last saved game
    public void LoadGame()
    {
        Debug.Log("Loaded Last Saved Game");

        playerSphere.transform.position = this.autoPosition;
        
        player.transform.rotation = this.autoRotation;
        player.GetComponent<CarController>().ResetAllMovementValues();

        player.GetComponent<CarController>().collectedCoins = this.collectedCoins;
        player.GetComponent<CarController>().SetCollectedCoinsCounter(this.collectedCoins.Count);
        player.GetComponent<CarController>().healthBar.setHealthBarValue(this.currentHealth);

        missionController.GetComponent<MissionController>().doneMissions = this.doneMissions;

        ActiveNotCollectedCoins();
    }

    public void ActiveNotCollectedCoins()
    {
        for (int i = 0; i < allCoins.Count; i++)
        {
            allCoins[i].SetActive(true);
        }

        for (int i = 0; i < allCoins.Count; i++)
        {
            for (int j = 0; j < collectedCoins.Count; j++)
            {
                if(allCoins[i] == collectedCoins[j])
                {
                    allCoins[i].SetActive(false);
                }
            }
        }
    }
}
