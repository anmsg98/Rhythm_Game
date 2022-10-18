using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    class Note
    {
        public int noteType { get; set; }
        public int order { get; set; }

        public Note(int noteType, int order)
        {
            this.noteType = noteType;
            this.order = order;
        }
        
    }

    public GameObject[] Notes;

    private ObjectPooler noteObjectPooler;
    private List<Note> notes = new List<Note>();
    private float x, z, startY = 30.0f;
    private float beatInterval = 1.0f;
    private int count = 1;
    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();
        for (int i = 1; i < 1000; i++)
        {
            if (count > 4) count = 1;
            notes.Add(new Note(count, i));
            count++;
        }
        
       
        
        for (int i = 0; i < notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }
    }

    void Update()
    {
        
    }

    IEnumerator AwaitMakeNote(Note note)
    {
        int noteTpye = note.noteType;
        int order = note.order;
        yield return new WaitForSeconds((order * beatInterval));
        MaKeNote(note);
    }

    void MaKeNote(Note note)
    {
        GameObject obj = noteObjectPooler.getObject(note.noteType);
        
        x = obj.transform.position.x;
        z = obj.transform.position.z;
        obj.transform.position = new Vector3(x, startY, z);
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.SetActive(true); 
    }
    
}
