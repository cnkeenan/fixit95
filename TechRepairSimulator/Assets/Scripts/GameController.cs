using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject _call;

    public float upperBound;
    public static bool wasCreated;
    public int completedCalls = 0;
    public int failedCalls = 0;

    public GameObject callIndicator;
    public GameObject callPad;
    public Sprite previousNameplate;
    public Sprite[] faceplates;
    public Text uiText;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        wasCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (callIndicator.GetComponent<IndicatorScript>().callFailed > 0)
        {
            failedCalls++;
            callIndicator.GetComponent<IndicatorScript>().callFailed--;
        }

        if (callPad.GetComponent<AnswerCall>().answered)
        {
            callIndicator.GetComponent<IndicatorScript>().onHold = true;
            if (GameObject.FindGameObjectWithTag("DialogBox").active)
            {
                callIndicator.GetComponent<TransitionToMinigame>().locked = true;
            }
            else
            {
                callIndicator.GetComponent<TransitionToMinigame>().locked = false;
            }
        }

        uiText.text = $"Rating: {completedCalls} of {completedCalls + failedCalls}";
    }
}
