using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    private float timeToWait = 1;
    public AudioClip startSound;
    private Coroutine play;

    void OnMouseDown()
    {
        if (play == null)
        {
            play = StartCoroutine(PlayAudioAndAnimate());
            SceneManager.LoadScene("SC01");
        }
    }

    IEnumerator PlayAudioAndAnimate()
    {
        gameObject.GetComponent<Animator>().SetBool("Pressed", true);
        GameObject.Find("Ratty").GetComponent<Animator>().SetBool("Alert", true);
        AudioSource.PlayClipAtPoint(startSound, transform.position);
        yield return new WaitForSeconds(1);
    }
}
