using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    void OnMouseDown() {
        SceneManager.LoadScene("SC01");
    }
}
