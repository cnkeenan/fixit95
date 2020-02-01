using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public Animator anim;
    static bool callOngoing;
    static bool onHold;

    public AudioClip tone;

    void Awake()
    {
        callOngoing = false;
        onHold = false;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!callOngoing)
        {
            StartCoroutine(Wait());
            callOngoing = true;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(9);
        anim.SetBool("Incoming", true);
        AudioSource.PlayClipAtPoint(tone, transform.position);
    }
}
