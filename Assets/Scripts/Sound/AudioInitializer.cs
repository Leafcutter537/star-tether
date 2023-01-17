using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInitializer : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        musicSource.volume = PlayerPrefs.GetFloat("Music Volume", 1);
        sfxSource.volume = PlayerPrefs.GetFloat("SFX Volume", 1);
    }
}
