using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    //References
    private AudioSource engineNoice;
    private CarController carController;
    private AudioSource reverseClip;

    //Configurations
    public float audioPitch = 1f;
    public float changeOfGear = 0.2f;

    //Boundaries
    public float minPitch = 0.2f;
    public float maxPitch = 1.8f;

    //Help Values
    private float speedToPitchConvertionRate;
    private float firstGear;
    private float secondGear;
    private float thirdGear;

    // Start is called before the first frame update
    void Start()
    {
        engineNoice = GetComponents<AudioSource>()[0];
        reverseClip = GetComponents<AudioSource>()[1];
        carController = GetComponent<CarController>();

        float carMaxSpeed = carController.GetCarFinalSpeed();

        speedToPitchConvertionRate = (maxPitch - minPitch) / carMaxSpeed;

        firstGear = ((maxPitch - minPitch) / 2);
        secondGear = ((maxPitch - minPitch) / 3) * 2;
        thirdGear = (maxPitch - minPitch);

        engineNoice.pitch = minPitch;
    }

    // Update is called once per frame
    void Update()
    {
        float carSpeed = carController.GetCarSpeed();
        
        if(carSpeed >= 0)
        {
            if(reverseClip.isPlaying)
            {
                reverseClip.Stop();
            }

            audioPitch = (speedToPitchConvertionRate * carSpeed) + minPitch;

            if (audioPitch <= firstGear)
            {
                engineNoice.pitch = audioPitch;
            }
            else if (firstGear < audioPitch && audioPitch <= secondGear)
            {
                engineNoice.pitch = audioPitch - changeOfGear;
            }
            else if (secondGear < audioPitch && audioPitch <= thirdGear)
            {
                engineNoice.pitch = audioPitch - (changeOfGear * 2);
            }

            if(Input.GetAxis("Vertical") <= 0)
            {
                engineNoice.pitch = audioPitch - (changeOfGear * 2);
            }
        }
        else
        {
            engineNoice.pitch = minPitch;
            if(!reverseClip.isPlaying)
            {
                reverseClip.Play();
            }
        }

    }

}
