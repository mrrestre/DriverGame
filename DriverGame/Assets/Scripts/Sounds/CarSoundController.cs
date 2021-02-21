using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    //References
    private CarController carController;
    private AudioController audioController;
    private AudioSource engineNoice;
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

    //Is the car on
    public bool isTheCarOn = false;

    // Start is called before the first frame update
    void Start()
    {
        //Audio Controller
        audioController = FindObjectOfType<AudioController>();

        engineNoice = audioController.ReturnAudioSourceWithName("EngineSound");
        reverseClip = audioController.ReturnAudioSourceWithName("ReverseSound");

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
        if(isTheCarOn)
        {
            float carSpeed = carController.GetCarSpeed();

            if (carSpeed >= 0)
            {
                if (reverseClip.isPlaying)
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

                if (Input.GetAxis("Vertical") <= 0)
                {
                    engineNoice.pitch = audioPitch - (changeOfGear * 2);
                }
            }
            else
            {
                engineNoice.pitch = minPitch;
                if (!reverseClip.isPlaying)
                {
                    reverseClip.Play();
                }
            }
        }
    }
        
    public void TurnOnTheCar()
    {
        isTheCarOn = true;
        audioController.PlaySoundByName("EngineStart");

        audioController.PlaySoundByName("EngineSound");
    }
}
