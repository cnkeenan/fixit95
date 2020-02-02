using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Coroutine endGame;
    public float speed = 1;
    public Image gameOverPanel;

    private RectTransform rect;

    [SerializeField]
    private string message = "Hey You Stink! Poopyhead!!!";
    [SerializeField]
    private string loseMessage = "Please come to my office to discuss your pathetic performance!";
    [SerializeField]
    private string winMessage = "Hey Great Job!\n(no raise tho)";

    [SerializeField]
    private Text text;

    [SerializeField]
    private Text gameOverText;


    // Start is called before the first frame update
    private void Start()
    {
        rect = gameObject.GetComponent<Image>().rectTransform;
        rect.localScale = new Vector2(0,0);
        ExecuteGameOver();
    }

    public void ExecuteGameOver( bool lose = true)
    {
        if (lose)
        {
            message = loseMessage;
        }
        else {
            message = winMessage;
        }

        if (text != null)
        {
            text.text = "";
        }
        else {
            Debug.Log("Please connect text to GameOver: " + gameObject.name);
            return;
        }

        if (gameOverText != null)
        {
            gameOverText.text = "";
        }
        else {
            Debug.Log("Please connect gameOverText to GameOver: " + gameObject.name);
            return;
        }


        if (gameOverPanel != null) {
            gameOverPanel.color = new Color(gameOverPanel.color.r, gameOverPanel.color.g, gameOverPanel.color.b, 0.0f);
        }

        if (speed < 0.0f) {
            speed = 1.0f;
        }

        if (endGame == null) {
            endGame =  StartCoroutine(result());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator result() {

        while (rect.localScale.x < 1) {
            rect.localScale = new Vector2(rect.localScale.x + 1 / 60.0f, rect.localScale.y + 1 / 60.0f);
            yield return new WaitForSeconds(1 / 60.0f / speed);
        }

        rect.localScale = new Vector2(1, 1);

        for (int i = 0; i < message.Length; i++) {
            yield return new WaitForSeconds(1 / speed);
            text.text = text.text + message[i];

            
        }

        yield return new WaitForSeconds(3);

        if (gameOverPanel != null) {
            while (gameOverPanel.color.a < 1.0f) {
                gameOverPanel.color = new Color(gameOverPanel.color.r, gameOverPanel.color.g, gameOverPanel.color.b, gameOverPanel.color.a + 1/60.0f);
                yield return new WaitForSeconds(1 / 60.0f / speed);
            }
        }

        string gameOver = "GAME\nOVER";

        for (int i = 0; i < gameOver.Length; i++) {

            yield return new WaitForSeconds(1 / speed);
            gameOverText.text = gameOverText.text + gameOver[i];
        }


        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);

        yield break;
    }


}
