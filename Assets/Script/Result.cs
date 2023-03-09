using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Video;

public class Result : MonoBehaviour
{
    private VideoPlayer videoSource;
    public Slider[] sliders;
    
    public Animator detailBoxAnim;
    public Animator hitScoreAnim;
   
    private float value;
    private int total100;
    private int total90;
    private float rateUp;

    public SpriteRenderer[] backGrounds;
    public TMP_Text[] judgeMents;
    public TMP_Text hitScore;
    public TMP_Text rate;
    
    void Start()
    {
        // 임의 변수 배정
        // for (int i = 0; i < 12; i++)
        // {
        //     PlayData.HitScore[i] = Random.Range(0, 100);
        //     PlayData.totalNote += PlayData.HitScore[i];
        // }
        VideoStart();
        total100 = PlayData.HitScore[11];
        total90 = PlayData.totalNote - PlayData.HitScore[0];
        judgeMents[4].text = total100.ToString();
        judgeMents[5].text = (total90-total100).ToString();
        judgeMents[6].text = PlayData.HitScore[0].ToString();
        judgeMents[7].text = PlayData.bestCombo.ToString();
        hitScore.text =
            $"<line-height=120%>\n<color=#FFFF00><align=right>{PlayData.HitScore[11].ToString()}\n" +
            $"<color=#FFEA00><align=right>{PlayData.HitScore[10].ToString()}\n<color=#FFDD00><align=right>{PlayData.HitScore[9].ToString()}\n" +
            $"<color=#FFC100><align=right>{PlayData.HitScore[8].ToString()}\n<color=#FF9F00><align=right>{PlayData.HitScore[7].ToString()}\n" +
            $"<color=#FF9F99><align=right>{PlayData.HitScore[6].ToString()}\n<color=#FF99A9><align=right>{PlayData.HitScore[5].ToString()}\n" +
            $"<color=#F36495><align=right>{PlayData.HitScore[4].ToString()}\n<color=#FB5ED9><align=right>{PlayData.HitScore[3].ToString()}\n" +
            $"<color=#FA5DE7><align=right>{PlayData.HitScore[2].ToString()}\n<color=#F45DFA><align=right>{PlayData.HitScore[1].ToString()}\n" +
            $"<line-height=160%><color=#FF0000>{PlayData.HitScore[0].ToString()}\n<color=#FF8E81>{PlayData.rate.ToString("N2")}%\n" +
            $"<color=#FFFF00>{PlayData.bestCombo.ToString()}";
    }

   
    void Update()
    {
        SliderControll();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (detailBoxAnim.GetBool("IN_OUT"))
            {
                detailBoxAnim.SetBool("IN_OUT", false);
            }
            else
            {
                detailBoxAnim.SetBool("IN_OUT", true);
            }
            if (hitScoreAnim.GetBool("IN_OUT"))
            {
                hitScoreAnim.SetBool("IN_OUT", false);
            }
            else
            {
                hitScoreAnim.SetBool("IN_OUT", true);
            }
        }
        
    }

    void VideoStart()
    { 
        VideoClip videoClip = Resources.Load<VideoClip>("Video/" + PlayData.music);
        videoSource = GetComponent<VideoPlayer>();
        
        videoSource.clip = videoClip;
        videoSource.SetDirectAudioMute(0, true);
        videoSource.Play();
    }
    
    void SliderControll()
    {
        if (sliders[0].value < (float)total100 / PlayData.totalNote)
        {
            sliders[0].value += 0.001f;
        }

        if (sliders[1].value < (float) total90 / PlayData.totalNote)
        {
            sliders[1].value += 0.001f;
        }
        else
        {
            PrintRate();
            PrintJudgeMent();
        }
    }
    
    void PrintLog()
    {
        Debug.Log(PlayData.combo);
        Debug.Log(PlayData.rate);
        Debug.Log(PlayData.totalNote);
        for (int i = 0; i < 12; i++)
        {
            Debug.Log(PlayData.HitScore[i]);
        }
    }

    void PrintRate()
    {
        if (rateUp < PlayData.rate)
        {
            rateUp += 0.03f;
            rate.text = $"{rateUp.ToString("F2")}%";
        }
        else
        {
            rate.text = $"{PlayData.rate.ToString("F2")}%";
            rate.text.ToIntArray();
        }
    }

    private float[] val = new float[2];
    void PrintJudgeMent()
    {
        Color color;
        for (int i = 0; i < 8; i++)
        {
            color = judgeMents[i].color;
            if (val[0] <= 1.0f)
            {
                val[0] += 0.0002f;
                color.a = val[0];
                judgeMents[i].color = color;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            color = backGrounds[i].color;
            if (val[1] <= 0.4f)
            {
                val[1] += 0.00008f;
                color.a = val[1];
                backGrounds[i].color = color;
            }
        }
    }
    
}
