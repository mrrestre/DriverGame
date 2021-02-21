using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProccessController : MonoBehaviour
{
    //Singleton Implementation
    public static ProccessController instance;

    //Saves the current Position of the car, it start where the car normally starts
    private Vector3 autoPosition { get; set; }

    //Saves the rotation of the car
    private Quaternion autoRotation { get; set; }

    //Saves how many coins the player has collected
    [SerializeField] private List<GameObject> collectedCoins = new List<GameObject>();

    //Saves which missions are already done
    private List<SingleMission> doneMissions { get; set; }

    //Saves the current health of the player
    private float currentHealth { get; set; }

    //Reference to the player
    public GameObject player;
    private CarController carController;

    //Reference to the sphere that moves the car
    public GameObject playerSphere;

    //Reference to the mission Controller
    public GameObject missionControllerObject;
    private MissionController missionController;

    //Reference to the all the coins
    public Transform coinsFolder;
    public List<GameObject> allCoins;
    [SerializeField] private TextMeshProUGUI textAllCoins;

    //Singleton Implementation
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    // Write the staring data of the game
    void Start()
    {
        missionController = missionControllerObject.GetComponent<MissionController>();
        carController = player.GetComponent<CarController>();

        autoPosition = new Vector3( carController.gameObject.transform.position.x, 
                                    carController.gameObject.transform.position.y, 
                                    carController.gameObject.transform.position.z);

        autoRotation = new Quaternion(0f, 0f, 0f, 0f);
        collectedCoins = new List<GameObject>();
        currentHealth = 1f;

        foreach (Transform child in coinsFolder)
        {
            allCoins.Add(child.gameObject);
        }

        textAllCoins.text = "/" + allCoins.Count.ToString();

        ActiveNotCollectedCoins();
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
        carController.ResetAllMovementValues();

        carController.SetCollectedCoins(this.collectedCoins);
        carController.SetCollectedCoinsCounter(this.collectedCoins.Count);

        carController.healthBar.setHealthBarValue(this.currentHealth);

        missionController.doneMissions = this.doneMissions;

        ActiveNotCollectedCoins();

        if(missionController.currentMission.hasMissionStarted)
        {
            Debug.Log("Mission Was Running");
            missionController.currentMission.startObject.GetComponent<StartMissionTrigger>().hasMissionStarted = false;
            missionController.countdown.StopCountdown();
            missionController.currentMission.startObject.SetActive(true);
            missionController.waypoint.setTarget(missionController.GetComponent<MissionController>().currentMission.startObject.transform);
        }
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
