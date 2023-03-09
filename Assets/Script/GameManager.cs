using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }
    
    public float noteSpeed;
    public float judgeTime;

    public GameObject rateUI;
    private TMP_Text rateText;
    private float rate;
    private float percent;
    private int noteCount;

    public GameObject comboUI;
    private TMP_Text comboText;
    private int combo = 0;
    private Animator comboAnimator;

    public GameObject judgementUI;
    private TMP_Text judgeText;
    private string judge;
    private Animator judgementAnimator;

    public GameObject objectPooler;
    public enum judges
    {
        NONE = 0,
        MAX100,
        MAX90,
        MAX80,
        MAX70,
        MAX60,
        MAX50,
        MAX40,
        MAX30,
        MAX20,
        MAX10,
        MAX1,
        Break
    };

    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;

    
    public string music;
    public AudioSource audioSource;
    
    
    public VideoPlayer videoSource;
    public RawImage videoBackGround;
    private AudioSource colorChageSound;
    private byte videoColor = 255;
    
    // 음악을 실행하는 함수
    void MusicStart()
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Beats/" + music);
        VideoClip videoClip = Resources.Load<VideoClip>("Video/" + music);
        audioSource = GetComponent<AudioSource>();
        videoSource = GetComponent<VideoPlayer>();
        
        audioSource.clip = audioClip;
        videoSource.clip = videoClip;
        
        videoSource.Play();
    }


    
    void Start()
    {
        Invoke("MusicStart", 2.5f);
        comboText = comboUI.GetComponent<TMP_Text>();
        judgeText = judgementUI.GetComponent<TMP_Text>();
        rateText = rateUI.GetComponent<TMP_Text>();
        comboAnimator = comboUI.GetComponent<Animator>();
        judgementAnimator = judgementUI.GetComponent<Animator>();
        colorChageSound = videoBackGround.GetComponent<AudioSource>();

        trailSpriteRenderers = new SpriteRenderer[trails.Length];
        for (int i = 0; i < trails.Length; i++)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S)) ShineTrail(0);
        else if (Input.GetKeyUp(KeyCode.S)) DarkTrail(0);

        if (Input.GetKey(KeyCode.D)) ShineTrail(1);
        else if (Input.GetKeyUp(KeyCode.D)) DarkTrail(1);

        if (Input.GetKey(KeyCode.L)) ShineTrail(2);
        else if (Input.GetKeyUp(KeyCode.L)) DarkTrail(2);

        if (Input.GetKey(KeyCode.Semicolon)) ShineTrail(3);
        else if (Input.GetKeyUp(KeyCode.Semicolon)) DarkTrail(3);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            noteSpeed -= 1f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            noteSpeed += 1f;
        }
        
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            if (videoColor < 255)
            {
                colorChageSound.Play();
                videoColor += 51;
            }
            videoBackGround.color = new Color32(videoColor, videoColor, videoColor, 255);
        }
        
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (videoColor > 0)
            {
                colorChageSound.Play();
                videoColor -= 51;
            }
            videoBackGround.color = new Color32(videoColor, videoColor, videoColor, 255);
        }
    }

    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.32f;
        trailSpriteRenderers[index].color = color;
    }

    public void DarkTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0f;
        trailSpriteRenderers[index].color = color;
    }

    public void ProcessJudge(judges judge)
    {
        noteCount++;
        if (judge == judges.Break)
        {
            PlayData.HitScore[0] += 1; 
            judgeText.text = "<color=#FF0000>BREAK";
            combo = 0;
        }
        else if (judge == judges.MAX1)
        {
            PlayData.HitScore[1] += 1; 
            rate += 1f;
            judgeText.text = "<color=#6E7072>MAX 1%";
            combo++;
        }
        else if (judge == judges.MAX10)
        {
            PlayData.HitScore[2] += 1; 
            rate += 10f;
            judgeText.text = "<color=#1F5A90>MAX 10%";
            combo++;
        }
        else if (judge == judges.MAX20)
        {
            PlayData.HitScore[3] += 1;
            rate += 20f;
            judgeText.text = "<color=#1F5A90>MAX 20%";
            combo++;
        }
        else if (judge == judges.MAX30)
        {
            PlayData.HitScore[4] += 1;
            rate += 30f;
            judgeText.text = "<color=#1F5A90>MAX 30%";
            combo++;
        }
        else if (judge == judges.MAX40)
        {
            PlayData.HitScore[5] += 1;
            rate += 40f;
            judgeText.text = "<color=#1F5A90>MAX 40%";
            combo++;
        }
        else if (judge == judges.MAX50)
        {
            PlayData.HitScore[6] += 1;
            rate += 50f;
            judgeText.text = "<color=#0BBC00>MAX 50%";
            combo++;
        }
        else if (judge == judges.MAX60)
        {
            PlayData.HitScore[7] += 1;
            rate += 60f;
            judgeText.text = "<color=#0BBC00>MAX 60%";
            combo++;
        }
        else if (judge == judges.MAX70)
        {
            PlayData.HitScore[8] += 1;
            rate += 70f;
            judgeText.text = "<color=#0BBC00>MAX 70%";
            combo++;
        }
        else if (judge == judges.MAX80)
        {
            PlayData.HitScore[9] += 1;
            rate += 80f;
            judgeText.text = "<color=#3AFF00>MAX 80%";
            combo++;
        }
        else if (judge == judges.MAX90)
        {
            PlayData.HitScore[10] += 1;
            rate += 90f;
            judgeText.text = "<color=#9FFF00>MAX 90%";
            combo++;
        }
        else if (judge == judges.MAX100)
        {
            PlayData.HitScore[11] += 1;
            rate += 100f;
            judgeText.text = "<color=#FFFF00>MAX 100%";
            combo++;
        }

        percent = rate / noteCount;
        comboText.text = $"<size=100%>combo\n<size=200%>{combo.ToString()}";
        rateText.text = $"<size=80%>RATE</size>  <b>{percent.ToString("N2")}%";
        comboAnimator.SetTrigger("SHOW");
        judgementAnimator.SetTrigger("SHOW");
    }

    public void Result()
    {
        PlayData.combo = combo;
        PlayData.rate = percent;
        PlayData.music = music;
        PlayData.totalNote = noteCount;
        PlayData.bestCombo = combo;
    }
    
}
