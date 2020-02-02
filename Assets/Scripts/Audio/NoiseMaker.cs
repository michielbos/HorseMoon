using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    
    public AudioClip sound;
    public float pitchVariety = 0f;
    public float volume = .5f;

    public void MakeSomeNoise() => AudioPool.PlaySound(transform.position, sound, volume, 1f + (.5f - Random.value) * pitchVariety);
}
