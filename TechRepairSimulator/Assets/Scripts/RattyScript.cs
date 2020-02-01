using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattyScript : MonoBehaviour
{
    Animator anim;
    private Coroutine wait;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wait == null)
        {
           wait = StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(30);
        if (anim.GetBool("Sleep"))
        {
            anim.SetBool("Sleep", false);
            wait = null;
            yield break;
        }
        else
        {
            anim.SetBool("Sleep", true);
            wait = null;
            yield break;
        }

       
    }
}
