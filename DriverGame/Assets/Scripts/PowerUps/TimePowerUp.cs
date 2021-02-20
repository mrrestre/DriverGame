using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePowerUp : MonoBehaviour
{
    public float timeToAddToCountdown = 5f;

    public float rotateY = 10.0f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotateY, 0f, Space.Self);
    }
}
