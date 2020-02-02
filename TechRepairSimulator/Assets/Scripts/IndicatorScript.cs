using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public Animator anim;
    public bool callOngoing;
    public bool onHold;
    float callWaitTime = 10.0f;
    float timer = 0.0f;
    public int callFailed = 0;

    public AudioClip tone;
    public AudioClip missedCall;


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
        else if (onHold)
        {

        }
        else
        {
            StartCoroutine(SleepWait());
        }
    }

    IEnumerator SleepWait()
    {
        yield return new WaitForSeconds(5);
        timer += Time.deltaTime;
        if (timer > callWaitTime)
        {
            AudioSource.PlayClipAtPoint(missedCall, transform.position);
            timer = 0.0f;
            callOngoing = false;
            anim.SetBool("Incoming", false);
            callFailed++;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(Random.Range(1, 9));
        anim.SetBool("Incoming", true);
        AudioSource.PlayClipAtPoint(tone, transform.position);
    }
}
