using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    private float timeToWait = 1;


    void OnMouseDown() {
        gameObject.GetComponent<Animator>().SetBool("Pressed", true);

        /*float timeWaited = 0;

        while (timeWaited < timeToWait) {
            timeWaited += Time.deltaTime;
        }*/

        SceneManager.LoadScene("SC01");

        
    }
}
