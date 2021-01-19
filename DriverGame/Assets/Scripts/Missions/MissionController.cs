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

    //Screen where the mission overview is shown
    public GameObject endMissionScreen;

    //How long the end mission overview is shown
    public int endMissionScreenTimer = 4;

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
            if (currentMission.startObject.GetComponent<StartMissionTrigger>().hasMissionStarted == true)
            {
                Debug.Log("Mission " + currentMission.name + " Started!");

                //Deactivate the start element on map
                currentMission.startObject.gameObject.SetActive(false);

                //Activate the end element on map
                currentMission.endObject.gameObject.SetActive(true);

                //Set WayPoint end to the end element
                waypoint.setTarget(currentMission.endObject.transform);

                //Start Timer
                countdown.startTimer(currentMission.timeForMission);
            }

            //Was the mission end element touched, then the mission is done
            if (currentMission.endObject.GetComponent<EndMissionTrigger>().hasMissionEnded == true)
            {
                Debug.Log("Mission " + currentMission.name + " Ended!");

                //Stop Timer
                countdown.stopTimer();

                //Write how long was needed to complete the mission
                currentMission.completionTime = (int)countdown.timer.Elapsed.TotalSeconds;
                Debug.Log("Time needed to end mission " + currentMission.completionTime);

                //Deactivate the end element on map
                currentMission.endObject.gameObject.SetActive(false);

                //Increment the mission counter
                this.missionCounter++;

                //Show Mission Overview (If it isn´t the last mission)
                if(this.missionCounter != this.missionsList.Count)
                {
                    endMissionScreen.gameObject.SetActive(true);
                    /*
                    if (endMissionScreen.gameObject.activeInHierarchy)
                    {
                        StartCoroutine(LateCall());
                    }
                    */
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

            //Set start trigger active
            currentMission.startObject.SetActive(true);

            //Set Waypoint to the start object of the next mission
            waypoint.setTarget(currentMission.startObject.transform);
        }
    }

    /*
    //Coroutine for EndScreen to disappear after "endMissionScreenTimer" seconds
    IEnumerator LateCall()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(endMissionScreenTimer);
        endMissionScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    */
}
