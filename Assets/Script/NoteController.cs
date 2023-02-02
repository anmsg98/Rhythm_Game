using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;

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
    private WaitForSeconds _time;
    private ObjectPooler noteObjectPooler;
    private List<Note> notes = new List<Note>();
    private float x, z, startY = 30.0f;
    private int count = 1;
    private Random randnum = new Random();
    
    private string Title;
    private string Artist;
    private int bpm;
    private int divider;
    private float startingPoint;
    private float beatCount;
    private float beatInterval;
    
    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();

        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + GameManager.instance.music);
        StringReader reader = new StringReader(textAsset.text);
        // 곡제목
        Title = reader.ReadLine();
        // 아티스트 정보
        Artist = reader.ReadLine();
        // bpm 정보
        string beatInformation = reader.ReadLine();
        bpm = Convert.ToInt32(beatInformation.Split(' ')[0]);
        divider = Convert.ToInt32(beatInformation.Split(' ')[1]);
        startingPoint = (float) Convert.ToDouble(beatInformation.Split(' ')[2]);

        beatCount = (float) bpm / divider;
        beatInterval = 0.125f / beatCount;

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            Note note = new Note(
                Convert.ToInt32(line.Split(' ')[0]),
                Convert.ToInt32(line.Split(' ')[1])
                );
            notes.Add(note);
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
        _time = new WaitForSeconds(startingPoint + order * beatInterval);

        yield return _time;
        
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
