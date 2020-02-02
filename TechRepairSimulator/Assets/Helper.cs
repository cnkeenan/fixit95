using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject helpBar;

    private void Awake()
    {
        if (helpBar != null)
        {
            helpBar.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (helpBar != null)
        {
            helpBar.SetActive(true);
        }
        else {
            Debug.Log("Add help bar");
        }
    }

    public void closeHelp() {
        if (helpBar != null)
        {
            helpBar.SetActive(false);
        }
    } 
}
