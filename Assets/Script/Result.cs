using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Result : MonoBehaviour
{
    private VideoPlayer videoSource;
    void Start()
    {
        VideoClip videoClip = Resources.Load<VideoClip>("Video/" + PlayData.music);
        videoSource = GetComponent<VideoPlayer>();

        videoSource.clip = videoClip;
        videoSource.SetDirectAudioMute(0, true);
        videoSource.Play();
        
        Debug.Log(PlayData.combo);
        Debug.Log(PlayData.rate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
