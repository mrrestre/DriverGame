using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMissionTrigger : MonoBehaviour
{
    public bool hasMissionStarted = false;

    [SerializeField] public GameObject uiElement;




    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Player_Car")
        {
            uiElement.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                hasMissionStarted = true;
                uiElement.SetActive(false);
                 /*
                Time.timeScale = 0.5f;
                InvokeRepeating("returnToNormalSpeed", 1f, 0.5f);
                 */
            }
        }
    }

    /*
    void returnToNormalSpeed()
    {
        Time.timeScale = 1f;
        CancelInvoke();
    }
    */

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player_Car")
        {
            uiElement.SetActive(false);
        }
    }
}
