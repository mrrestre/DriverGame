using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    ////////////All Mission////////////

    [Header("All Missions")]

    //Missions pool
    public List<SingleMission> missionsList = new List<SingleMission>();

    //Mission Counter
    private int missionCounter = 0;

    ////////////Current Mission////////////

    [Header("Current Missions")]

    //Change during the game
    public SingleMission currentMission;


    ////////////Done Missions////////////

    [Header("Done Missions")]

    //Done Mission list
    public List<SingleMission> doneMissions = new List<SingleMission>();

    public bool allRegularMissionsDone = false;

    ////////Secret Mission Scripts////////

    [Header("Secret Mission")]

    public SingleMission secretMission;
    public bool secretMissionActivated = false;

    ////////External Scripts////////

    [Header("External Components")]

    //Countdown Logic
    public GameObject countDownGameObject;
    public CountdownTimer countdown;

    //Waypoint Logic
    public GameObject wayPointGameObject;
    public WayPoint waypoint;

    //Save system
    public GameObject proccessControllerGameObject;
    private ProccessController proccessController;

    //Player
    public GameObject player;
    private CarController carController;

    //EndMissionScreen
    public GameObject endGameScreen;
    private EndGameMenu endGameMenu;

    ///////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        countdown           = countDownGameObject.GetComponent<CountdownTimer>();
        waypoint            = wayPointGameObject.GetComponent<WayPoint>();
        proccessController  = proccessControllerGameObject.GetComponent<ProccessController>();
        carController       = player.GetComponent<CarController>();
        endGameMenu         = endGameScreen.GetComponent<EndGameMenu>();

        StartNextMission();
    }

    // Update is called once per frame
    void Update()
    {
        if(carController.areAllCoinsCollected)
        {
            secretMissionActivated = true;
        }

        //If there are still missions to be done
        if (!allRegularMissionsDone)
        {
            //If a mission has been started (Start element was touched with the car)
            if (currentMission.hasMissionStarted == true)
            {
                //Deactivate the start element on map
                currentMission.startObject.gameObject.SetActive(false);

                //Start Timer
                if (!countdown.hasMissionStarted)
                {
                    countdown.StartCountdown(currentMission.timeForMission);
                }

                //If the mission has any enemies, activate them
                if (currentMission.isThereEnemies == true)
                {
                    currentMission.enemiesFolder.SetActive(true);
                }

                //Define start procedure for each mission type
                switch (currentMission.missionType)
                {
                    //Mission is to go from point A to point B
                    case SingleMission.MissionType.simpleMission:

                        //Activate the end element on map
                        currentMission.endObject.gameObject.SetActive(true);

                        //Set WayPoint end to the end element
                        waypoint.setTarget(currentMission.endObject.transform);

                        break;

                    //Go through a list of objects in a given order
                    case SingleMission.MissionType.missionArrayOfObjectivesOrdered:

                        //Activate the next posible objective
                        currentMission.objectivesList[currentMission.objectiveCounter].gameObject.SetActive(true);

                        //Set the waypoint to the next objective
                        waypoint.setTarget(currentMission.objectivesList[currentMission.objectiveCounter].transform);

                        //Check if the next objective is reached
                        if (currentMission.objectivesList[currentMission.objectiveCounter].GetComponent<SingleObject>().objectTouched == true)
                        {
                            //Deactivate the objective
                            currentMission.objectivesList[currentMission.objectiveCounter].gameObject.SetActive(false);

                            //All objectives from the mission are done
                            if (currentMission.objectiveCounter == currentMission.objectivesList.Length - 1)
                            {
                                currentMission.hasMissionEnded = true;
                            }
                            else
                            {
                                //Set the next objective
                                currentMission.objectiveCounter++;
                            }
                        }

                        break;

                    //Go through a list of objects in any order (Hit enemies for example)
                    case SingleMission.MissionType.missionArrayOfObjectivesDissordered:

                        //Disable the way point
                        waypoint.gameObject.SetActive(false);

                        //Every objective has been done
                        if (currentMission.unorderedObjectives.Count == 0)
                        {
                            currentMission.hasMissionEnded = true;
                        }
                        else
                        {
                            //Go through every element
                            for (int i = 0; i < currentMission.unorderedObjectives.Count; i++)
                            {

                                //If an objective was touched increase the counter
                                if (currentMission.unorderedObjectives[i].GetComponent<SingleObject>().objectTouched == true)
                                {
                                    //Disable that objective
                                    currentMission.unorderedObjectives[i].SetActive(false);

                                    //Remove the objective from the list
                                    currentMission.unorderedObjectives.RemoveAt(i);

                                    //Increase the counter
                                    currentMission.unorderedObjectiveCounter++;

                                }
                                else
                                {
                                    //If objective not yet reached, it stays active
                                    currentMission.unorderedObjectives[i].SetActive(true);

                                }
                            }
                        }
                        break;
                }
            }

            //Was the mission end element touched, then the mission is done
            if (currentMission.hasMissionEnded == true)
            {
                FindObjectOfType<AudioController>().PlaySoundByName("EndMissionSound");

                //Add mission to the done mission
                this.doneMissions.Add(currentMission);

                Debug.Log("Mission " + currentMission.missionType + " Ended!");

                //Write how long was needed to complete the mission
                currentMission.completionTime = (int)countdown.elapsedTime;
                Debug.Log("Time needed to end mission " + currentMission.completionTime);

                //Stop Timer
                countdown.StopCountdown();

                //If the mission has any enemies, deactivate them
                if (currentMission.isThereEnemies == true)
                {
                    currentMission.enemiesFolder.SetActive(false);
                }

                //Deactivate the end point if it applies
                switch (currentMission.missionType)
                {
                    //Mission is to go from point A to point B
                    case SingleMission.MissionType.simpleMission:
                        currentMission.endObject.gameObject.SetActive(false);
                        break;
                }

                //Increment the mission counter
                this.missionCounter++;


                //Show End Game Screen (If it isn´t the last mission)
                if (this.missionCounter != this.missionsList.Count)
                {
                    currentMission.ActiveEndMissionScreen();
                    carController.ResetAllMovementValues();
                }
                //Show all mission overview if that was the last mission
                else if (this.missionCounter == this.missionsList.Count && !secretMissionActivated)
                {
                    Debug.Log("You Completed all missions!");

                    //Set the all missions done boolean to true
                    this.allRegularMissionsDone = true;

                    endGameMenu.ActivateScreen();
                }
                //Only if all missions are played and the secret mission is active
                else
                {
                    this.allRegularMissionsDone = true;

                    currentMission.ActiveEndMissionScreen();

                    //Save the current state
                    proccessController.SaveGame(player.transform.position,
                                                 player.transform.rotation,
                                                 carController.GetCollectedCoins(),
                                                 this.doneMissions,
                                                 carController.healthBar.getCurrentHealth());
                    //Start the secret Mission
                    StartSecretMission();

                    //Stop the following steps
                    return;
                }


                //Save the current state
                proccessController.SaveGame(player.transform.position,
                                             player.transform.rotation,
                                             carController.GetCollectedCoins(),
                                             this.doneMissions,
                                             carController.healthBar.getCurrentHealth());

                //Start a new mission (If not all missions are done)
                StartNextMission();
            }
        }

        else if (allRegularMissionsDone && secretMissionActivated)
        {
            if (currentMission.hasMissionStarted == true)
            {
                //Deactivate the start element on map
                currentMission.startObject.gameObject.SetActive(false);

                //Start Timer
                if (!countdown.hasMissionStarted)
                {
                    countdown.StartCountdown(currentMission.timeForMission);
                }

                //If the mission has any enemies, activate them
                if (currentMission.isThereEnemies == true)
                {
                    currentMission.enemiesFolder.SetActive(true);
                }

                //Activate the next posible objective
                currentMission.objectivesList[currentMission.objectiveCounter].gameObject.SetActive(true);

                //Set the waypoint to the next objective
                waypoint.setTarget(currentMission.objectivesList[currentMission.objectiveCounter].transform);

                //Check if the next objective is reached
                if (currentMission.objectivesList[currentMission.objectiveCounter].GetComponent<SingleObject>().objectTouched == true)
                {
                    //Deactivate the objective
                    currentMission.objectivesList[currentMission.objectiveCounter].gameObject.SetActive(false);

                    //All objectives from the mission are done
                    if (currentMission.objectiveCounter == currentMission.objectivesList.Length - 1)
                    {
                        currentMission.hasMissionEnded = true;
                    }
                    else
                    {
                        //Set the next objective
                        currentMission.objectiveCounter++;
                    }
                }
            }

            if (currentMission.hasMissionEnded == true)
            {
                Debug.Log("Secret Mission Done!");

                //Write how long was needed to complete the mission
                currentMission.completionTime = (int)countdown.elapsedTime;
                Debug.Log("Time needed to end secret mission " + currentMission.completionTime);

                //Stop Timer
                countdown.StopCountdown();

                //If the mission has any enemies, deactivate them
                if (currentMission.isThereEnemies == true)
                {
                    currentMission.enemiesFolder.SetActive(false);
                }

                endGameMenu.ActivateScreen();
            }
        }
    }

    void StartNextMission()
    {
        //If all Missions are done
        if(this.missionCounter != this.missionsList.Count)
        {
            //Set the current mission to the next posible mission
            currentMission = missionsList[missionCounter];
            
            Debug.Log("Next Mission is: " + currentMission.missionType);

            //Set start trigger active
            currentMission.startObject.SetActive(true);

            //Set Waypoint to the start object of the next mission
            waypoint.setTarget(currentMission.startObject.transform);

            currentMission.skyBoxScript.SetSkyBoxSettings();
        }
    }   

    void StartSecretMission()
    {
        FindObjectOfType<AudioController>().PauseSoundWithName("Background1");
        FindObjectOfType<AudioController>().PlaySoundByName("SpookyTheme");

        currentMission = secretMission;

        Debug.Log("Next Mission is the secret Mission");

        //Set start trigger active
        currentMission.startObject.SetActive(true);

        //Set Waypoint to the start object of the next mission
        waypoint.setTarget(currentMission.startObject.transform);

        currentMission.skyBoxScript.SetSkyBoxSettings();
    }

    
}
