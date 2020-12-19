﻿using System.Collections;
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
            }
    }
}