using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CountdownTimer : MonoBehaviour
{
    public GameObject textDisplay;
    public int secondsLeft = 30;
    public bool takingAway = false;

    private StartMissionTrigger startMissionTrigger;
    public EndMissionTrigger endMissionTrigger;

    public GameObject startTrigger;

    public GameObject countdownFailedScreen;

    Coroutine lastCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        textDisplay.GetComponent<Text>().text = "00:00";
        this.startMissionTrigger = startTrigger.GetComponent<StartMissionTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startMissionTrigger.hasMissionStarted == true && takingAway == false && secondsLeft > 0)
        {
            lastCoroutine = StartCoroutine(TimerTake());
        }
        
        

       
    }


    IEnumerator TimerTake()
    {
        takingAway = true;
        //(startMissionTrigger.hasMissionStarted == true && endMissionTrigger.hasMissionEnded == false)
        
            yield return new WaitForSeconds(1);
            secondsLeft -= 1;
        

        if (secondsLeft < 10)
        {
            textDisplay.GetComponent<Text>().text = "00:0" + secondsLeft;
        }
        else
        {
            textDisplay.GetComponent<Text>().text = "00:" + secondsLeft;
        }
        takingAway = false;

        if (secondsLeft == 0)
        {
            countdownFailedScreen.gameObject.SetActive(true);
        }

    }
}
