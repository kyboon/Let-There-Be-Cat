
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public float volume = 1f;

    [HideInInspector]
    public AudioSource source;
}
