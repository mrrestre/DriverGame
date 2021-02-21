using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMissionTrigger : MonoBehaviour
{
    public bool hasMissionEnded = false;

    public SingleMission currentMission;

    //public ParticleSystem ringEnd;

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Car")
        {
            hasMissionEnded = true;
            
        }
    }
}
