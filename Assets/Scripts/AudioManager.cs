using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds = new List<Sound>();
    // Start is called before the first frame update
    private AudioSource _audioSource;
    public static AudioManager instance = null;

    public float musicMultiplier = 1f;
    public float sfxMultiplier = 1f;

    private float initialMusicVolume = 1f;
    public static AudioManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        musicMultiplier = PlayerPrefs.GetFloat("MusicMultiplier", 1);
        sfxMultiplier = PlayerPrefs.GetFloat("SFXMultiplier", 1);

        _audioSource = GetComponent<AudioSource>();

        initialMusicVolume = _audioSource.volume;
        _audioSource.volume = initialMusicVolume * musicMultiplier;
        PlayMusic();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume * sfxMultiplier;
        }
    }

    private void Start()
    {
        PlayMusic();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

    public void PlaySound(int index)
    {
        sounds[index].source.Play();
    }
    public void setMusicMultiplier(float newMultiplier)
    {
        musicMultiplier = newMultiplier;
        _audioSource.volume = initialMusicVolume * newMultiplier;
        Debug.Log(newMultiplier + " " + _audioSource.volume);
    }

    public void commitMultipliers()
    {
        Debug.Log("Commit volume multipliers");
        PlayerPrefs.SetFloat("MusicMultiplier", musicMultiplier);
        PlayerPrefs.SetFloat("SFXMultiplier", sfxMultiplier);

        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * sfxMultiplier;
        }
    }

    public void setSFXMultiplier(float newMultiplier)
    {
        sfxMultiplier = newMultiplier;
    }
}
