using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionController : MonoBehaviour
{
    public GameObject startTrigger;
    public GameObject endTrigger;

    public GameObject endScreen;

    private StartMissionTrigger startMissionTrigger;
    private EndMissionTrigger endMissionTrigger;

    // Start is called before the first frame update
    void Start()
    {
        this.startMissionTrigger = startTrigger.GetComponent<StartMissionTrigger>();
        this.endMissionTrigger = endTrigger.GetComponent<EndMissionTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if(startMissionTrigger.hasMissionStarted == true)
        {
            Debug.Log("Mission Started!");
            startTrigger.gameObject.SetActive(false);

            endTrigger.gameObject.SetActive(true);
        }

        if(endMissionTrigger.hasMissionEnded == true)
        {
            endScreen.gameObject.SetActive(true);

            Debug.Log("Mission Ended!");

            endTrigger.gameObject.SetActive(false);
        }
    }
}
