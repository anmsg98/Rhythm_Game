using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using Unity.Mathematics;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MusicSelect : MonoBehaviour
{
    
    public static MusicSelect instance { get; set; }
    
    private List<string> musicList;
    private int currentMusic;
    private int countMusic;

    private Color fadeInColor;
    
    public Animator anim;
    public GameObject Music;
    public VideoPlayer videoSource;
    public TMP_Text musicInfo;
    public SpriteRenderer coverImage;
    public Image fadeIn;
    
    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        fadeInColor = fadeIn.color;
        musicList = new List<string>();
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/Music List");
        StringReader reader = new StringReader(textAsset.text);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            Vector3 transform = new Vector3(0, 0, 0);
            
            
            string title = line;
            musicList.Add(title);
            GameObject instant = Instantiate(Music);
            instant.transform.position += new Vector3(0, -countMusic, 0);
            instant.name = countMusic.ToString();
            
            SpriteRenderer thumbnail = instant.transform.Find("Thumbnail").gameObject.GetComponent<SpriteRenderer>();
            thumbnail.sprite = Resources.Load<Sprite>("Sprites/" + title + "_T");


            TMP_Text text = instant.transform.Find("Title").transform.Find("Music Title").transform.Find("Text").gameObject.GetComponent<TMP_Text>();
            text.text = title;
            
            countMusic += 1;
        }
        
        VideoStart();
        CurrentMusicInfo();
        Highlight();
    }

    void UpScroll()
    {
        videoSource.Stop();
        currentMusic -= 1;
        if (currentMusic < 0)
            currentMusic = countMusic - 1;
        anim.SetTrigger("Rotate");
        Highlight();
    }

    void DownScroll()
    {
        videoSource.Stop();
        currentMusic += 1;
        if (currentMusic >= countMusic)
            currentMusic = 0;
        anim.SetTrigger("Rotate");
        Highlight();
    }

    public void VideoStart()
    {
        VideoClip videoClip = Resources.Load<VideoClip>("Video/" + musicList[currentMusic]);
        videoSource = GetComponent<VideoPlayer>();
        videoSource.clip = videoClip;
        videoSource.SetDirectAudioVolume(0, 0.2f);
        videoSource.Play();
    }

    public void CurrentMusicInfo()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + musicList[currentMusic]);
        StringReader reader = new StringReader(textAsset.text);
        // 곡제목
        string Title = reader.ReadLine();
        // 아티스트 정보
        string Artist = reader.ReadLine();
        // bpm 정보
        string beatInformation = reader.ReadLine();
        int bpm = Convert.ToInt32(beatInformation.Split(' ')[0]);
        coverImage.sprite = Resources.Load<Sprite>("Sprites/" + musicList[currentMusic]);
        musicInfo.text = Title + "\n" + Artist + "\n" + "BPM " + bpm.ToString();
        
    }

    void Highlight()
    {
        for (int i = 0; i < countMusic; i++)
        {
            GameObject Parent = GameObject.Find(i.ToString());
            
            SpriteRenderer thumbnail = Parent.transform.Find("Thumbnail").gameObject.GetComponent<SpriteRenderer>();
            SpriteRenderer title = Parent.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            TMP_Text text = Parent.transform.Find("Title").transform.Find("Music Title").transform.Find("Text").gameObject.GetComponent<TMP_Text>();

            Color color;

            if (i == currentMusic)
            {
                color = thumbnail.color;
                color.a = 1.0f;
                thumbnail.color = color;
                
                color = title.color;
                color.a = 0.5f;
                title.color = color;

                color = text.color;
                color.a = 1.0f;
                text.color = color;
            }
            else
            {
                color = thumbnail.color;
                color.a = 0.5f;
                thumbnail.color = color;
                
                color = title.color;
                color.a = 0.0f;
                title.color = color;

                color = text.color;
                color.a = 0.5f;
                text.color = color;
            }
        }
    }

    IEnumerator FadeIn()
    {
        while (fadeIn.color.a <= 1.0f)
        {
            yield return new WaitForSeconds( 0.001f );
            fadeInColor.a += 0.001f;
            fadeIn.color = fadeInColor;
        }
        GameStart();
    }
    void GameStart()
    {
        PlayData.music = musicList[currentMusic];
        SceneManager.LoadScene("GameScene");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpScroll();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownScroll();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine("FadeIn");
        }
    }
}
