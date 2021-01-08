using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMissionTrigger : MonoBehaviour
{
    public bool hasMissionStarted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Car")
        {
            hasMissionStarted = true;

            Time.timeScale = 0.5f;
            InvokeRepeating("returnToNormalSpeed", 1f, 0.5f);
        }
    }
    void returnToNormalSpeed()
    {
        Time.timeScale = 1f;
        CancelInvoke();
    }
}
