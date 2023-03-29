using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UIElements;

public class JudgeMentGuide : MonoBehaviour
{
    void Start()
    {
        transform.Translate(Vector3.down * GameManager.instance.judgeTime * MusicSelect.instance.noteSpeed * 0.001f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.Translate(Vector3.up * GameManager.instance.judgeTime * 0.001f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.Translate(Vector3.down * GameManager.instance.judgeTime * 0.001f);
        }
    }
}
