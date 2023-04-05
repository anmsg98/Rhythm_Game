using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGradient : MonoBehaviour
{
    public Material gradientMat;
    public Color leftCol;
    public Color rightCol;
    
    void Start()
    {
        gradientMat.SetColor("_Color", leftCol);
        gradientMat.SetColor("_Color2", rightCol);
        StartCoroutine(ChangeColor());
    }

    void Update()
    {
        gradientMat.SetColor("_Color", leftCol);
        gradientMat.SetColor("_Color2", rightCol);
        
    }

    IEnumerator ChangeColor()
    {
        while (leftCol.r <= 1.0f)
        {
            leftCol.r += 0.01f; leftCol.g += 0.00815f; leftCol.b += 0.00047f;
            rightCol.r +=0.01f; rightCol.g += 0.00517f; rightCol.b += 0.00549f;
            gradientMat.SetColor("_Color", leftCol);
            gradientMat.SetColor("_Color2", rightCol);
            yield return new WaitForSeconds(0.01f);
        }
        
        
    }

}
