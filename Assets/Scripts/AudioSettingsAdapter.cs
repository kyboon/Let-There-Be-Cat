using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsAdapter : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider musicSlider;
    public Slider sfxSlider;
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicMultiplier", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXMultiplier", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMusicMultiplier(float value)
    {
        AudioManager.instance.setMusicMultiplier(value);
    }

    public void setSFXMultiplier(float value)
    {
        AudioManager.instance.setSFXMultiplier(value);
    }

    private void OnDestroy()
    {
        AudioManager.instance.commitMultipliers();
    }
}
