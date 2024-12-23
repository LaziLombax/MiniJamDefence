using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public int audioSourcePoolSize = 10; // Number of audio sources in the pool

    [Header("Audio Clips")]
    public AudioClip[] laserSounds;
    public AudioClip[] blasterSounds;
    public AudioClip[] asteroidHitSounds;
    // Add other audio clip arrays here

    private List<AudioSource> audioSourcePool;

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSourcePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSourcePool()
    {
        audioSourcePool = new List<AudioSource>();

        for (int i = 0; i < audioSourcePoolSize; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSourcePool.Add(audioSource);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource audioSource in audioSourcePool)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        // If all audio sources are playing, add a new one to the pool
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        audioSourcePool.Add(newAudioSource);
        return newAudioSource;
    }

    // Play a single sound effect from an array of clips
    public void PlaySound(AudioClip[] clips, bool loop = false)
    {
        if (clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }

    // Play laser sound with optional looping
    public void PlayLaserSound(bool loop = false)
    {
        PlaySound(laserSounds, loop);
    }

    // Stop laser sound
    public void StopLaserSound()
    {
        foreach (AudioSource audioSource in audioSourcePool)
        {
            if (audioSource.clip != null && System.Array.Exists(laserSounds, clip => clip == audioSource.clip) && audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = null;
            }
        }
    }
}
