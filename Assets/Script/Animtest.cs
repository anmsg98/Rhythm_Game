using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animtest : MonoBehaviour
{
    public Animator anim;
    private bool longnote=true;
    void Start()
    {
        
    }

    // Update is called once per frame
    IEnumerator Showanim()
    {
        yield return new WaitForSeconds(1.0f);
        longnote = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && longnote)
        {
            longnote = false;
            anim.SetTrigger("Space");
            StartCoroutine("Showanim");
        }
        
    }
}
