using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    public GameObject[] missionsList;

    public int currentMission;
    public List<int> completedMissions = new List<int>();

    public bool allMissionsDone = false;

    public GameObject mission;

    public GameObject startObject;
    public GameObject endObject;

    public GameObject endScreen;

    public StartMissionTrigger startMissionTrigger;
    public EndMissionTrigger endMissionTrigger;

    // Start is called before the first frame update
    void Start()
    {
        startANewMission();
    }

    // Update is called once per frame
    void Update()
    {
        if(!allMissionsDone)
        {
            if (startMissionTrigger.hasMissionStarted == true)
            {
                Debug.Log("Mission Started!");
                startObject.gameObject.SetActive(false);

                endObject.gameObject.SetActive(true);
            }

            if (endMissionTrigger.hasMissionEnded == true)
            {
                endScreen.gameObject.SetActive(true);

                Debug.Log("Mission Ended!");

                endObject.gameObject.SetActive(false);

                this.completedMissions.Add(this.currentMission);

                startANewMission();
            }
        }
    }

    void startANewMission()
    {
        //If all Missions are done
        if(this.missionsList.Length == this.completedMissions.Count)
        {
            this.allMissionsDone = true;
            //TODO: create end screen
            Debug.Log("You Completed all missions!");
        }
        else
        {
            while (true)
            {
                this.currentMission = Random.Range(0, this.missionsList.Length);
                if (!missionAlreadyDone(this.currentMission))
                {
                    break;
                }
            }

            //Choose a random Mission from the list of missions
            mission = missionsList[this.currentMission];

            //Assign the start and end triggers from the choosen mission
            this.startObject = mission.transform.GetChild(0).gameObject;
            this.endObject = mission.transform.GetChild(1).gameObject;

            //Set start trigger active
            this.startObject.gameObject.SetActive(true);

            //Get the triggers from the start and end objects
            this.startMissionTrigger = startObject.GetComponent<StartMissionTrigger>();
            this.endMissionTrigger = endObject.GetComponent<EndMissionTrigger>();
        }
    }

    bool missionAlreadyDone(int randomMission)
    {
        bool result = false;

        for(int i = 0; i < this.completedMissions.Count; i++)
        {
            if(randomMission == completedMissions[i])
            {
                //Mission Already Done
                result = true;
            }
        }
        
        return result;
    }
}
