using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float accelerationRate = 16f;
    public float finalVelocity = 70f;
    public float howLong = 4f;

    public float rotateY = 10.0f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotateY, 0f, Space.Self);
    }
}
