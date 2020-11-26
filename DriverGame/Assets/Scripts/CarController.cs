using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Sphere in the Middel of the object
    public Rigidbody sphere;

    //To simplify the meassurments in Unity
    private const float MULTIPLY_FACTOR = 1000f;

    //Definition of the different forces
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnsStrength = 180f;

    private float speedInput, turnInput;


    // Start is called before the first frame update
    void Start()
    {
        //Remove the parent node from the sphere
        sphere.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;

        //Forward and reverse speed with input
        if(Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * MULTIPLY_FACTOR;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel * MULTIPLY_FACTOR;
        }

        //Lateral movement with input
        turnInput = Input.GetAxis("Horizontal");
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnsStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));

        //Move our object to the sphere location
        transform.position = sphere.transform.position;
    }

    private void FixedUpdate()
    {
        //Move the sphere forward and backward
        if(Mathf.Abs(speedInput) > 0)
        {
            sphere.AddForce(transform.forward * speedInput);
        }
        
    }
}
