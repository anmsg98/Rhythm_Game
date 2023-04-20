using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warning : MonoBehaviour
{
    public SpriteRenderer fadeIn;
    private Color fadeInColor;
    private bool enableFadeIn;
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
            if (fadeIn.color.a < 1.0f)
            {
                fadeInColor.a += Time.deltaTime;
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
