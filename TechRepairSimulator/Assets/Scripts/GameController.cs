using Narrate;
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
    public string ClipSource;
    public AudioClip[] Clips;
}

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public float upperBound;
    public static bool wasCreated;

    public AudioClip clickSound;
    public AudioClip mainMenu;
    public AudioClip waitingForCall;
    public GameObject ratty;

    public GameObject CallPadIndicatorPrefab;
    public GameObject CallPadIndicator;

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
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        var jsonPath = Resources.Load<TextAsset>("Dialog/dialog");
        MiniGameScenarioOptions = JsonHelper.FromJson<ScenarioOptions>(jsonPath.text);

        foreach (ScenarioOptions option in MiniGameScenarioOptions)
        {
            option.Clips = Resources.LoadAll<AudioClip>(option.ClipSource);
        }

        faceplates = Resources.LoadAll<Sprite>("Sprites/Faceplates-Sheet");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnMouseDown()
    {
        AudioSource.PlayClipAtPoint(clickSound, transform.position);
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
            Narrate.NarrationManager.instance.GetComponent<AudioSource>().loop = true;
        }
        else if (scene.name == "SC01")
        {
            Narrate.NarrationManager.instance.GetComponent<AudioSource>().Play();
            Narrate.NarrationManager.instance.GetComponent<AudioSource>().clip = waitingForCall;
            Narrate.NarrationManager.instance.GetComponent<AudioSource>().Play();
            if (MiniGameLoader != null)
            {
                if (MiniGameLoader.CurrentScore < MiniGameLoader.options.ScoreThreshold)
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
                Destroy(MiniGameLoader.gameObject);
            }

            CallPadIndicator = Instantiate(CallPadIndicatorPrefab);
            CallPadIndicator.GetComponent<AnswerCall>().CreateNarration(MiniGameScenarioOptions[CurrentMinigameIndex].Scenarios, MiniGameScenarioOptions[CurrentMinigameIndex].Clips);
        }
        else if (scene.name == "MainMenu")
        {
            Narrate.NarrationManager.instance.GetComponent<AudioSource>().clip = mainMenu;
            Narrate.NarrationManager.instance.GetComponent<AudioSource>().Play();

            if(MiniGameLoader != null)
            {
                CurrentMinigameIndex = 0;
                Destroy(MiniGameLoader.gameObject);
            }
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
        if (CallPadIndicator)
        {
            if(NarrationManager.instance.isPlaying)
            {
                CallPadIndicator.GetComponentInChildren<Animator>().SetBool("Incoming", false);
                CallPadIndicator.GetComponentInChildren<Animator>().SetBool("Hold", true);
                CallPadIndicator.GetComponent<AnswerCall>().answered = true;
            }
            var facePlate = GameObject.FindGameObjectWithTag("SpeakerFacePlate");
            if (facePlate != null)
            {
                facePlate.GetComponent<Image>().sprite = faceplates[MiniGameScenarioOptions[CurrentMinigameIndex].FacePlateIndex];
            }

            if (CallPadIndicator.GetComponent<AnswerCall>().answered)
            {
                CallPadIndicator.GetComponent<IndicatorScript>().onHold = true;
                GameObject diagBox = GameObject.FindGameObjectWithTag("DialogBox");
                if (NarrationManager.instance.isPlaying)
                {
                    CallPadIndicator.GetComponent<TransitionToMinigame>().locked = true;
                }
                else
                {
                    CallPadIndicator.GetComponent<TransitionToMinigame>().locked = false;
                }
            }
        }
    }
}
