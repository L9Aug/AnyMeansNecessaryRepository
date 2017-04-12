using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioEffectController : MonoBehaviour
{
    public bool OverrideCurrentAudio = false;
    public bool PlayOnAwake = false;

    public List<AudioClip> AudioOptions = new List<AudioClip>();

    private void Start()
    {
        if (PlayOnAwake)
        {
            PlayAudio();
        }
    }

    public void PlayAudio()
    {
        if (AudioOptions.Count > 0)
        {
            AudioSource mySource = GetComponent<AudioSource>();
            if (!mySource.isPlaying || OverrideCurrentAudio)
            {
                mySource.Stop();
                mySource.PlayOneShot(AudioOptions[Random.Range(0, AudioOptions.Count)]);
            }
        }
    }

}
