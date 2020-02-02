using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narrate;
using UnityEngine.UI;

public class AnswerCall : MonoBehaviour
{
    public AudioClip callStarted;
    Narration narration = new Narration();
    public bool answered = false;
    void Start()
    {
    }

    void OnMouseDown()
    {
        if (!answered)
        {
            answered = true;
            AudioSource audiosource = NarrationManager.instance.GetComponent<AudioSource>();
            audiosource.clip = callStarted;
            GameObject.Find("Indicator").GetComponent<Animator>().SetBool("Incoming", false);
            GameObject.Find("Indicator").GetComponent<Animator>().SetBool("Hold", true);
            NarrationManager.instance.PlayNarration(narration);
            audiosource.Play();
            AudioSource.PlayClipAtPoint(callStarted, transform.position);
            GameObject source = GameObject.Find("One shot audio");
            source.GetComponent<AudioSource>().loop = true;
        }
    }

    public void CreateNarration(string[] scenarioPhrases, AudioClip[] clips)
    {
        List<Phrase> phrases = new List<Phrase>();

        for (int i = 0; i < scenarioPhrases.Length; i++)
        {
            Phrase phrase = new Phrase();
            phrase.text = scenarioPhrases[i];
            phrase.audio = clips[i];
            phrases.Add(phrase);
        }
        Phrase endPhrase = new Phrase();
        endPhrase.text = "**PRESS THE HOLD BUTTON TO PROCEED**";
        phrases.Add(endPhrase);

        narration.phrases = phrases.ToArray();
    }
}
