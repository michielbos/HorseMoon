using System.Collections;
using UnityEngine;
using ObjectPooling;
using System;

public class AudioPool : ObjectPool
{
    static AudioPool _instance;
    void Start() => _instance = this;

    public bool mute = false;

    static public void PlaySoundBypass(Vector3 position, AudioClip clip, float volume = 1f, float pitch = 1f) => _instance.IPlaySound(position, clip, volume, pitch, 20000, true);
    /// <summary>
    /// Play a sound clip using an object pool
    /// </summary>
    /// <param name="position">world position where the sound comes from, suggested to use transform.postion</param>
    /// <param name="clip">audio clip asset to play</param>
    /// <param name="volume">volume, from 0 to 1, this is multiplied by current audio settings volume, default is 1</param>
    /// <param name="pitch">pitch of the sound.</param>
    static public void PlaySound(Vector3 position, AudioClip clip, float volume = 1f, float pitch = 1f) => _instance.IPlaySound(position, clip, volume, pitch, 20000);
    void IPlaySound(Vector3 position, AudioClip clip, float volume, float pitch, int lowpass, bool bypass = false) => StartCoroutine(PlaySoundRoutine(position, clip, volume, pitch, lowpass, bypass));

    IEnumerator PlaySoundRoutine(Vector3 position, AudioClip clip, float volume, float pitch, int lowpassfilter, bool bypass)
    {
        if(!bypass)
            volume *= Volume.Effects * Volume.Effects;

        if (!mute && volume > 0f && ActiveObjects < 32 && clip != null)
        {

            var audio = Create().GetComponent<AudioSource>();
            audio.clip = clip;
            audio.Play();
            audio.volume = volume;
            var lowpass = audio.GetComponent<AudioLowPassFilter>();
            lowpass.enabled = lowpassfilter < 20000;
            lowpass.cutoffFrequency = lowpassfilter;
            while (audio.isPlaying)
            {
                audio.pitch = pitch;
                yield return null;
            }
            Dispose(audio.gameObject);

        }
    }
    internal static void PlaySoundLowpass(Vector3 position, AudioClip clip, float volume, int frequency) => _instance.IPlaySound(position, clip, volume, 1f, frequency);
}

