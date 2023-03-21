using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInfo : MonoBehaviour
{
    void StartEvent()
    {
        MusicSelect.instance.CurrentMusicInfo();
        MusicSelect.instance.VideoStart();
    }
    
    
}
