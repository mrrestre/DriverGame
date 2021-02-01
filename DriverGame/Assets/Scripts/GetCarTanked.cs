using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCarTanked : MonoBehaviour
{
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.findImageBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player_Car")
        {
            healthBar.setHealthBarFull();
        }
    }
}
