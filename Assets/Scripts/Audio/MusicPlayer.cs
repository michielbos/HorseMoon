using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    AudioSource source;
    public bool IsPlaying => source.isPlaying;

    // WTF is this, honeslty, this is so backwards it hurts. But tiz what it is.
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.priority = 0;

        var theme = FindObjectOfType<MusicTheme>();
        if (theme)
            PlaySong(theme.clip, theme.delay);
    }

    void Update()
    {
        source.volume = Volume.Music;
    }
    
    public void PlaySong(AudioClip clip, float delay = 0f)
    {
        StopAllCoroutines();
        if (clip == null)
            source.Stop();
        else if(clip != source.clip)
        {
            if(delay > 0f)
            {
                StartCoroutine(PlayDelayed(clip, delay));
            }
            else
            {
                source.clip = clip;
                source.PlayDelayed(.5f);
            }
        }
    }

    IEnumerator PlayDelayed(AudioClip clip, float delay = 0f)
    {
        for(; ; )
        {
            yield return new WaitForSeconds(delay);
            source.clip = clip;
            source.loop = false;
            source.Play();
            yield return new WaitForSecondsRealtime(clip.length);
        }
    }
}
