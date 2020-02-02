using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class TextLoaderOptions
{
    public string[] MiniGamePrompts;
    public float LetterSpeed;
    public float MaxLetterSpeed;
    public float ScoreThreshold;
    public float MinLetterSpawn;
    public float MaxLetterSpawn;
}

[System.Serializable]
public class ScenarioOptions
{
    public string[] Scenarios;
    public TextLoaderOptions MiniGameOptions;
    public int FacePlateIndex;
}

public class GameController : MonoBehaviour
{
    public GameObject _call;
    private static GameController _instance;

    public float upperBound;
    public static bool wasCreated;
    public int completedCalls = 0;
    public int failedCalls = 0;

    GameObject callIndicator;
    GameObject callPad;
    public GameObject callIndicatorPrefab;
    public GameObject callPadPrefab;
    public Sprite previousNameplate;
    public Sprite[] faceplates;
    public TextLoader TextLoaderPrefab;
    public LetterObject LetterPrefab;
    public MovingLetter MovingPrefab;
    public Vector3 StartPosition;
    public Vector3 FlyingStart;

    private TextLoader MiniGameLoader;
    public ScenarioOptions[] MiniGameScenarioOptions;
    public int CurrentMinigameIndex;
    private bool Lost;
    public GameOver GameOver;

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        var jsonPath = Resources.Load<TextAsset>("Dialog/dialog");
        MiniGameScenarioOptions = JsonHelper.FromJson<ScenarioOptions>(jsonPath.text);

        faceplates = Resources.LoadAll<Sprite>("Sprites/Faceplates-Sheet");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SC02")
        {
            MiniGameLoader = Instantiate(TextLoaderPrefab);
            MiniGameLoader.options = MiniGameScenarioOptions[CurrentMinigameIndex].MiniGameOptions;
            MiniGameLoader.LetterPrefab = LetterPrefab;
            MiniGameLoader.MovingPrefab = MovingPrefab;
            MiniGameLoader.StartPosition = StartPosition;
            MiniGameLoader.FlyingStart = FlyingStart;
        }
        else if (scene.name == "SC01")
        {
            if (MiniGameLoader != null)
            {
                if (MiniGameLoader.CurrentScore < 0)
                {
                    Lost = true;
                    SceneManager.LoadScene("Game Results_BAD");
                    return;
                }

                CurrentMinigameIndex++;
                if (CurrentMinigameIndex >= MiniGameScenarioOptions.Length)
                {
                    Lost = false;
                    SceneManager.LoadScene("Game Results_BAD");
                    return;
                }

                //do something with the score
                Destroy(MiniGameLoader.gameObject);
                //get next scenario;
            }
            callPad = Instantiate(callPadPrefab);
            callIndicator = Instantiate(callIndicatorPrefab);
            callIndicator.name = "Indicator";
            callPad.GetComponent<AnswerCall>().CreateNarration(MiniGameScenarioOptions[CurrentMinigameIndex].Scenarios);
        }
        else if (scene.name == "Game Results_BAD")
        {
            GameOver = FindObjectOfType<GameOver>();
            GameOver.ExecuteGameOver(Lost);
        }
    }

    void Start()
    {
        
        wasCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (callIndicator)
        {
            var facePlate = GameObject.FindGameObjectWithTag("SpeakerFacePlate");
            if (facePlate != null)
            {
                Debug.Log(MiniGameScenarioOptions[CurrentMinigameIndex].FacePlateIndex);
                Debug.Log(faceplates[MiniGameScenarioOptions[CurrentMinigameIndex].FacePlateIndex].name);
                facePlate.GetComponent<Image>().sprite = faceplates[MiniGameScenarioOptions[CurrentMinigameIndex].FacePlateIndex];
            }
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
        }
    }
}
