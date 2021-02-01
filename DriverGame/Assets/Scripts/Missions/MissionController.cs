using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    //Missions pool
    public List<SingleMission> missionsList = new List<SingleMission>();

    //Mission Counter
    private int missionCounter = 0;

    //Change during the game
    public SingleMission currentMission;

    private bool allMissionsDone = false;

    ////////External Scripts////////

    //Countdown Logic
    public GameObject countDownGameObject;
    private CountdownTimer countdown;

    //Waypoint Logic
    public GameObject wayPointGameObject;

    private WayPoint waypoint;

    // Start is called before the first frame update
    void Start()
    {
        countdown  = countDownGameObject.GetComponent<CountdownTimer>();
        waypoint   = wayPointGameObject.GetComponent<WayPoint>();

        startNextMission();
    }

    // Update is called once per frame
    void Update()
    {
        //If there are still missions to be done
        if(!allMissionsDone)
        {
            //If a mission has been started (Start element was touched with the car)
            if (currentMission.hasMissionStarted == true)
            {
                //Deactivate the start element on map
                currentMission.startObject.gameObject.SetActive(false);

                //Start Timer
                countdown.startTimer(currentMission.timeForMission);

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
                            if(currentMission.objectiveCounter == currentMission.objectivesList.Length - 1)
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
                Debug.Log("Mission " + currentMission.missionType + " Ended!");

                //Stop Timer
                countdown.stopTimer();

                //Write how long was needed to complete the mission
                currentMission.completionTime = (int)countdown.timer.Elapsed.TotalSeconds;
                Debug.Log("Time needed to end mission " + currentMission.completionTime);

                //If the mission has any enemies, activate them
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

                //Show Mission Overview (If it isn´t the last mission)
                if(this.missionCounter != this.missionsList.Count)
                {
                    currentMission.activeEndMissionScreen();
                }
                //Show all mission overview if that was the last mission
                else
                {
                    Debug.Log("You Completed all missions!");

                    //Set the all missions done boolean to true
                    this.allMissionsDone = true;

                    //TODO: create end screen
                    Time.timeScale = 0f;
                }


                //Start a new mission (If not all missions are done)
                startNextMission();
            }
        }
    }

    void startNextMission()
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
        }
    }   
}
