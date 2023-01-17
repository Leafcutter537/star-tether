using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        musicSource = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        sfxSource = GameObject.Find("SoundPlayer").GetComponent<AudioSource>();
        sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume", 1);
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume", 1);
    }

    public void OnChangeSFXSlider()
    {
        float volume = sfxSlider.value;
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFX Volume", volume);
    }
    public void OnChangeMusicSlider()
    {
        float volume = musicSlider.value;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("Music Volume", volume);
    }
}
