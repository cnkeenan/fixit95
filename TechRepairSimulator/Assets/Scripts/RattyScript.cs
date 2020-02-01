using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RattyScript : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(30);
        if (anim.GetBool("Sleep"))
        {
            anim.SetBool("Sleep", false);
        }
        else
        {
            anim.SetBool("Sleep", true);
        }
    }
}
