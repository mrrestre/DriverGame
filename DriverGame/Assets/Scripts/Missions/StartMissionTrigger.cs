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

                Debug.Log("Mission Started!");

                uiElement.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player_Car")
        {
            uiElement.SetActive(false);
        }
    }
}
