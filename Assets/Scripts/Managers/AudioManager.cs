using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] AudioSource audioSourceSFX;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] float sfxVolume;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
        sfxVolumeSlider.value = sfxVolume;
    }

    private void Start()
    {
        SaveManager.instance.saveEvent.AddListener(SaveAM);
    }

    public void PlayButtonSFXByIndex(int indexAC)
    {
        if (audioSourceSFX.clip != audioClips[indexAC])
        {
            audioSourceSFX.clip = audioClips[indexAC];
        }
        audioSourceSFX.Play();
    }

    public void SetValueSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;
    }

    private void OnApplicationPause(bool pause)
    {
        SaveAM();        
    }

    private void OnApplicationQuit()
    {
        SaveAM();
    }

    private void SaveAM()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }
}
