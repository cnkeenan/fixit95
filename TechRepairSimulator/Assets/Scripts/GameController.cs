using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct TextLoaderOptions
{
    public string[] MiniGamePrompts;
    public float LetterSpeed;
    public float MaxLetterSpeed;
    public float ScoreThreshold;
    public float MinLetterSpawn;
    public float MaxLetterSpawn;
}

public class GameController : MonoBehaviour
{
    public GameObject _call;
    private static GameController _instance;

    public float upperBound;
    public static bool wasCreated;
    public int completedCalls = 0;
    public int failedCalls = 0;

    public GameObject callIndicator;
    public GameObject callPad;
    public Sprite previousNameplate;
    public Sprite[] faceplates;
    public Text uiText;
    public TextLoader TextLoaderPrefab;
    public LetterObject LetterPrefab;
    public MovingLetter MovingPrefab;
    public Vector3 StartPosition;
    public Vector3 FlyingStart;

    private TextLoader MiniGameLoader;
    public Dictionary<string, TextLoaderOptions> MiniGameScenarioOptions;
    public string CurrentScenario;

    void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "SC02")
        {
            MiniGameLoader = Instantiate(TextLoaderPrefab);
            MiniGameLoader.options = MiniGameScenarioOptions[CurrentScenario];
            MiniGameLoader.LetterPrefab = LetterPrefab;
            MiniGameLoader.MovingPrefab = MovingPrefab;
            MiniGameLoader.StartPosition = StartPosition;
            MiniGameLoader.FlyingStart = FlyingStart;
        }
        else if (scene.name == "SC01")
        {
            if(MiniGameLoader != null)
            {
                //do something with the score
                Destroy(MiniGameLoader.gameObject);
                //get next scenario;
            }
        }
    }

    void Start()
    {
        MiniGameScenarioOptions = new Dictionary<string, TextLoaderOptions>();
        MiniGameScenarioOptions.Add("TEST", new TextLoaderOptions()
        {
            LetterSpeed = 2.0f,
            MaxLetterSpawn = 2.0f,
            MinLetterSpawn = 1.0f,
            MaxLetterSpeed = 8.0f,
            ScoreThreshold = 15.0f,
            MiniGamePrompts = new string[]
            {
                "Bonzi buddy still exists",
                "Fake Credit Card Numbers",
                "Bad input HR dont fire me",
                "Why to old people use tech"
            }
        });
        wasCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (callIndicator)
        {
            if (callIndicator.GetComponent<IndicatorScript>().callFailed > 0)
            {
                failedCalls++;
                callIndicator.GetComponent<IndicatorScript>().callFailed--;
            }

            if (callPad.GetComponent<AnswerCall>().answered)
            {
                callIndicator.GetComponent<IndicatorScript>().onHold = true;
                GameObject diagBox = GameObject.FindGameObjectWithTag("DialogBox");
                if (diagBox != null && diagBox.activeSelf)
                {
                    callIndicator.GetComponent<TransitionToMinigame>().locked = true;
                }
                else
                {
                    callIndicator.GetComponent<TransitionToMinigame>().locked = false;
                }
            }

            uiText.text = $"Rating: {completedCalls} of {completedCalls + failedCalls}";
        }
    }
}
