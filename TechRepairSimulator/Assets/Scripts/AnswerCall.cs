using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerCall : MonoBehaviour
{
    public AudioClip callStarted;
    Narrate.Narration narration = new Narrate.Narration();

    void Start() {
        CreateNarration();
    }

    void OnMouseDown() {
        Debug.Log("Call Answered - Scene Transition Needed");
        AudioSource.PlayClipAtPoint(callStarted, transform.position);
        Narrate.NarrationManager.instance.PlayNarration(narration);
        gameObject.SetActive(false);
    }

    void CreateNarration() {
        var phrase = new Narrate.Phrase();
        Narrate.Phrase[] phrases= new Narrate.Phrase[1];
        phrase.text = "Hello";
        phrases[0] = phrase;
        narration.phrases = phrases;
    }
}
