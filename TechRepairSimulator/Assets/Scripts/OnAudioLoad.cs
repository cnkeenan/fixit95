using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAudioLoad : MonoBehaviour
{
    public AudioClip miniGameMusic;
    void Awake()
    {
        AudioSource AudioSource = Narrate.NarrationManager.instance.GetComponent<AudioSource>();
        AudioSource.clip = miniGameMusic;
        AudioSource.volume = 0.3f;
        AudioSource.Play();
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
