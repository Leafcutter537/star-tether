using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip energyPickup;
    [SerializeField] private AudioClip asteroidFired;
    [SerializeField] private AudioClip wormholeReached;
    [SerializeField] private AudioClip asteroidDestroyed;
    [SerializeField] private AudioClip toggle;
    [SerializeField] private AudioClip tether;
    [SerializeField] private AudioClip barrierDestroyed;
    [Header("Audio Events")]
    [SerializeField] private EnergyPickupEvent energyPickupEvent;
    [SerializeField] private AsteroidFiredEvent asteroidFiredEvent;
    [SerializeField] private WormholeReachedEvent wormholeReachedEvent;
    [SerializeField] private AsteroidDestroyedEvent asteroidDestroyedEvent;
    [SerializeField] private ToggleEvent toggleEvent;
    [SerializeField] private TetherEvent tetherEvent;
    [SerializeField] private BarrierDestroyedEvent barrierDestroyedEvent;
    private void OnEnable()
    {
        energyPickupEvent.AddListener(OnEnergyPickup);
        asteroidFiredEvent.AddListener(OnAsteroidFired);
        wormholeReachedEvent.AddListener(OnWormholeReached);
        asteroidDestroyedEvent.AddListener(OnAsteroidDestroyed);
        toggleEvent.AddListener(OnToggle);
        tetherEvent.AddListener(OnTether);
        barrierDestroyedEvent.AddListener(OnBarrierDestroyed);
    }
    private void OnDisable()
    {
        energyPickupEvent.RemoveListener(OnEnergyPickup);
        asteroidFiredEvent.RemoveListener(OnAsteroidFired);
        wormholeReachedEvent.RemoveListener(OnWormholeReached);
        asteroidDestroyedEvent.RemoveListener(OnAsteroidDestroyed);
        toggleEvent.RemoveListener(OnToggle);
        tetherEvent.RemoveListener(OnTether);
        barrierDestroyedEvent.RemoveListener(OnBarrierDestroyed);
    }
    private void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    private void OnEnergyPickup(object sender, EventParameters args)
    {
        PlayAudioClip(energyPickup);
    }
    private void OnAsteroidFired(object sender, EventParameters args)
    {
        PlayAudioClip(asteroidFired);
    }
    private void OnWormholeReached(object sender, EventParameters args)
    {
        PlayAudioClip(wormholeReached);
    }
    private void OnAsteroidDestroyed(object sender, EventParameters args)
    {
        PlayAudioClip(asteroidDestroyed);
    }
    private void OnToggle(object sender, EventParameters args)
    {
        PlayAudioClip(toggle);
    }
    private void OnTether(object sender, EventParameters args)
    {
        PlayAudioClip(tether);
    }
    private void OnBarrierDestroyed(object sender, EventParameters args)
    {
        PlayAudioClip(barrierDestroyed);
    }
}
