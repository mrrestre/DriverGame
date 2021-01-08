using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


public class CountdownTimer : MonoBehaviour
{
    //The Text which is shown on the top part of the screen
    public GameObject textDisplay;
    
    //Depends on the current Mission
    private int remainingTime;

    //Screen when the timer reaches zero
    public GameObject countdownFailedScreen;

    //StopWatch to keep track of the time elapsed
    public Stopwatch timer;


    // Start is called before the first frame update
    void Start()
    {
        textDisplay.GetComponent<Text>().text = "00:00";
        timer = new Stopwatch();
    }

    public void startTimer(int howLongTimer)
    {
        this.remainingTime = howLongTimer;

        if(!timer.IsRunning)
        {
            timer.Start();
            InvokeRepeating("controlTimerOnScreen", 0f, 1.0f);
        }
    }

    public void stopTimer()
    {
        timer.Stop();
        CancelInvoke("controlTimerOnScreen");
        textDisplay.GetComponent<Text>().text = "00:00";
    }


    void controlTimerOnScreen()
    {
        remainingTime = remainingTime - (int)timer.Elapsed.TotalSeconds;

        if (remainingTime >= 60)
        {
            string remaingSecondsString;

            if (remainingTime % 60 > 9)
            {
                remaingSecondsString = ":" + remainingTime % 60;
            }
            else
            {
                remaingSecondsString = ":0" + remainingTime % 60;
            }

            textDisplay.GetComponent<Text>().text = "0" + Mathf.Floor(remainingTime / 60) + remaingSecondsString;
        }
        else if (remainingTime < 59)
        {
            textDisplay.GetComponent<Text>().text = "00:" + remainingTime;
        }
        else if (remainingTime < 10)
        {
            textDisplay.GetComponent<Text>().text = "00:0" + remainingTime;
        }

        if (remainingTime == 0)
        {
            countdownFailedScreen.gameObject.SetActive(true);
        }
    }
}

/*
 * public class CountdownTimer : MonoBehaviour
{
    //The Text which is shown on the top part of the screen
    public GameObject textDisplay;
    
    //Depends on the current Mission
    private int remainingTime;

    //Screen when the timer reaches zero
    public GameObject countdownFailedScreen;

    Coroutine lastCoroutine = null;

    //Trigger to end the screen
    private bool endElementReached = false;


    // Start is called before the first frame update
    void Start()
    {
        textDisplay.GetComponent<Text>().text = "0:00";
    }

    public void startTimer(int howLongTimer)
    {
        this.remainingTime = howLongTimer;

        lastCoroutine = StartCoroutine(TimerTake());
    }

    public void stopTimer()
    {
        endElementReached = true;
        lastCoroutine = null;
    }


    IEnumerator TimerTake()
    {       
        if(!endElementReached)
        {
            yield return new WaitForSeconds(1);
            remainingTime -= 1;

            if (remainingTime >= 60)
            {
                string remaingSeconds;

                if (remainingTime % 60 > 9)
                {
                    remaingSeconds = ":" + remainingTime % 60;
                }
                else
                {
                    remaingSeconds = ":0" + remainingTime % 60;
                }

                textDisplay.GetComponent<Text>().text = Mathf.Floor(remainingTime / 60) + remaingSeconds;
            }
            else if (remainingTime < 59)
            {
                textDisplay.GetComponent<Text>().text = "00:" + remainingTime;
            }
            else if (remainingTime < 10)
            {
                textDisplay.GetComponent<Text>().text = "00:0" + remainingTime;
            }

            if (remainingTime == 0)
            {
                countdownFailedScreen.gameObject.SetActive(true);
            }
        }
        else
        {
            textDisplay.GetComponent<Text>().text = "0:00";
        }
    }
}
*/
