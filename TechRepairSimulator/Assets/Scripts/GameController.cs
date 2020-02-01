using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject _call;
    
    public float upperBound;
    public static bool wasCreated;
    public int completedCalls = 0;

    void Start() {
        wasCreated = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CallWaiting(Random.Range(1.0f, upperBound)));
    }

    // Waits and instantiates if it doesn't exist
    IEnumerator CallWaiting(float waitTime) {
        if (!wasCreated) {
            yield return new WaitForSeconds(waitTime);
            wasCreated = true;
        }
    }
}
