using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class Result : MonoBehaviour
{
    private VideoPlayer videoSource;
    public Slider[] sliders;
    
    public Animator detailBoxAnim;
    public Animator hitScoreAnim;
   
    private float value;
    private int total100;
    private int totalUnder90;

    public TMP_Text hitScore;
    void Start()
    {
        hitScore.text =
            $"<line-height=120%>\n<color=#FFFF00><align=right>{PlayData.HitScore[11].ToString()}\n" +
            $"<color=#FFEA00><align=right>{PlayData.HitScore[10].ToString()}\n<color=#FFDD00><align=right>{PlayData.HitScore[9].ToString()}\n" +
            $"<color=#FFC100><align=right>{PlayData.HitScore[8].ToString()}\n<color=#FF9F00><align=right>{PlayData.HitScore[7].ToString()}\n" +
            $"<color=#FF9F99><align=right>{PlayData.HitScore[6].ToString()}\n<color=#FF99A9><align=right>{PlayData.HitScore[5].ToString()}\n" +
            $"<color=#F36495><align=right>{PlayData.HitScore[4].ToString()}\n<color=#FB5ED9><align=right>{PlayData.HitScore[3].ToString()}\n" +
            $"<color=#FA5DE7><align=right>{PlayData.HitScore[2].ToString()}\n<color=#F45DFA><align=right>{PlayData.HitScore[1].ToString()}\n" +
            $"<line-height=160%><color=#FF0000>{PlayData.HitScore[0].ToString()}\n<color=#FF8E81>{PlayData.rate.ToString("N2")}%\n0";
    }

   
    void Update()
    {
        // if (sliders[0].value < (float)(total100 / PlayData.totalNote))
        // {
        //     sliders[0].value += 0.001f;
        // }
        // else
        // {
        //     sliders[0].value = (float)(total100 / PlayData.totalNote);
        // }
        //
        // if (totalUnder90 > 0)
        // {
        //     if (sliders[1].value < (float) (totalUnder90 / PlayData.totalNote))
        //     {
        //         sliders[1].value += 0.001f;
        //     }
        //     else
        //     {
        //         sliders[1].value = (float) (totalUnder90 / PlayData.totalNote);
        //     }
        // }

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
    
}
