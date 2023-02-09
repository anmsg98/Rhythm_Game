using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> Notes;
    private List<List<GameObject>> poolsOfNotes;
    public int noteCount = 10;
    private bool more = true;
    
    void Start()
    {
        poolsOfNotes = new List<List<GameObject>>();
        for (int i = 0; i < Notes.Count; i++)
        {
            poolsOfNotes.Add(new List<GameObject>());
            for (int j = 0; j < noteCount; j++)
            {
                GameObject obj = Instantiate(Notes[i]);
                obj.SetActive(false);
                
                poolsOfNotes[i].Add(obj);
            }
        }
    }

    public GameObject getObject(int noteType)
    {
        for (int i = 0; i < noteCount; i++) 
        {
            if (!poolsOfNotes[noteType - 1][i].activeInHierarchy)
            {
                return poolsOfNotes[noteType - 1][i];
            }
        }

        if (more)
        {
            GameObject obj = Instantiate(Notes[noteType - 1]);
            poolsOfNotes[noteType - 1].Add(obj);
            return obj;
        }

        return null;
    }

    
    
    void Update()
    {
        // 연타나 빠른 비트의 노트가 같은라인에 내려올때 후행 노트가 판정이 같이 처리되는 문제점 해결
        List<List<int>> arr = new List<List<int>>();
        
        for (int i = 0; i < Notes.Count; i++)
        {
            arr.Add(new List<int>()); 
            for (int j = 0; j < noteCount; j++)
            {
                if (!poolsOfNotes[i][j].activeInHierarchy)
                    arr[i].Add(9999);
                else
                {
                    arr[i].Add((int)poolsOfNotes[i][j].GetComponent<NoteBehavior>().notePrior);
                }
            }
        }

        for (int i = 0; i < Notes.Count; i++)
        {
            int n = arr[i].Min();
            poolsOfNotes[i][arr[i].IndexOf(n)].GetComponent<NoteBehavior>().noteJudge = true;
        }

    }
}
 
