using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public static NoteBehavior instance { get; set; }
    
    // Break 판정선
    private GameObject missJudge;
    
    // 노트 타입, 노트 처리 순서, 노트 판정 가능 여부
    public int noteType;
    public float notePrior = 0f;
    public float noteTiming;
    public bool noteJudge = false;
    
    // 판정구역, 판정 세부조정
    private float judgeSection;
    private float detailedJudgment;
    
    // 판정(1~100%), 키입력
    public GameManager.judges judge;
    private KeyCode keyCode;

  
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        missJudge = GameObject.Find("Miss JudgeLine");
        detailedJudgment = GameManager.instance.judgeTime * 0.001f * 44100f;
        Debug.Log(detailedJudgment);
        if (noteType == 1) keyCode = KeyCode.D;
        if (noteType == 2) keyCode = KeyCode.F;
        if (noteType == 3) keyCode = KeyCode.L;
        if (noteType == 4) keyCode = KeyCode.Semicolon;
    }

    // Update is called once per frame
    void Update()
    {
        judgeSection = Mathf.Abs(noteTiming - Convert.ToSingle(GameManager.instance.audioSource.timeSamples));
        transform.Translate(Vector3.down * GameManager.instance.noteSpeed * Time.deltaTime);
        CheckJudgeMent();
        KeyInput();
    }

    void KeyInput()
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (judge != GameManager.judges.NONE && noteJudge)
            {
                GameManager.instance.ProcessJudge(judge);
                Debug.Log(judgeSection / 44.1f);
                noteJudge = false;
                gameObject.SetActive(false);
            }
        }
    }
    
    void CheckJudgeMent()
    {
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
        else if (transform.position.y <= missJudge.transform.position.y)
        {
            judge = GameManager.judges.Break;
            GameManager.instance.ProcessJudge(judge);
            noteJudge = false;
            gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        judge = GameManager.judges.NONE;
    }
}
