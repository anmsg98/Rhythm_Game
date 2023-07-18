using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public static NoteBehavior instance { get; set; }
    
    // Break 판정선
    private GameObject missJudge;
    
    // 노트 타입, 노트 처리 순서, 노트 판정 가능 여부
    public int noteType;
    public float notePrior = 0f;
    public int longNoteOrder;
    public bool noteJudge = false;
    private bool longPress = true;
    private bool enableBreak = true;
    private bool longClick = false;

    // 판정구역, 판정 세부조정
    public float beatInterval;
    public float noteTiming;
    private float longNoteTiming;
    private float judgeSection;
    private float detailedJudgment;
    
    
    // 판정(1~100%), 키입력
    public GameManager.judges judge;
    public GameManager.judges longJudge;
    private GameManager.judges finalLongJudge;
    private KeyCode keyCode;

  
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        missJudge = GameObject.Find("Miss JudgeLine");
        
        detailedJudgment = GameManager.instance.judgeTime * 0.001f * 44100f;
        
        if (noteType == 1 || noteType == 5) keyCode = KeyCode.D;
        if (noteType == 2 || noteType == 6)  keyCode = KeyCode.F;
        if (noteType == 3 || noteType == 7)  keyCode = KeyCode.L;
        if (noteType == 4 || noteType == 8)  keyCode = KeyCode.Semicolon;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.pause.activeInHierarchy)
        {
            judgeSection = Mathf.Abs(noteTiming - Convert.ToSingle(GameManager.instance.audioSource.timeSamples));
            longNoteTiming = Mathf.Abs((noteTiming + (longNoteOrder * beatInterval * 44100f)) - Convert.ToSingle(GameManager.instance.audioSource.timeSamples));
            transform.Translate(Vector3.down * MusicSelect.instance.noteSpeed * Time.deltaTime);
            if (noteType < 5) CheckJudgeMent();
            else CheckLongJudgeMent();
        }
       
    }

    IEnumerator LongNote()
    {
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.feverGauge += 0.5f;
        GameManager.instance.combo += GameManager.instance.feverCount;
        GameManager.instance.comboText.text = $"<size=100%>combo\n<size=200%>{GameManager.instance.combo.ToString()}";
        longPress = true;
        KeyBomb();
    }

    void CheckJudgeMent()
    {
        if (GameManager.instance.judgeStart && noteJudge)
        {
            if (Input.GetKeyDown(keyCode))
            {
                if (judge != GameManager.judges.NONE && noteJudge)
                {
                    GameManager.instance.ProcessJudge(judge);
                    GameManager.instance.ShowJudgementAnim();
                    KeyBomb();
                    noteJudge = false;
                    gameObject.SetActive(false);
                }
            }
        }

        if (judgeSection <= detailedJudgment * 3f)
        {
            if (judgeSection >= detailedJudgment * 2.8f && judgeSection < detailedJudgment * 3f)
            {
                judge = GameManager.judges.MAX1;
            }
            else if (judgeSection >= detailedJudgment * 2.6f && judgeSection < detailedJudgment * 2.8f)
            {
                judge = GameManager.judges.MAX10;
            }
            else if (judgeSection >= detailedJudgment * 2.4f && judgeSection < detailedJudgment * 2.6f)
            {
                judge = GameManager.judges.MAX20;
            }
            else if (judgeSection >= detailedJudgment * 2.2f && judgeSection < detailedJudgment * 2.4f)
            {
                judge = GameManager.judges.MAX30;
            }
            else if (judgeSection >= detailedJudgment * 2.0f && judgeSection < detailedJudgment * 2.2f)
            {
                judge = GameManager.judges.MAX40;
            }
            else if (judgeSection >= detailedJudgment * 1.8f && judgeSection < detailedJudgment * 2.0f)
            {
                judge = GameManager.judges.MAX50;
            }
            else if (judgeSection >= detailedJudgment * 1.6f && judgeSection < detailedJudgment * 1.8f)
            {
                judge = GameManager.judges.MAX60;
            }
            else if (judgeSection >= detailedJudgment * 1.4f && judgeSection < detailedJudgment * 1.6f)
            {
                judge = GameManager.judges.MAX70;
            }
            else if (judgeSection >= detailedJudgment * 1.2f && judgeSection < detailedJudgment * 1.4f)
            {
                judge = GameManager.judges.MAX80;
            }
            else if (judgeSection >= detailedJudgment * 1.0f && judgeSection < detailedJudgment * 1.2f)
            {
                judge = GameManager.judges.MAX90;
            }
            else if (judgeSection < detailedJudgment)
            {
                judge = GameManager.judges.MAX100;
            }
        }

        else
        {
            if ((noteTiming < Convert.ToSingle(GameManager.instance.audioSource.timeSamples)))
            {
                judge = GameManager.judges.Break;
                GameManager.instance.ProcessJudge(judge);
                GameManager.instance.ShowJudgementAnim();
                noteJudge = false;
                gameObject.SetActive(false);
            }
        }
    }

    void CheckLongJudgeMent()
    {
        if (Input.GetKeyDown(keyCode))
                {
                    if (judge != GameManager.judges.NONE && noteJudge)
                    {
                        longJudge = judge;
                        GameManager.instance.ProcessJudge(longJudge);
                        enableBreak = false;
                        longClick = true;
                        KeyBomb();
                    }
                }
                else if (longJudge != GameManager.judges.NONE && Input.GetKey(keyCode) && longPress)
                {
                    if ((GameManager.instance.audioSource.timeSamples >
                         (noteTiming + (longNoteOrder * beatInterval * 44100f))) &&
                        longNoteTiming > detailedJudgment * 3f)
                    {
                        longJudge = GameManager.judges.MAX1;
                        GameManager.instance.ProcessJudge(longJudge);
                        GameManager.instance.ShowJudgementAnim();
                        gameObject.SetActive(false);
                    }

                    longPress = false;
                    GameManager.instance.ShowJudgementAnim();
                    StartCoroutine("LongNote");
                }

                else if (Input.GetKeyUp(keyCode) && noteJudge && longClick)
                {
                    if (longNoteTiming > detailedJudgment * 3f)
                    {
                        longJudge = GameManager.judges.Break;
                        GameManager.instance.ProcessJudge(longJudge);
                        GameManager.instance.ShowJudgementAnim();
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        GameManager.instance.ProcessJudge(longJudge);
                        GameManager.instance.ShowJudgementAnim();
                        gameObject.SetActive(false);
                    }

                    longClick = false;
                    noteJudge = false;
                }
        
        if (judgeSection <= detailedJudgment * 3f)
            {
                if (judgeSection >= detailedJudgment * 2.8f && judgeSection < detailedJudgment * 3f)
                {
                    judge = GameManager.judges.MAX1;
                }
                else if (judgeSection >= detailedJudgment * 2.6f && judgeSection < detailedJudgment * 2.8f)
                {
                    judge = GameManager.judges.MAX10;
                }
                else if (judgeSection >= detailedJudgment * 2.4f && judgeSection < detailedJudgment * 2.6f)
                {
                    judge = GameManager.judges.MAX20;
                }
                else if (judgeSection >= detailedJudgment * 2.2f && judgeSection < detailedJudgment * 2.4f)
                {
                    judge = GameManager.judges.MAX30;
                }
                else if (judgeSection >= detailedJudgment * 2.0f && judgeSection < detailedJudgment * 2.2f)
                {
                    judge = GameManager.judges.MAX40;
                }
                else if (judgeSection >= detailedJudgment * 1.8f && judgeSection < detailedJudgment * 2.0f)
                {
                    judge = GameManager.judges.MAX50;
                }
                else if (judgeSection >= detailedJudgment * 1.6f && judgeSection < detailedJudgment * 1.8f)
                {
                    judge = GameManager.judges.MAX60;
                }
                else if (judgeSection >= detailedJudgment * 1.4f && judgeSection < detailedJudgment * 1.6f)
                {
                    judge = GameManager.judges.MAX70;
                }
                else if (judgeSection >= detailedJudgment * 1.2f && judgeSection < detailedJudgment * 1.4f)
                {
                    judge = GameManager.judges.MAX80;
                }
                else if (judgeSection >= detailedJudgment * 1.0f && judgeSection < detailedJudgment * 1.2f)
                {
                    judge = GameManager.judges.MAX90;
                }
                else if (judgeSection < detailedJudgment)
                {
                    judge = GameManager.judges.MAX100;
                }
            }

        else
        {
            if ((noteTiming < Convert.ToSingle(GameManager.instance.audioSource.timeSamples)) && enableBreak)
            {
                judge = GameManager.judges.Break;
                GameManager.instance.ProcessJudge(judge);
                GameManager.instance.ShowJudgementAnim();
                noteJudge = false;
                gameObject.SetActive(false);
            }
        }
    }
    public void Initialize()
    {
        longPress = true;
        longClick = false;
        judge = GameManager.judges.NONE;
        longJudge = GameManager.judges.NONE;
        enableBreak = true;
        noteJudge = false;
        longNoteTiming = 0f;
        judgeSection = 0f;
    }

    void KeyBomb()
    {
        if (keyCode == KeyCode.D)
        {
            GameManager.instance.keyBomb[0].Play();
        }
        else if (keyCode == KeyCode.F)
        {
            GameManager.instance.keyBomb[1].Play();
        }
        else if (keyCode == KeyCode.L)
        {
            GameManager.instance.keyBomb[2].Play();
        }
        else if (keyCode == KeyCode.Semicolon)
        {
            GameManager.instance.keyBomb[3].Play();
        }
        
    }
}
