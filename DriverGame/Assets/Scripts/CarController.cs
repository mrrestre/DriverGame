using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Sphere in the Middle of the object
    public Rigidbody sphereRigidBody;

    // to Controll the Health
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    //Wheels
    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25f;

    //To simplify the meassurments in Unity
    private const float MULTIPLY_FACTOR = 1000f;

    //Definition of the different forces
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnsStrength = 180f, gravityModifier = 10f;

    private float speedInput, turnInput;

    public float acceleration = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        //Remove the parent node from the spheres on the wheels
        sphereRigidBody.transform.parent = null;

        //Increment the Gravity for this object
        Physics.gravity *= gravityModifier;

        // set the Health to 100 when the game starts
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
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

        //Rotate the left wheel with the input
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);

        //Rotate the right wheel with the input
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);

        //Move our object to the position of the spheres 
        transform.position = sphereRigidBody.transform.position;

    }

    private void FixedUpdate()
    {
        //Move the sphere forward and backward
        if (Mathf.Abs(speedInput) > 0)
        {
            sphereRigidBody.AddForce(transform.forward * speedInput);
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        sphereRigidBody.velocity.Scale(Vector3.zero);
    }

    private void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
