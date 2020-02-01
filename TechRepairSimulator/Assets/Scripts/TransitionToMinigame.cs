using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionToMinigame : MonoBehaviour
{
    void OnMouseDown()
    {
        var anim = GetComponent<Animator>();

        if (anim.GetBool("Hold"))
        {
            SceneManager.LoadScene("SC02");
        }
    }
}
