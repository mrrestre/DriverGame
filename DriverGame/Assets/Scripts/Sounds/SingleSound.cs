using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SingleSound
{
    public string soundName;

    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    public bool loop;

    public AudioMixer output;
    public AudioMixerGroup output1;
}
