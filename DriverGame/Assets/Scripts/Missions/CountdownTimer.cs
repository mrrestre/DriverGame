using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CountdownTimer : MonoBehaviour
{
    //The Text which is shown on the top part of the screen
    public GameObject textGameObject;
    private TextMeshProUGUI countdownText;

    //Depends on the current Mission
    public float timeForMission;
    public float remainingTime;
    public float elapsedTime = 0f;

    //Determines if the mission has started
    public bool hasMissionStarted;

    //Determines if the countdown is running
    public bool isCountdownRunning;

    //Screen when the timer reaches zero
    public GameObject countdownFailedScreen;

    //To activate the right screens
    private MissionFailedMenu missionFailedMenu;

    //PowerUp Timer
    private float boostTimer;
    private bool timerBoosting;

    // Start is called before the first frame update
    void Start()
    {
        textGameObject = this.gameObject;
        missionFailedMenu = countdownFailedScreen.GetComponent<MissionFailedMenu>();
        countdownText = textGameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hasMissionStarted)
        {
            if(isCountdownRunning)
            {
                elapsedTime += Time.deltaTime;
                remainingTime = Mathf.Round(timeForMission - elapsedTime);
                UpdateCountdownOnScreen();
                
                if (remainingTime == 0)
                {
                    missionFailedMenu.ActivateScreen();
                    StopCountdown();
                }
            }
            else
            {
                UpdateCountdownOnScreen();
            }
        }
        else
        {
            countdownText.text = "00:00";
        }
    }

    public void StartCountdown(int howLongTimer)
    {
        Debug.Log("Countdown Started");

        this.timeForMission = howLongTimer;

        this.hasMissionStarted = true;
        this.isCountdownRunning = true;
    }

    public void PauseCountdown()
    {
        Debug.Log("Countdown Paused");

        this.isCountdownRunning = false;
    }

    public void ResumeCountdown()
    {
        Debug.Log("Countdown Resumed");

        this.isCountdownRunning = true;
    }

    public void StopCountdown()
    {
        Debug.Log("Countdown Stopped");

        this.hasMissionStarted = false;
        this.isCountdownRunning = false;
        this.elapsedTime = 0f;
        this.remainingTime = 0f;
        this.timeForMission = 0f;
    }


    void UpdateCountdownOnScreen()
    {
        if (this.remainingTime >= 60)
        {
            string remaingSecondsString;
        
            if (this.remainingTime % 60 > 9)
            {
                remaingSecondsString = ":" + remainingTime % 60;
            }
            else
            {
                remaingSecondsString = ":0" + remainingTime % 60;
            }

            countdownText.text = "0" + Mathf.Floor(remainingTime / 60) + remaingSecondsString;
        }
        else if (this.remainingTime < 60 && this.remainingTime > 9)
        {
            countdownText.text = "00:" + remainingTime;
        }
        else if (this.remainingTime < 10)
        {
            countdownText.text = "00:0" + remainingTime;
        }
    }
}

/*  private void Update()
  {
      ///////////////////////Timer-Boost/////////////////////////////
      if (timerboosting)
      {
          boosttimer += Time.deltaTime;
          if (boosttimer >= 10)
          {
              boosttimer = 0;
              timerboosting = false;
          }
      }
  }

/*  private void OnTriggerEnter(Collider other)
  {
      // TimerBoost
      if (other.tag == "TimerBoost")
      {
          timerboosting = true;
          Destroy(other.gameObject);
      }
  }*/
