﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPoint : MonoBehaviour
{
    private Image iconImg;
    private Text distanceText;

    public Transform player;
    public Transform target;
    public Camera cam;

    public float closeEnoughDist;

    public MissionController missionController;

    private StartMissionTrigger startMissionTrigger;
    private EndMissionTrigger endMissionTrigger;

    
    // Start is called before the first frame update
    void Start()
    {
        this.startMissionTrigger = missionController.startObject.GetComponent<StartMissionTrigger>();
        this.endMissionTrigger = missionController.endObject.GetComponent<EndMissionTrigger>();
        iconImg = GetComponent<Image>();
        distanceText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            GetDistance();
            CheckOnScreen();
        }

    }

    private void GetDistance()
    {
        float dist = Vector3.Distance(player.position, target.position);
        distanceText.text = dist.ToString("f1") + "m";

        if (startMissionTrigger.hasMissionStarted == true)
        {
            Destroy(gameObject);
        }
    }

    private void CheckOnScreen()
    {
        float thing = Vector3.Dot((target.position - cam.transform.position).normalized, cam.transform.forward);

        if(thing <= 0)
        {
            ToggleUI(false);
        }
        else
        {
            ToggleUI(true);
            transform.position = cam.WorldToScreenPoint(target.position);
        }
    }

    private void ToggleUI(bool _value)
    {
        iconImg.enabled = _value;
        distanceText.enabled = _value;
    }
}
