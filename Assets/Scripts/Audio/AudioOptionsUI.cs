using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;

    void Start()
    {
        masterSlider.onValueChanged.AddListener(AudioSettingsManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioSettingsManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioSettingsManager.Instance.SetSFXVolume);
        muteToggle.onValueChanged.AddListener(AudioSettingsManager.Instance.SetMute);
    }
} 

