﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    [Header("External Components")]
    
    //Sphere in the Middle of the object
    [SerializeField] private Rigidbody sphereRigidBody;

    //Wheels
    [SerializeField] private Transform leftFrontWheel, rightFrontWheel;

    //For the Cornering and acceleration
    [SerializeField] private GameObject tiltingPart;

    // Health Bar reference
    public HealthBar healthBar;
    [SerializeField] private GameObject noLivesScreen;
    //How much Heatlth has the vehicle remaining
    public float currentHealth;
    ////////////////////////////////////

    [Header("Speed Managment")]

    //Definition of the different forces when moving Forward
    //Current Speed
    [SerializeField] private float currentVelocity = 0.0f;

    //Max Speed 
    [SerializeField] private float finalVelocity = 30f;

    //How quickly accelerates per second
    [SerializeField] private float accelerationRate = 8f;

    //How quickly deaccelerates per second
    [SerializeField] private float decelerationRate = 4f;

    //How hard does it turns
    [SerializeField] private float turnStrength = 180f;

    [Space]

    //Definition of the different forces when moving backwards
    //Max reverse Speed
    [SerializeField] private float backFinalVelocity = -20f;

    //How quickly accelerates per second going backwards
    [SerializeField] private float backAccelerationRate = 4f;

    //How hard does it turns
    [SerializeField] private float backTurnStrength = 180f;    

    //Definition of the different forces when moving forward
    private float initialVelocity = 0.0f;

    ////////////////////////////////////

    [Header("Corner Tilting")]

    //Tilting by cornering
    //How much the car tilts each second
    [SerializeField] private float lateralTiltingFactor = 0.5f;

    //Maximun lateral tilt of the car
    [SerializeField] private float lateralMaxTilt = 10f;

    //At which speed the car starts tilting
    [SerializeField] private float tiltStartAtSpeed = 10f;

    private float currentLateralTilt = 0.0f;
    private float zeroTilt = 0.0f;

    ////////////////////////////////////

    [Header("Acceleration Tilting")]

    //Tilting by cornering
    //How much the car tilts each second
    [SerializeField] private float accelerationTiltingFactor = 0.5f;

    //Maximun acceleration tilt of the car
    [SerializeField] private float accelerationMaxTilt = 8f;

    public float currentAccelerationTilt = 0.0f;

    ////////////////////////////////////

    [Header("Tire Trails")]

    [SerializeField] private List<GameObject> tireTrails;

    ////////////////////////////////////

    [Header("Crashing")]

    [SerializeField] private GameObject obstacleDissapears;
    [SerializeField] private GameObject obstacleDamages;

    ////////////////////////////////////

    [Header("Coins")]

    private float collectedCoins = 0;
    [SerializeField] private TextMeshProUGUI textCoins;
    ////////////////////////////////////

    [Header("Others")]

    //Gravity Modifier
    [SerializeField] private float gravityModifier = 10f;

    //To define how far the wheels may turn
    [SerializeField] private float maxWheelTurn = 25f;

    

    //Input for both axis
    private float turnInput;

    ////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        //Remove the parent node from the spheres on the wheels
        sphereRigidBody.transform.parent = null;

        //Increment the Gravity for this object
        Physics.gravity *= gravityModifier;

        //For the acceleration tilt
        currentAccelerationTilt = tiltingPart.transform.rotation.x;

        //For the tilting of the car
        currentLateralTilt = tiltingPart.transform.rotation.z;

        // for the Health bar
        healthBar.findImageBar();
        healthBar.setHealthBarFull();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.changeHelathBarColor();

        if (healthBar.getCurrentHealth() <= 0)
        {
            noLivesScreen.SetActive(true);
        }
        ////////////////////////Speed Control////////////////////////

        //Set the right current Velocity each frame depending on the user input
        if (Input.GetAxis("Vertical") > 0)
        {
            currentVelocity = currentVelocity + (accelerationRate * Time.deltaTime);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            //Works as an extra brake
            if(currentVelocity > 0)
            {
                currentVelocity = currentVelocity - ((backAccelerationRate + decelerationRate) * Time.deltaTime);
            }
            //Start reversing
            else
            {
                currentVelocity = currentVelocity - (backAccelerationRate * Time.deltaTime);
            }
        }
        //Return to zero speed if not a vertical input found
        else
        {
            currentVelocity = currentVelocity - (decelerationRate * Time.deltaTime);
            currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, finalVelocity);
        }

        //ensure the velocity never goes out of the backFinal/final boundaries
        currentVelocity = Mathf.Clamp(currentVelocity, backFinalVelocity, finalVelocity);

        ////////////////////////Trails Manager////////////////////////
        
        //where should be the trails generated
        if((currentVelocity <= finalVelocity * 0.5f && Input.GetAxis("Vertical") > 0) 
            || (Mathf.Abs(currentLateralTilt) > lateralMaxTilt * 0.8f)
            || (Input.GetAxis("Vertical") < 0))
        {
            SetEmitionOnAllWheels(true);
        }
        else if(currentVelocity > finalVelocity * 0.5f || Input.GetAxis("Vertical") <= 0)
        {
            SetEmitionOnAllWheels(false);
        }

        ////////////////////////Acceleration Tilt////////////////////////

        //If the car has not reached the maximun tilting possible (Eather way)
        if (Mathf.Abs(currentAccelerationTilt) <= accelerationMaxTilt)
        {
            if(Input.GetAxis("Vertical") != 0)
            {
                //is the car Accelerating
                if (Input.GetAxis("Vertical") > 0)
                {
                    currentAccelerationTilt = currentAccelerationTilt - accelerationTiltingFactor;

                    currentAccelerationTilt = Mathf.Clamp(currentAccelerationTilt, -accelerationMaxTilt, zeroTilt);
                }

                //is the car braking (Not going backwards)
                else if (Input.GetAxis("Vertical") < 0)
                {
                    currentAccelerationTilt = currentAccelerationTilt + accelerationTiltingFactor;

                    currentAccelerationTilt = Mathf.Clamp(currentAccelerationTilt, zeroTilt, accelerationMaxTilt);
                }
            }   
        }
        

        //Return the tilt to zero if no input found and the car is tilted
        if (Input.GetAxis("Vertical") == 0)
        {
            if (currentAccelerationTilt > zeroTilt)
            {
                currentAccelerationTilt = currentAccelerationTilt - accelerationTiltingFactor;
            }
            else if (currentAccelerationTilt < zeroTilt)
            {
                currentAccelerationTilt = currentAccelerationTilt + accelerationTiltingFactor;
            }

            currentAccelerationTilt = Mathf.Clamp(currentAccelerationTilt, -accelerationMaxTilt, accelerationMaxTilt);
        }


        ////////////////////////Turning Control////////////////////////

        //Lateral movement with input
        turnInput = Input.GetAxis("Horizontal");

        //Defines how the turning works when going forwards
        if (currentVelocity > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * currentVelocity, 0f));
        }

        //Defines how the turning works when going backwards
        if (currentVelocity < 0)
        {
           transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * backTurnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }

        ////////////////////////Turning Tilt////////////////////////

        //Only Applies if the car is going forward
        if (currentVelocity > tiltStartAtSpeed)
        {
            //If the car has not reached the maximun tilting possible (Eather way)
            if (Mathf.Abs(currentLateralTilt) <= lateralMaxTilt)
            {
                //The user is making an input
                if (turnInput != 0)
                {
                    //User is pushing the left arrow key therefore rotates to the right
                    if (turnInput > 0)
                    {
                        currentLateralTilt = currentLateralTilt + lateralTiltingFactor * Mathf.Sqrt(currentVelocity);

                        //Does not allow the tilt to go outward of the possible range
                        currentLateralTilt = Mathf.Clamp(currentLateralTilt, zeroTilt, lateralMaxTilt);
                    }
                    //User is pushing the right arrow key therefore rotates to the left
                    else
                    {
                        currentLateralTilt = currentLateralTilt - lateralTiltingFactor * Mathf.Sqrt(currentVelocity);

                        //Does not allow the tilt to go outward of the possible range
                        currentLateralTilt = Mathf.Clamp(currentLateralTilt, -lateralMaxTilt, zeroTilt);
                    }
                }
            }
        }

        //Return the tilt to zero if no input found and the car is tilted
        if ((Mathf.Abs(currentLateralTilt) > 0))
        {
            if (currentLateralTilt > zeroTilt)
            {
                currentLateralTilt = currentLateralTilt - lateralTiltingFactor;
            }
            if (currentLateralTilt < zeroTilt)
            {
                currentLateralTilt = currentLateralTilt + lateralTiltingFactor;
            }
        }


        ////////////////////////Wheel Turning////////////////////////

        //Rotate the left wheel with the input
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);

        //Rotate the right wheel with the input
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);


        ////////////////////////Car Movement////////////////////////
        
        //Move our object to the position of the sphere 
        transform.position = sphereRigidBody.transform.position;


        //Tilt the car when turning and accelerating / breaking
        tiltingPart.transform.localRotation = Quaternion.Euler(currentAccelerationTilt, 0, currentLateralTilt);
    }

    private void FixedUpdate()
    {
        //Move the sphere forward and backward
        if (Mathf.Abs(currentVelocity) > 0)
        {
            //Apply force to the Sphere
            sphereRigidBody.AddForce(transform.forward * currentVelocity, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the other object should dissapear on contact
        if(other.gameObject.tag == "Obstacle_Dissapears")
        {
            this.obstacleDissapears.GetComponent<AudioSource>().Play();
            this.obstacleDissapears.GetComponent<ParticleSystem>().Play();
            other.gameObject.SetActive(false);
        }
        
        else if(other.gameObject.tag == "Player_Damage")
        {
            this.obstacleDamages.GetComponent<AudioSource>().Play();
            this.obstacleDamages.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            this.obstacleDamages.transform.GetChild(1).GetComponent<ParticleSystem>().Play();

            /*
            sphereRigidBody.velocity.Scale(new Vector3(0, 0, 0));
            healthBar.setHealthBarValue(.2f);
            Debug.Log("Damage collider");
            */
            sphereRigidBody.velocity.Scale(new Vector3(0, 0, 0));

            // hier kommt den Logik von Damage
            // MAX SPEED 50
            if (currentVelocity > 0 && currentVelocity <= 5)
            { healthBar.setHealthBarDamage(.02f); }
            else if (currentVelocity > 5 && currentVelocity <= 10)
            { healthBar.setHealthBarDamage(.04f); }
            else if (currentVelocity > 10 && currentVelocity <= 15)
            { healthBar.setHealthBarDamage(.06f); }
            else if (currentVelocity > 15 && currentVelocity <= 20)
            { healthBar.setHealthBarDamage(.08f); }
            else if (currentVelocity > 20 && currentVelocity <= 25)
            { healthBar.setHealthBarDamage(.10f); }
            else if (currentVelocity > 25)
            { healthBar.setHealthBarDamage(.12f); }

            Debug.Log("2 " + currentHealth);

        }
        else if(other.gameObject.tag == "Bullet")
        {
            healthBar.setHealthBarDamage(.03f);
        }
        else if (other.gameObject.tag == "Coins")
        {
            collectedCoins++;
            textCoins.text = collectedCoins.ToString();
            other.gameObject.SetActive(false);
            Debug.Log("Coins" + textCoins.text);
        }


    }
    

    public float GetCarSpeed()
    {
        return this.currentVelocity;
    }

    public float GetCarFinalSpeed()
    {
        return this.finalVelocity;
    }

    public void SetEmitionOnAllWheels(bool value)
    {
        for (int i = 0; i < tireTrails.Count; i++)
        {
            tireTrails[i].GetComponent<TrailRenderer>().emitting = value;
        }
    }

    /*    private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Player_Damage")
            {



            }
        }
    */
}
