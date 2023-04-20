using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    public float startTime;
    
    public float judgeTime;
    public bool judgeStart;
    
    public SpriteRenderer fadeIn;
    public bool enableFadeIn;
    Color fadeInColor;
    
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

    public GameObject bgaOff;
    public Transform inGameUI;
    public TMP_Text[] JudgeValue;
    private int Under90;

    public GameObject fever;
    public GameObject feverText;
    private int feverCount = 1;
    private int feverGauge;

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

    public Transform gearPosition;
    public SpriteRenderer gearTransparency;
    
    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;
    
    public string music;

    public AudioSource audioSource;
    public VideoPlayer videoSource;
    
    public RawImage videoBackGround;
    private AudioSource colorChageSound;
    private byte videoColor = 255;

    public GameObject pause;
    private int pauseMenuIndex;
    public Material[] gradientMat;
    private Color leftCol;
    private Color rightCol;
    public TMP_Text[] pauseMenuText;
    
    // 음악을 실행하는 함수
    IEnumerator MusicStart()
    {
        music = PlayData.music;
        AudioClip audioClip = Resources.Load<AudioClip>("Audio/" + music);
        VideoClip videoClip = Resources.Load<VideoClip>("Video/" + music);
        videoSource = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        
        audioSource.clip = audioClip;
        videoSource.clip = videoClip;
        videoSource.audioOutputMode = VideoAudioOutputMode.None;
        
        yield return new WaitForSeconds(MusicSelect.instance.syncTime);
        audioSource.Play();
        videoSource.Play();
        judgeStart = true;
    }
    
    void Start()
    {
        StartCoroutine("MusicStart");
        ChangeFeverColor();
        GearOptimize();
       
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
        if (!pause.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.D)) ShineTrail(0);
            else if (Input.GetKeyUp(KeyCode.D)) DarkTrail(0);

            if (Input.GetKey(KeyCode.F)) ShineTrail(1);
            else if (Input.GetKeyUp(KeyCode.F)) DarkTrail(1);

            if (Input.GetKey(KeyCode.L)) ShineTrail(2);
            else if (Input.GetKeyUp(KeyCode.L)) DarkTrail(2);

            if (Input.GetKey(KeyCode.Semicolon)) ShineTrail(3);
            else if (Input.GetKeyUp(KeyCode.Semicolon)) DarkTrail(3);

            // 인게임중 노트 스피드 바꾸면 싱크 안맞음 수정 필요
            /*if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                MusicSelect.instance.noteSpeed -= 1f;
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MusicSelect.instance.noteSpeed += 1f;
            }*/

            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                if (videoColor < 255)
                {
                    colorChageSound.Play();
                    videoColor += 51;
                }

                videoBackGround.color = new Color32(videoColor, videoColor, videoColor, 255);
            }

            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                if (videoColor > 0)
                {
                    colorChageSound.Play();
                    videoColor -= 51;
                }

                videoBackGround.color = new Color32(videoColor, videoColor, videoColor, 255);
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                if (bgaOff.activeInHierarchy)
                {
                    bgaOff.SetActive(false);
                }
                else
                {
                    bgaOff.SetActive(true);
                }
            }
        }

        FeverSystem();
        FadeIn();
        Pause();
    }

    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.15f;
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
            ChangeFeverColor();
            feverText.transform.GetChild(1).gameObject.SetActive(false);
            PlayData.HitScore[0] += 1; 
            judgeText.text = "<color=#FF0000>BREAK";
            combo = 0;
            feverGauge = 0;
            feverCount = 1;
        }
        else if (judge == judges.MAX1)
        {
            PlayData.HitScore[1] += 1; 
            rate += 1f;
            judgeText.text = "<color=#6E7072>MAX 1%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX10)
        {
            PlayData.HitScore[2] += 1; 
            rate += 10f;
            judgeText.text = "<color=#1F5A90>MAX 10%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX20)
        {
            PlayData.HitScore[3] += 1;
            rate += 20f;
            judgeText.text = "<color=#1F5A90>MAX 20%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX30)
        {
            PlayData.HitScore[4] += 1;
            rate += 30f;
            judgeText.text = "<color=#1F5A90>MAX 30%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX40)
        {
            PlayData.HitScore[5] += 1;
            rate += 40f;
            judgeText.text = "<color=#1F5A90>MAX 40%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX50)
        {
            PlayData.HitScore[6] += 1;
            rate += 50f;
            judgeText.text = "<color=#0BBC00>MAX 50%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX60)
        {
            PlayData.HitScore[7] += 1;
            rate += 60f;
            judgeText.text = "<color=#0BBC00>MAX 60%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX70)
        {
            PlayData.HitScore[8] += 1;
            rate += 70f;
            judgeText.text = "<color=#0BBC00>MAX 70%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX80)
        {
            PlayData.HitScore[9] += 1;
            rate += 80f;
            judgeText.text = "<color=#3AFF00>MAX 80%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX90)
        {
            PlayData.HitScore[10] += 1;
            rate += 90f;
            judgeText.text = "<color=#9FFF00>MAX 90%";
            combo += feverCount;
            feverGauge++;
        }
        else if (judge == judges.MAX100)
        {
            PlayData.HitScore[11] += 1;
            rate += 100f;
            judgeText.text = "<color=#FFFF00>MAX 100%";
            combo += feverCount;
            feverGauge++;
        }

        Under90 = 0;
        if(MusicSelect.instance.fever != 2) FeverSystem();

        

        for (int i = 2; i < 11; i++)
        {
            Under90 += PlayData.HitScore[i];
        }

        JudgeValue[0].text = PlayData.HitScore[11].ToString();
        JudgeValue[1].text = Under90.ToString();
        JudgeValue[2].text = PlayData.HitScore[1].ToString();
        JudgeValue[3].text = PlayData.HitScore[0].ToString();
        JudgeValue[3].text = PlayData.HitScore[0].ToString();
            
        percent = rate / noteCount;
        comboText.text = $"<size=100%>combo\n<size=200%>{combo.ToString()}";
        rateText.text = $"<size=80%>RATE</size>  <b>{percent.ToString("N2")}%";
        comboAnimator.SetTrigger("SHOW");
        judgementAnimator.SetTrigger("SHOW");
    }

    private void ChangeFeverColor()
    {
        SpriteRenderer feverColor = fever.GetComponent<SpriteRenderer>();
        if (feverCount == 1)
        {
            feverColor.color = new Color(150f / 255f, 150f / 255f, 150f / 255f);
            feverText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = new Color(150f / 255f, 150f / 255f, 150f / 255f);
        }
        else if (feverCount == 2)
        {
            feverColor.color = new Color(81f / 255f, 183f / 255f, 203f / 255f);
            feverText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = new Color(81f / 255f, 183f / 255f, 203f / 255f);
        }
        else if (feverCount == 3)
        {
            feverColor.color = new Color(61f / 255f, 228f / 255f, 128f / 255f);
            feverText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = new Color(61f / 255f, 228f / 255f, 128f / 255f);
        }
        else if (feverCount == 4)
        {
            feverColor.color = new Color(155f / 255f, 226f / 255f, 34f / 255f);
            feverText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = new Color(155f / 255f, 226f / 255f, 34f / 255f);
        }
        else
        {
            feverColor.color = new Color(234f / 255f, 224f / 255f, 0f);
            feverText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().color = new Color(234f / 255f, 224f / 255f, 0f);
        }
    }

    private void FeverSystem()
    {
        if (feverGauge <= 42)
        {
            fever.transform.localScale =
                new Vector3(feverGauge, fever.transform.localScale.y, fever.transform.localScale.z);
        }

        if (feverGauge > 42)
        {
            if (feverGauge < 44)
            {
                feverText.transform.GetChild(1).gameObject.SetActive(true);
                feverText.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("SHOW");
            }

            if (MusicSelect.instance.fever == 0)
            {
                fever.GetComponent<AudioSource>().Play();
                feverGauge = 0;
                feverCount++;
                if (feverCount > 5)
                {
                    feverCount = 5;
                }

                ChangeFeverColor();
                feverText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "x" + feverCount.ToString();
                feverText.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("SHOW");
                feverText.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("OUT");
                Invoke("FeverGlowOut", 1f);
            }
            
            else if (MusicSelect.instance.fever == 1)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    fever.GetComponent<AudioSource>().Play();
                    feverGauge = 0;
                    feverCount++;
                    if (feverCount > 5)
                    {
                        feverCount = 5;
                    }

                    ChangeFeverColor();
                    feverText.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = "x" + feverCount.ToString();
                    feverText.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("SHOW");
                    feverText.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("OUT");
                    Invoke("FeverGlowOut", 1f);
                }
            }
        }
    }

    private void FeverGlowOut()
    {
        feverText.transform.GetChild(1).gameObject.SetActive(false);
    }
    private void GearOptimize()
    {
        int pos = MusicSelect.instance.gearPosition;
        
        if (pos == 0)
        {
            inGameUI.position = new Vector3(inGameUI.position.x + 6f, inGameUI.position.y, inGameUI.position.z);
        }

        gearPosition.position =
            new Vector3(gearPosition.position.x + (pos * 6f), gearPosition.position.y, gearPosition.position.z);
        
        rateUI.transform.position =
            new Vector3(rateUI.transform.position.x + (pos * 6f), rateUI.transform.position.y,
                rateUI.transform.position.z);
        
        comboUI.transform.position =
            new Vector3(comboUI.transform.position.x + (pos * 6f), comboUI.transform.position.y,
                comboUI.transform.position.z);
        
        judgementUI.transform.position =
            new Vector3(judgementUI.transform.position.x + (pos * 6f), judgementUI.transform.position.y,
                judgementUI.transform.position.z);
        
        for (int i = 0; i < 4; i++)
        {
            trails[i].transform.position =
                new Vector3(trails[i].transform.position.x + (pos * 6f), trails[i].transform.position.y,
                    trails[i].transform.position.z);
        }

        Color color;
        color = gearTransparency.color;
        color.a = 1.0f - MusicSelect.instance.transparency * 0.01f;
        gearTransparency.color = color;

        color = rateUI.GetComponent<TMP_Text>().color;
        if (MusicSelect.instance.rate == 0) color.a = 0f;
        else color.a = 1.0f;
        rateUI.GetComponent<TMP_Text>().color = color;
    }
    
    private void FadeIn()
    {
        if (enableFadeIn)
        {
            if (fadeIn.color.a < 1.0f)
            {
                fadeInColor.a += MusicSelect.instance.fadeTime * Time.deltaTime;
                fadeIn.color = fadeInColor;
            }

            else
            {
                Invoke("Result", 2.0f);
            }
        }
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause.activeInHierarchy)
            {
                for (int i = 0; i < 3; i++)
                {
                    gradientMat[i].SetColor("_Color", new Color(0f, 0f, 0f));
                    gradientMat[i].SetColor("_Color2", new Color(0f, 0f, 0f));
                }
                pause.SetActive(true);
                audioSource.Pause();
                videoSource.Pause();
                StartCoroutine(EnablePauseMenu());
                DisablePauseMenu();
            }
        }
        
        if (pause.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                leftCol = new Color(0f, 0f, 0f);
                rightCol = new Color(0f, 0f, 0f);
                pauseMenuIndex -= 1;
                if (pauseMenuIndex < 0) pauseMenuIndex = 2;
                StartCoroutine(EnablePauseMenu());
                DisablePauseMenu();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                leftCol = new Color(0f, 0f, 0f);
                rightCol = new Color(0f, 0f, 0f);
                pauseMenuIndex += 1;
                if (pauseMenuIndex > 2) pauseMenuIndex = 0;
                StartCoroutine(EnablePauseMenu());
                DisablePauseMenu();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (pauseMenuIndex == 0)
                    SceneManager.LoadScene("GameScene");
                else if (pauseMenuIndex == 1)
                    SceneManager.LoadScene("SelectScene");
                else
                {
                    Application.Quit();
                }
            }
        }
    }

    IEnumerator EnablePauseMenu()
    { 
       while (leftCol.r <= 1.0f)
       {
            leftCol.r += 0.01f; leftCol.g += 0.00815f; leftCol.b += 0.00047f;
            rightCol.r += 0.01f; rightCol.g += 0.00517f; rightCol.b += 0.00549f;
            gradientMat[pauseMenuIndex].SetColor("_Color", leftCol);
            gradientMat[pauseMenuIndex].SetColor("_Color2", rightCol);
            yield return new WaitForSeconds(0.01f);
       }
    }

    void DisablePauseMenu()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i != pauseMenuIndex)
            {
                gradientMat[i].SetColor("_Color", new Color(0f,0f,0f));
                gradientMat[i].SetColor("_Color2", new Color(0f,0f,0f));
                pauseMenuText[i].color = new Color(1f, 1f, 1f);
            }
            else
            {
                pauseMenuText[i].color = new Color(0f, 0f, 0f);
            }
        }
    }

    public void Result()
    {
        PlayData.combo = combo;
        PlayData.rate = percent;
        PlayData.totalNote = noteCount;
        PlayData.bestCombo = combo;
        SceneManager.LoadScene("ResultScene");
    }
    
}
