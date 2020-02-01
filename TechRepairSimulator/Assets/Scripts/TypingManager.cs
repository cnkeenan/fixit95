using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingManager : MonoBehaviour
{
    public string TargetScript;
    private char[] TargetScriptArray;
    private char[] currentInput;
    private int CurrentCharPointer = 0;
    // Start is called before the first frame update
    void Start()
    {
        TargetScriptArray = TargetScript.ToCharArray();
        currentInput = new char[TargetScriptArray.Length];
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach(char c in Input.inputString)
        {
            if (c == '\b')
            {
                CurrentCharPointer--;
            }
            else
            {
                currentInput[CurrentCharPointer] = c;
                CurrentCharPointer++;
            }
        }*/
    }
}
