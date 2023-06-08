using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class NoteController : MonoBehaviour
{
    class Note
    {
        public int noteType { get; set; }

        public int longNote { get; set; }
        public float order { get; set; }
        public float noteTiming { get; set; }
        
        public Note(int noteType, int longNote, float order, float noteTiming)
        {
            this.noteType = noteType;
            this.longNote = longNote;
            this.order = order;
            this.noteTiming = noteTiming;
        }
    }

    public GameObject[] Notes;
    private WaitForSeconds _time;
    private ObjectPooler noteObjectPooler;
    private List<Note> notes = new List<Note>();
    private float x, z;
    public float startY;
    
    private string Title;
    private string Artist;
    private int bpm;
    private int divider;
    private float startingPoint;
    private float beatCount;
    private float beatInterval;

    private List<float> orderList = new List<float>();
    

    void Start()
    {
        List<int> lists = new List<int>();
        lists.Add(1); lists.Add(2); lists.Add(3); lists.Add(4);
        RandomNote(lists);
        
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();
        startY = MusicSelect.instance.noteSpeed * 2.6f - 1.675f;
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + PlayData.music);
        StringReader reader = new StringReader(textAsset.text);
        // 곡제목
        Title = reader.ReadLine();
        // 아티스트 정보
        PlayData.Artist = reader.ReadLine();
        // bpm 정보
        string beatInformation = reader.ReadLine();
        bpm = Convert.ToInt32(beatInformation.Split(' ')[0]);
        divider = Convert.ToInt32(beatInformation.Split(' ')[1]);
        startingPoint = (float) Convert.ToDouble(beatInformation.Split(' ')[2]);

        beatCount = (float) bpm / divider;
        beatInterval = 0.125f / beatCount;

        string line;
        int notetype;
        int longNote;
        float order;
        float noteTiming;

        while ((line = reader.ReadLine()) != null)
        {
            notetype = Convert.ToInt32(line.Split(' ')[0]);
            
            // 미러 노트
            if (MusicSelect.instance.chaos == 1)
            {
                if (notetype == 1) notetype = 4;
                else if (notetype == 2) notetype = 3;
                else if (notetype == 3) notetype = 2;
                else if (notetype == 4) notetype = 1;
            }
            
            // 랜덤 노트
            else if (MusicSelect.instance.chaos == 2)
            {
                if (notetype == 1) notetype = lists[0];
                else if (notetype == 2) notetype = lists[1]; 
                else if (notetype == 3) notetype = lists[2];
                else if (notetype == 4) notetype = lists[3];
            }

            longNote = Convert.ToInt32(line.Split(' ')[1]);
            if (longNote > 0)
            {
                notetype += 4;
            }
            order = Convert.ToSingle(line.Split(' ')[2]);
            noteTiming = (8423.1f - (MusicSelect.instance.syncTime - 2.4f) * 44100f) + (Convert.ToInt32(order) * beatInterval * 44100f);
            Note note = new Note(notetype, longNote, order, noteTiming);
            orderList.Add(order);
            notes.Add(note);
            
        }
        // 노트의 출력 시간 설정
        for (int i = 0; i < notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }
        // 게임 종료 (마지막 노트 출력)
        StartCoroutine(AwaitGameResult(notes[notes.Count - 1].order));
    }
    
    void Update()
    {
        
    }

    IEnumerator AwaitMakeNote(Note note)
    {
        int noteTpye = note.noteType;
        float order = note.order;
        _time = new WaitForSeconds(startingPoint + order * beatInterval);

        yield return _time;
        
        MaKeNote(note);
    }

    IEnumerator AwaitGameResult(float order)
    {
        _time = new WaitForSeconds(startingPoint + order * beatInterval + 8.0f);

        yield return _time;
        GameManager.instance.enableFadeIn = true;
    }

    private int cnt = 0;
    void MaKeNote(Note note)
    {
        GameObject obj = noteObjectPooler.getObject(note.noteType);

        if (note.noteType > 4)
        {
            x = obj.transform.GetChild(0).gameObject.transform.position.x;
            z = obj.transform.GetChild(0).gameObject.transform.position.z;
            
            obj.transform.GetChild(0).gameObject.transform.position = new Vector3(x, startY + (112.5f / bpm * note.longNote), z);
            obj.transform.GetChild(1).gameObject.transform.position = new Vector3(x, startY, z);
            obj.transform.GetChild(2).gameObject.transform.position = new Vector3(x, startY + (112.5f / bpm * note.longNote) / 2, z);
            obj.transform.GetChild(2).gameObject.transform.localScale = new Vector3(1.24f, (112.5f / bpm * note.longNote), 1);
        }

        else
        {
            x = obj.transform.position.x;
            z = obj.transform.position.z;

            obj.transform.position = new Vector3(x, startY, z);
        }
        
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.GetComponent<NoteBehavior>().notePrior = orderList[cnt];
        obj.GetComponent<NoteBehavior>().noteTiming = note.noteTiming;
        obj.SetActive(true);
        cnt += 1;
    }
    
    private List<T> RandomNote<T>(List<T> list)
    {
        int random1,  random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }


}
