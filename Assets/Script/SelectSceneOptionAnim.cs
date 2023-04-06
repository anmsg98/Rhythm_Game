using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSceneOptionAnim : MonoBehaviour
{
    void OptionBlurOn()
    {
        MusicSelect.instance.MenuUi.SetActive(false);
        MusicSelect.instance.optionBlur.SetActive(true);
    }
    
    void OptionBlurOff()
    {
        MusicSelect.instance.MenuUi.SetActive(true);
        MusicSelect.instance.optionBlur.SetActive(false);
    }
}
