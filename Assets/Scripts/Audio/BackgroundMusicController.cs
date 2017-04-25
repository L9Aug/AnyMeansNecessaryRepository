using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicController : MonoBehaviour
{
    public List<AudioClip> PossibleTracks = new List<AudioClip>();
    AudioSource mySource;

    bool isQueued = false;

    // Use this for initialization
    void Start()
    {
        mySource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mySource.isPlaying)
        {
            if (!isQueued)
            {
                PlayNextTrack();
            }
        }
    }

    void PlayNextTrack()
    {
        isQueued = true;
        StartCoroutine(DelayedPlaying());
    }

    IEnumerator DelayedPlaying()
    {
        isQueued = true;
        yield return new WaitForSeconds(Random.Range(1f, 10f));

        mySource.clip = PossibleTracks[Random.Range(0, PossibleTracks.Count)];
        mySource.Play();
        isQueued = false;
    }

}
