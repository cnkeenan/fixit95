using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    void OnMouseDown() {
        Animator anim = gameObject.GetComponent<Animator>();
        if(anim)
            anim.SetBool("Pressed", true);

        Application.Quit();
    }
}
