using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class MusicSelect : MonoBehaviour
{
    private List<string> musicList;
    public GameObject Music;
    void Start()
    {
        musicList = new List<string>();
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/Music List");
        StringReader reader = new StringReader(textAsset.text);
        string line;
        int index = 0;
        while ((line = reader.ReadLine()) != null)
        {
            Vector3 transform = new Vector3(0, 0, 0);
            
            
            string title = line;
            musicList.Add(title);
            GameObject instant = Instantiate(Music);
            instant.transform.position += new Vector3(0, -index, 0);
            instant.name = index.ToString();
            
            SpriteRenderer thumbnail = instant.transform.Find("Thumbnail").gameObject.GetComponent<SpriteRenderer>();
            thumbnail.sprite = Resources.Load<Sprite>("Sprites/" + title + "_T");


            TMP_Text text = instant.transform.Find("Title").transform.Find("Music Title").transform.Find("Text").gameObject.GetComponent<TMP_Text>();
            text.text = title;
            

            index += 1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
