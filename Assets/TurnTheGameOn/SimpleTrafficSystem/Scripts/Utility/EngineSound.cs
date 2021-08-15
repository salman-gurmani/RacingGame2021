﻿namespace TurnTheGameOn.SimpleTrafficSystem
{
    /// A simple audio implimentation example
    /// Attach this script to an AITrafficCar prefab
    /// Assign an audio clip
    /// An audio sound file is included at Assets\TurnTheGameOn\SimpleTrafficSystem\DemoContent\Audio
    /// the pitch will be dynamically adjusted based on the current speed.
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class EngineSound : MonoBehaviour
    {
        private AudioSource _audioSource;
        private AITrafficCar _AITrafficCar;
        private float topSpeed;
        private float currentSpeed;
        public float idlePitch = 0.16f;
        public float spatialBlend = 1f;
        public AudioClip engineAudioClip;

        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.spatialBlend = spatialBlend;
            _audioSource.loop = true;
            _audioSource.clip = engineAudioClip;
            _audioSource.Play();
        }

        private void Start()
        {
            _AITrafficCar = GetComponent<AITrafficCar>();
            topSpeed = _AITrafficCar.topSpeed;
        }

        void Update()
        {
            currentSpeed = _AITrafficCar.CurrentSpeed();
            _audioSource.pitch = currentSpeed <= 3 ? idlePitch : currentSpeed / topSpeed;
        }
    }
}