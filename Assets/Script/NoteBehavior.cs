using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{

    public GameObject missJudge;
    
    public int noteType;
    private float noteSpeed;
    
    private float judgeSection;
    private float detailedJudgment;
    
    private GameManager.judges judge;
    private KeyCode keyCode;

    void Start()
    {
        noteSpeed = GameManager.instance.noteSpeed;
        detailedJudgment = GameManager.instance.judgeTime * noteSpeed * 0.001f;
        
        if (noteType == 1) keyCode = KeyCode.S;
        if (noteType == 2) keyCode = KeyCode.D;
        if (noteType == 3) keyCode = KeyCode.L;
        if (noteType == 4) keyCode = KeyCode.Semicolon;
    }

    // Update is called once per frame
    void Update()
    {
        judgeSection = Mathf.Abs(transform.position.y + 4.1f); 
        transform.Translate(Vector3.down * noteSpeed * Time.deltaTime);
        CheckJudgeMent();
        KeyInput();

    }

    void KeyInput()
    {
        if (Input.GetKeyDown(keyCode))
        {
            Debug.Log(judge);
            if (judge != GameManager.judges.NONE)
            {
                transform.position = new Vector3(-6f,10f,0f);
                judge = GameManager.judges.NONE;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            noteSpeed -= 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            noteSpeed += 1f;
        }
    }
    
    void CheckJudgeMent()
    {
        if (judgeSection <= detailedJudgment * 3f)
        {
            if (judgeSection >= detailedJudgment * 2f && judgeSection < detailedJudgment * 3f)
            {
                judge = GameManager.judges.BAD;
            }
            else if (judgeSection >= detailedJudgment && judgeSection < detailedJudgment * 2f)
            {
                judge = GameManager.judges.GOOD;
            }
            else if (judgeSection < detailedJudgment)
            {
                judge = GameManager.judges.PERFECT;
            }
        }
        else if (transform.position.y <= missJudge.transform.position.y)
        {
            judge = GameManager.judges.MISS;
            Debug.Log(judge);
            judge = GameManager.judges.NONE;
            transform.position = new Vector3(-6f,10f,0f);
        }
    }
    
}
