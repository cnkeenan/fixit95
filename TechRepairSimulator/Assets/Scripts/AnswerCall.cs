using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Narrate;

public class AnswerCall : MonoBehaviour
{
    public AudioClip callStarted;
    Narration narration = new Narration();
    JsonDialog scenarios = new JsonDialog();
    public bool answered = false;
    void Start()
    {
        var jsonPath = Resources.Load<TextAsset>("Dialog/dialog");
        scenarios = JsonUtility.FromJson<JsonDialog>(jsonPath.text);
        CreateNarration();
    }

    void OnMouseDown()
    {
        if (!answered)
        {
            answered = true;
            Debug.Log("Call Answered - Scene Transition Needed");
            AudioSource audiosource = NarrationManager.instance.GetComponent<AudioSource>();
            audiosource.clip = callStarted;
            GameObject.Find("Indicator").GetComponent<Animator>().SetBool("Incoming", false);
            GameObject.Find("Indicator").GetComponent<Animator>().SetBool("Hold", true);
            NarrationManager.instance.PlayNarration(narration);
            audiosource.Play();
        }
    }

    void CreateNarration()
    {
        List<Phrase> phrases = new List<Phrase>();

        for (int i = 0; i < scenarios.Scenarios.Length; i++)
        {
            Phrase phrase = new Phrase();
            if (scenarios.Scenarios[i].Contains("**PRESS"))
            {
                phrase.text = scenarios.Scenarios[i];
                phrases.Add(phrase);
                break;
            }
            phrase.text = scenarios.Scenarios[i];
            phrases.Add(phrase);
        }

        narration.phrases = phrases.ToArray();
    }
}
