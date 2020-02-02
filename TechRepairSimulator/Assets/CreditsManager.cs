using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsPanel;

    private void Start()
    {
        if (creditsPanel.active)
        {
            creditsPanel.SetActive(false);
        }
    }

    public void Revere(){
        if (creditsPanel.active)
        {
            creditsPanel.SetActive(false);
        }
        else {
            creditsPanel.SetActive(true);
        }
    }
}
