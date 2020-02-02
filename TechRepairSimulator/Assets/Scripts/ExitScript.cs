using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    void OnMouseDown() {
            gameObject.GetComponent<Animator>().SetBool("Pressed", true);

            Application.Quit();
    }
}
