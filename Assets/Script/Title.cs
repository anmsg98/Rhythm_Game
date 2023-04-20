using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Title : MonoBehaviour
{
    public SpriteRenderer fadeIn;
    
    public AudioSource mainBgm;
    public AudioSource effectSound;
    private bool enableFadeIn;
    private Color fadeInColor;

    private void Awake()
    {
        fadeInColor = fadeIn.color;
    }

    void Start()
    {
        
    }
    
    private void FadeOut()
    {
        if (!enableFadeIn)
        {
            if (fadeIn.color.a > 0.0f)
            {
                fadeInColor.a -= Time.deltaTime;
                fadeIn.color = fadeInColor;
            }
        }
    }

    private void FadeIn()
    {
        if (enableFadeIn)
        {
            mainBgm.volume -= Time.deltaTime * 0.5f;
            
            if (fadeIn.color.a < 1.0f)
            {
                fadeInColor.a += Time.deltaTime;
                fadeIn.color = fadeInColor;
            }

            else
            {
                Invoke("SceneChange", 1.3f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        FadeOut();
        FadeIn();
        if (Input.anyKeyDown)
        {
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/Enter");
            effectSound.clip = audioClip;
            effectSound.Play();
            enableFadeIn = true;
        }
    }

    void SceneChange()
    {
        SceneManager.LoadScene("SelectScene");
    }
}
