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
        }
    }

    public void CreateNarration(string[] scenarioPhrases)
    {
        List<Phrase> phrases = new List<Phrase>();

        for (int i = 0; i < scenarioPhrases.Length; i++)
        {
            Phrase phrase = new Phrase();
            phrase.text = scenarioPhrases[i];
            phrases.Add(phrase);
        }
        Phrase endPhrase = new Phrase();
        endPhrase.text = "**PRESS THE HOLD BUTTON TO PROCEED**";
        phrases.Add(endPhrase);

        narration.phrases = phrases.ToArray();
    }
}
