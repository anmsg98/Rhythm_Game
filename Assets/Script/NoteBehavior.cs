using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{

    private GameObject missJudge;
    
    public int noteType;
    
    private float judgeSection;
    private float detailedJudgment;
    
    public GameManager.judges judge;
    private KeyCode keyCode;

    void Start()
    {
        missJudge = GameObject.Find("Miss JudgeLine");
        detailedJudgment = GameManager.instance.judgeTime * GameManager.instance.noteSpeed * 0.001f;
        
        if (noteType == 1) keyCode = KeyCode.S;
        if (noteType == 2) keyCode = KeyCode.D;
        if (noteType == 3) keyCode = KeyCode.L;
        if (noteType == 4) keyCode = KeyCode.Semicolon;
    }

    // Update is called once per frame
    void Update()
    {
        judgeSection = Mathf.Abs(transform.position.y + 4.1f); 
        detailedJudgment = GameManager.instance.judgeTime * GameManager.instance.noteSpeed * 0.001f;
        transform.Translate(Vector3.down * GameManager.instance.noteSpeed * Time.deltaTime);
        CheckJudgeMent();
        KeyInput();

    }

    void KeyInput()
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (judge != GameManager.judges.NONE)
            {
                GameManager.instance.ProcessJudge(judge);
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
            gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        judge = GameManager.judges.NONE;
    }
}
