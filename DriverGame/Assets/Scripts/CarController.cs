using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Sphere in the Middle of the object
    public Rigidbody sphereRigidBody;

    // Health Bar reference
    [SerializeField] private HealthBar healthBar;

    //Wheels
    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25f;

    //Definition of the different forces
    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnsStrength = 180f, gravityModifier = 10f;

    private float speedInput, turnInput;


    // Start is called before the first frame update
    void Start()
    {
        //Remove the parent node from the spheres on the wheels
        sphereRigidBody.transform.parent = null;

        //Increment the Gravity for this object
        Physics.gravity *= gravityModifier;

    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;

        //Forward and reverse speed with input
        if(Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel;
        }


        //Lateral movement with input
        turnInput = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Vertical") > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnsStrength * Time.deltaTime * sphereRigidBody.velocity.magnitude, 0f));
        }

        else if (Input.GetAxis("Vertical") < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnsStrength * Time.deltaTime * Input.GetAxis("Vertical") * 10f, 0f));
        }


        //Rotate the left wheel with the input
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);

        //Rotate the right wheel with the input
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);

        //Move our object to the position of the spheres 
        transform.position = sphereRigidBody.transform.position;

    }

    private void FixedUpdate()
    {
        //Only apply force if the max speed is not reached
        if(sphereRigidBody.velocity.magnitude < maxSpeed)
        {
            //Move the sphere forward and backward
            if (Mathf.Abs(speedInput) > 0)
            {
                sphereRigidBody.AddForce(transform.forward * speedInput, ForceMode.Acceleration);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the other object should dissapear on contact
        if(other.gameObject.tag == "Obstacle_Dissapears")
        {
            other.gameObject.SetActive(false);
        }
        else
        {
            sphereRigidBody.velocity.Scale(Vector3.zero);
        }
    }

    private void CarGetDamaged(Collider other)
    {
        if(other.gameObject.tag == "Palyer_Damage")
        {
            healthBar.setHealthBar(.2f);
        }
    }

}
