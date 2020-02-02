using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    void OnMouseDown() {
        gameObject.GetComponent<Animator>().SetBool("Pressed", true);

        SceneManager.LoadScene("SC01");

        
    }
}
