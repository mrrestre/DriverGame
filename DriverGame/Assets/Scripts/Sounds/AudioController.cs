using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public string standardBackgroundMusic = "Background1";

    public SingleSound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        //Create a new Audio Source for each Audio clip needed
        foreach (SingleSound sound in sounds)
        {
            //Create a new component for the needed sound. It wont show on the inspector
            sound.source = gameObject.AddComponent<AudioSource>();
            
            //Set the right variables to the sound given into the inspector
            sound.source.clip   = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch  = sound.pitch;
            sound.source.loop   = sound.loop;
        }
    }

    void Start()
    {
        PlaySoundByName(standardBackgroundMusic);
    }

    public void PlaySoundByName(string name)
    {
        SingleSound sound = SearchForSoundInList(name);

        if(sound != null)
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning("The sound with the name: " + name + " was not found");
        }
    }

    public void PauseSoundWithName(string name)
    {
        SingleSound sound = SearchForSoundInList(name);

        if (sound != null && sound.source.isPlaying)
        {
            sound.source.Stop();
        }
        else
        {
            Debug.LogWarning("The sound with the name: " + name + " was not found");
        }
    }

    public void StopAllSound()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].source.isPlaying)
            {
                sounds[i].source.Stop();
            }
        }
    }

    private SingleSound SearchForSoundInList(string name)
    {
        SingleSound searchedSound = null;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].soundName == name)
            {
                searchedSound = sounds[i];
            }
        }

        return searchedSound;
    }

    public AudioSource ReturnAudioSourceWithName(string name)
    {
        AudioSource searchedAudioSource = null;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].soundName == name)
            {
                searchedAudioSource = sounds[i].source;
            }
        }

        return searchedAudioSource;
    }
}
