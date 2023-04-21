using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warning : MonoBehaviour
{
    public SpriteRenderer fadeIn;
    private Color fadeInColor;
    private bool enableFadeIn;
    private float timeCount;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        fadeInColor = fadeIn.color;
        Application.targetFrameRate = 400;
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
            if (fadeIn.color.a < 1.0f)
            {
                fadeInColor.a += Time.deltaTime * 1.2f;
                fadeIn.color = fadeInColor;
            }

            else
            {
                Invoke("SceneChange", 0.5f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        FadeOut();
        FadeIn();
        
        timeCount += Time.deltaTime;
        
        if (timeCount >= 7.0f)
        {
            enableFadeIn = true;
        }
        
        if (Input.anyKeyDown)
        {
            enableFadeIn = true;
        }
    }

    void SceneChange()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
