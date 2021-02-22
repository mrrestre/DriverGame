using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SingleMission : MonoBehaviour
{
    ////////////For all Missions important////////////

    [Header("All Missions")]

    //Where does the mission starts
    public GameObject startObject;

    //Skybox content
    public GameObject skyBoxObject;
    public SkyBox skyBoxScript;

    //How long does the player has to complete the mission
    public int timeForMission;

    //Starts as 0, after the mission is done, the countdown script updates this value
    public int completionTime;

    //If the mission already started
    public bool hasMissionStarted = false;

    //If the mission is finished
    public bool hasMissionEnded = false;

    //The type of the mission enum
    public MissionType missionType;

    //Value to turn on the enemies on a mission if any
    public bool isThereEnemies;

    //End screen for the given mission
    public GameObject endMissionScreen;
    public GameObject endMissionTimeText;
    public GameObject endMissionStars;

    //How long the end mission overview is shown
    private int endMissionScreenTimer = 3;

    //Empty game Object where all the enemies of a given mission are child objects
    public GameObject enemiesFolder;
    public GameObject secretStartScreen;

    ////////////Depending of the mission may be null////////////

    [Header("Simple Mission")]

    //Just relevant for the "simpleMission"
    public GameObject endObject;

    [Header("Mission Ordered Objectives")]

    //Relevant for the "simpleMissionArrayOfObjectives"
    //Each objective should have the script "SingleObject.cs"
    public GameObject[] objectivesList;
    public int objectiveCounter = 0;

    [Header("Mission Unodered Objectives")]

    //For the mission to catch objectives whitout a given order
    public List<GameObject> unorderedObjectives;
    public int unorderedObjectiveCounter = 0;


    void Start()
    {
        skyBoxScript = skyBoxObject.GetComponent<SkyBox>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startObject.GetComponent<StartMissionTrigger>().hasMissionStarted == true)
        {
            this.hasMissionStarted = true;
        }
        else if (startObject.GetComponent<StartMissionTrigger>().hasMissionStarted == false)
        {
            this.hasMissionStarted = false;
        }

        if (missionType == MissionType.simpleMission)
        {
            if (endObject.GetComponent<EndMissionTrigger>().hasMissionEnded == true)
            {
                this.hasMissionEnded = true;
            }
        }

        if (endMissionScreen.gameObject.activeInHierarchy)
        {
            StartCoroutine(LateCall());
        }
    }

    //Method for the representation of the mission overview
    public void ActiveEndMissionScreen()
    {
        endMissionScreen.SetActive(true);
        endMissionTimeText.GetComponent<TextMeshProUGUI>().text = completionTime.ToString() + " s";

        endMissionStars.transform.GetChild(0).gameObject.SetActive(true);
        
        if(completionTime <= timeForMission * 0.8f)
        {
            endMissionStars.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (completionTime <= timeForMission * 0.5f)
        {
            endMissionStars.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    //Coroutine for EndScreen to disappear after "endMissionScreenTimer" seconds
    IEnumerator LateCall()
    {
        Time.timeScale = 0.8f;
        yield return new WaitForSeconds(endMissionScreenTimer);
        endMissionScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
    }


    //Enum for the definition of the mission type
    public enum MissionType
    {
        simpleMission,                          //From point A to point B
        missionArrayOfObjectivesOrdered,        //From point A to point B through a list of points
        missionArrayOfObjectivesDissordered
    }
}
