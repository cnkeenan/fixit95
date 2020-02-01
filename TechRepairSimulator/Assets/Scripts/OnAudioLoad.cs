using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAudioLoad : MonoBehaviour
{
    public AudioClip miniGameMusic;
    void Awake()
    {
        Narrate.NarrationManager.instance.GetComponent<AudioSource>().clip = miniGameMusic;
        Narrate.NarrationManager.instance.GetComponent<AudioSource>().Play();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
