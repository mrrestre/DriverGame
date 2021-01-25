using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        // for the Health bar
        healthBar.findImageBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player_Car")
        {
            healthBar.setHealthBarFull();
        }
    }
}
