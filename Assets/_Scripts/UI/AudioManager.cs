using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider VolumeSliderMaster;
    [SerializeField] Slider VolumeSliderMusic;
    [SerializeField] Slider VolumeSliderSFX;

    const string MASTER_VOLUME = "Master";
    const string MUSIC_VOLUME = "MusicVolume";
    const string SFX_VOLUME = "SFXVolume";
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        VolumeSliderMaster.onValueChanged.AddListener(VolumeChangeMaster);
        VolumeSliderMusic.onValueChanged.AddListener(VolumeChangeMusic);
        VolumeSliderSFX.onValueChanged.AddListener(VolumeChangeSFX);
    }
    private void Start()
    {
        LoadAudio();
        VolumeChangeMaster(VolumeSliderMaster.value);
        VolumeChangeMusic(VolumeSliderMusic.value);
        VolumeChangeSFX(VolumeSliderSFX.value);
    }
    public void VolumeChangeMaster(float value)
    {
        mixer.SetFloat(MASTER_VOLUME, Mathf.Log10(value)*20);
    }
    public void VolumeChangeMusic(float value)
    {
        mixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(value) * 20);
    }
    public void VolumeChangeSFX(float value)
    {
        mixer.SetFloat(SFX_VOLUME, Mathf.Log10(value) * 20);
    }
    public void SaveAudio()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME, VolumeSliderMaster.value);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, VolumeSliderMusic.value);
        PlayerPrefs.SetFloat(SFX_VOLUME, VolumeSliderSFX.value);
    }
    public void LoadAudio()
    {
        VolumeSliderMaster.value = PlayerPrefs.GetFloat(MASTER_VOLUME, VolumeSliderMaster.value);
        VolumeSliderMusic.value = PlayerPrefs.GetFloat(MUSIC_VOLUME, VolumeSliderMusic.value);
        VolumeSliderSFX.value = PlayerPrefs.GetFloat(SFX_VOLUME, VolumeSliderSFX.value);
    }
    private void OnDisable()
    {
        VolumeSliderMaster.onValueChanged.RemoveListener(VolumeChangeMaster);
        VolumeSliderMusic.onValueChanged.RemoveListener(VolumeChangeMusic);
        VolumeSliderSFX.onValueChanged.RemoveListener(VolumeChangeSFX);
        SaveAudio();
    }
}
