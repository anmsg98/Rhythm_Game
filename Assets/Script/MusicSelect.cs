using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Mono.Cecil;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MusicSelect : MonoBehaviour
{
    
    public static MusicSelect instance { get; set; }
    
    private List<string> musicList;
    private int currentMusic;
    private int countMusic;
    
    private Color fadeInColor;
    
    public Animator musicInfoAnim;
    public Animator optionBoxAnim;
    
    public GameObject Music;
    public VideoPlayer videoSource;
    public TMP_Text musicInfo;
    public SpriteRenderer coverImage;
    public Image fadeIn;

    public GameObject MenuUi;
    public GameObject optionBlur;
    
    public AudioSource optionSound;
    public SpriteRenderer[] optionSprite;
    public GameObject[] optionText;
    public GameObject selectedBox;
    private int optionIndex;
    public float noteSpeed;
    private int fever;
    private int fader;
    public int chaos;
    public int transparency;
    public Scrollbar transparecyScroll;
    public int gearPosition;
    public int rate;

    public GameObject QuitUI;

    private void Awake()
    {
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Start()
    {
       
        Initialize();
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

    void OptionBox()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (optionBoxAnim.GetBool("IN_OUT"))
            {
                optionSound.clip = Resources.Load<AudioClip>("Audio/Menu off");
                optionSound.Play();
                optionBoxAnim.SetBool("IN_OUT", false);
            }
            else
            {
                optionSound.clip = Resources.Load<AudioClip>("Audio/Menu on");
                optionSound.Play();
                optionBoxAnim.SetBool("IN_OUT", true);
            }
        }
    }
    
    void UpScroll()
    {
        AudioSource scroll = GetComponent<AudioSource>();
        scroll.clip = Resources.Load<AudioClip>("Audio/Scroll");
        scroll.Play();
        
        videoSource.Stop();
        currentMusic -= 1;
        if (currentMusic < 0)
            currentMusic = countMusic - 1;
        musicInfoAnim.SetTrigger("Rotate");
        Highlight();
    }

    void DownScroll()
    {
        AudioSource scroll = GetComponent<AudioSource>();
        scroll.clip = Resources.Load<AudioClip>("Audio/Scroll");
        scroll.Play();
        
        videoSource.Stop();
        currentMusic += 1;
        if (currentMusic >= countMusic)
            currentMusic = 0;
        musicInfoAnim.SetTrigger("Rotate");
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
        AudioSource gameStart = GetComponent<AudioSource>();
        gameStart.clip = Resources.Load<AudioClip>("Audio/Start");
        gameStart.Play();
        while (fadeIn.color.a <= 1.0f)
        {
            yield return new WaitForSeconds( 0.001f );
            fadeInColor.a += 0.01f;
            fadeIn.color = fadeInColor;
        }
        yield return new WaitForSeconds( 2f );
        GameStart();
    }

    void SelectedBox()
    {
        for (int i = 0; i < 7; i++)
        {
            if (i == optionIndex)
            {
                optionSprite[i].sprite = Resources.Load<Sprite>("Sprites/Option_Bar_2");
            }
            else
            {
                optionSprite[i].sprite = Resources.Load<Sprite>("Sprites/Option_Bar");
            }
        }
        Transform selectPos = selectedBox.GetComponent<Transform>();
        AudioSource selectSound = selectedBox.GetComponent<AudioSource>();
        
        selectSound.Play();
        
        if (optionIndex < 4)
        {
            selectPos.position = new Vector3(selectPos.position.x, 3.6f - (optionIndex * 0.55f), selectPos.position.z);
        }
        else
        {
            selectPos.position = new Vector3(selectPos.position.x, 0.4f - ((optionIndex - 4) * 0.55f),
                selectPos.position.z);
        }
    }

    void OptionOptimize(int value)
    {
        TMP_Text textValue;
        AudioSource optimizeSound = selectedBox.GetComponent<AudioSource>();
        
        optimizeSound.Play();
        
        if (optionIndex == 0)
        {
            noteSpeed += (value * 0.5f);
            if (noteSpeed < 5) noteSpeed = 5f;
            else if (noteSpeed > 20.0f) noteSpeed = 20.0f;
        
            textValue = optionText[optionIndex].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
            textValue.text = noteSpeed.ToString("F1");
        }
        // 피버 기능 추가 해야됨
        else if (optionIndex == 1)
        {
            fever += value;
            if (fever < 0) fever = 0;
            else if (fever > 2) fever = 2;
            
            textValue = optionText[optionIndex].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
            if (fever == 0) textValue.text = "AUTO X5";
            else if (fever == 1) textValue.text = "X5";
            else if (fever == 2) textValue.text = "OFF";
        }
        // 페이더 기능 추가해야됨 (블링크, 페이드 1,2 등등..)
        else if (optionIndex == 2)
        {
            fader += value;
            if (fader < 0) fader = 0;
            else if (fader > 2) fader = 2;
            
            textValue = optionText[optionIndex].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
            if (fader == 0) textValue.text = "OFF";
            else if (fader == 1) textValue.text = "FADER 1";
            else if (fader == 2) textValue.text = "FADER 2";
        }
        else if (optionIndex == 3)
        {
            chaos += value;
            if (chaos < 0) chaos = 0;
            else if (chaos > 2) chaos = 2;
            
            textValue = optionText[optionIndex].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
            if (chaos == 0) textValue.text = "OFF";
            else if (chaos == 1) textValue.text = "MIRROR";
            else if (chaos == 2) textValue.text = "RANDOM";
        }
        else if (optionIndex == 4)
        {
            transparency += value * 10;
            if (transparency < 0) transparency = 0;
            else if (transparency > 100) transparency = 100;

            transparecyScroll.value = transparency * 0.01f;
            
            textValue = optionText[optionIndex].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
            textValue.text = transparency.ToString() + "%";
        }
        else if (optionIndex == 5)
        {
            gearPosition += value;
            if (gearPosition < -1) gearPosition = -1;
            else if (gearPosition > 1) gearPosition = 1;
            
            textValue = optionText[optionIndex].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
            if (gearPosition == -1) textValue.text = "LEFT";
            else if (gearPosition == 0) textValue.text = "CENTER";
            else if (gearPosition == 1) textValue.text = "RIGHT";
        }
        else if (optionIndex == 6)
        {
            rate += value;
            if (rate < 0) rate = 0;
            else if (rate > 1) rate = 1;
            
            textValue = optionText[optionIndex].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
            if (rate == 0) textValue.text = "OFF";
            else if (rate == 1) textValue.text = "ON";
        }
    }

    void Initialize()
    {
        string textAsset = Application.streamingAssetsPath + "/Option/PlayerOption.txt";
        StreamReader reader = new StreamReader(textAsset);
        string line;
        line = reader.ReadLine();
        noteSpeed = (float) Convert.ToDouble(line.Split(' ')[1]);
        line = reader.ReadLine();
        fever = Convert.ToInt32(line.Split(' ')[1]);
        line = reader.ReadLine();
        fader = Convert.ToInt32(line.Split(' ')[1]);
        line = reader.ReadLine();
        chaos = Convert.ToInt32(line.Split(' ')[1]);
        line = reader.ReadLine();
        transparency = Convert.ToInt32(line.Split(' ')[1]);
        line = reader.ReadLine();
        gearPosition = Convert.ToInt32(line.Split(' ')[1]);
        line = reader.ReadLine();
        rate = Convert.ToInt32(line.Split(' ')[1]);
        
        TMP_Text textValue;

        textValue = optionText[0].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
        textValue.text = noteSpeed.ToString("F1");

        textValue = optionText[1].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
        if (fever == 0) textValue.text = "AUTO X5";
        else if (fever == 1) textValue.text = "X5";
        else if (fever == 2) textValue.text = "OFF";

        textValue = optionText[2].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
        if (fader == 0) textValue.text = "OFF";
        else if (fader == 1) textValue.text = "FADER 1";
        else if (fader == 2) textValue.text = "FADER 2";

        textValue = optionText[3].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
        if (chaos == 0) textValue.text = "OFF";
        else if (chaos == 1) textValue.text = "MIRROR";
        else if (chaos == 2) textValue.text = "RANDOM";

        transparecyScroll.value = transparency * 0.01f;
        textValue = optionText[4].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
        textValue.text = transparency.ToString() + "%";

        textValue = optionText[5].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
        if (gearPosition == -1) textValue.text = "LEFT";
        else if (gearPosition == 0) textValue.text = "CENTER";
        else if (gearPosition == 1) textValue.text = "RIGHT";

        textValue = optionText[6].transform.Find("Value").gameObject.GetComponent<TMP_Text>();
        if (rate == 0) textValue.text = "OFF";
        else if (rate == 1) textValue.text = "ON";
    }

    void SaveOption()
    {
        string path = Application.streamingAssetsPath + "/Option/PlayerOption.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine("NoteSpeed " + noteSpeed.ToString("F1"));
        writer.WriteLine("fever " + fever.ToString());
        writer.WriteLine("fader " + fader.ToString());
        writer.WriteLine("chaos " + chaos.ToString());
        writer.WriteLine("transparency " + transparency.ToString());
        writer.WriteLine("gearPosition " + gearPosition.ToString());
        writer.WriteLine("rate " + rate.ToString());
        writer.Close();
    }
    
    void GameStart()
    {
        PlayData.music = musicList[currentMusic];
        SceneManager.LoadScene("GameScene");
    }
    
    void Update()
    {
        if (!QuitUI.activeInHierarchy)
        {
            if (optionBoxAnim.GetBool("IN_OUT") == false)
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
                    MenuUi.SetActive(false);
                }
                
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    optionSound.clip = Resources.Load<AudioClip>("Audio/Menu off");
                    optionSound.Play();
                    MenuUi.SetActive(false);
                    QuitUI.SetActive(true);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    optionIndex -= 1;
                    if (optionIndex < 0) optionIndex = 6;
                    SelectedBox();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    optionIndex += 1;
                    if (optionIndex > 6) optionIndex = 0;
                    SelectedBox();
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OptionOptimize(-1);
                    SaveOption();
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OptionOptimize(1);
                    SaveOption();
                }
            }
            
            OptionBox();
            
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                optionSound.clip = Resources.Load<AudioClip>("Audio/Menu off");
                optionSound.Play();
                MenuUi.SetActive(true);
                QuitUI.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Application.Quit();
            }
        }
    }
}
