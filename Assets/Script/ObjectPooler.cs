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
    public int noteCount;
    private bool more = true;
    
    void Start()
    {
        poolsOfNotes = new List<List<GameObject>>();
        for (int i = 0; i < Notes.Count; i++)
        {
            poolsOfNotes.Add(new List<GameObject>());
            for (int j = 0; j < noteCount; j++)
            {
                GameObject obj;
                
                obj = Instantiate(Notes[i],
                        new Vector3(Notes[i].transform.position.x + (MusicSelect.instance.gearPosition * 6f), Notes[i].transform.position.y ,Notes[i].transform.position.z),
                        Quaternion.identity);
               
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
            GameObject obj;
            
            obj = Instantiate(Notes[noteType - 1],
                new Vector3(Notes[noteType - 1].transform.position.x + (MusicSelect.instance.gearPosition * 5.77f), 
                    Notes[noteType - 1].transform.position.y ,Notes[noteType - 1].transform.position.z),
                Quaternion.identity);
            
            poolsOfNotes[noteType - 1].Add(obj);
            return obj;
        }

        return null;
    }

    
    
    void Update()
    {
        CheckNotePrior();
    }
    public void CheckNotePrior()
    {
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
 
