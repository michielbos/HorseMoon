using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPlayer : MonoBehaviour
{
    public float volume;

    public void Play()
    {
        var src = GetComponent<AudioSource>();
        src.Play();
        src.volume = volume * Volume.Steps;
        src.pitch = .9f + Random.value * .2f;
    }
}
