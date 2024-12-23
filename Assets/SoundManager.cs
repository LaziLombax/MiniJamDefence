using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public int audioSourcePoolSize = 10; // Number of audio sources in the pool

    [Header("Audio Clips")]
    public SoundClip[] laserSounds;
    public SoundClip[] blasterSounds;
    public SoundClip[] turretSounds;
    public SoundClip[] asteroidHitSounds;
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

    // Play a single sound effect from an array of SoundClip
    public void PlaySound(SoundClip[] soundClips, bool loop = false)
    {
        if (soundClips.Length == 0) return;

        SoundClip soundClip = soundClips[Random.Range(0, soundClips.Length)];
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.loop = loop;
        audioSource.clip = soundClip.clip;
        audioSource.volume = soundClip.volume;
        audioSource.time = soundClip.startTime;
        audioSource.Play();
    }

    // Play laser sound with optional looping
    public void PlayLaserSound(bool loop = false)
    {
        //PlaySound(laserSounds, loop);
    }

    // Stop laser sound
    public void StopLaserSound()
    {
        foreach (AudioSource audioSource in audioSourcePool)
        {
            if (audioSource.clip != null && System.Array.Exists(laserSounds, soundClip => soundClip.clip == audioSource.clip) && audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = null;
            }
        }
    }
}

[System.Serializable]
public class SoundClip
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    public float startTime = 0f; // Start time in seconds
}
