using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextLoader : MonoBehaviour
{
    private static TextLoader _instance;
    public LetterObject LetterPrefab;
    public MovingLetter MovingPrefab;
    List<LetterObject> Prompt = new List<LetterObject>();
    List<MovingLetter> Flyers = new List<MovingLetter>();
    public Vector3 StartPosition;
    public Vector3 FlyingStart;
    private Dictionary<char, Sprite> CharacterSprites = new Dictionary<char, Sprite>();
    private float TimeLeft = 1.0f;
    private int SendIt = 0;
    public float LetterSpeed;
    private int CurrentPrompt = 0;
    public float CurrentScore = 0;
    private float ScoreMultiplier = 1.0f;
    public TextLoaderOptions options;
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/FontTextAtlas");
        foreach(Sprite sprite in sprites)
        {
            string spriteName = sprite.name;
            int indexOfUnderscore = spriteName.IndexOf('_');
            if(indexOfUnderscore == -1)
            {
                continue;
            }
            int keyIndex = indexOfUnderscore+1;
            CharacterSprites[spriteName[keyIndex]] = sprite;
        }
        LetterSpeed = options.LetterSpeed;
        LoadPrompt();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.CapsLock))
        {
            SceneManager.LoadScene("SC01");
        }
        TimeLeft -= Time.deltaTime;
        if(TimeLeft <= 0.0f && SendIt < Prompt.Count)
        {
            Flyers[SendIt].StartMoving(LetterSpeed);
            SendIt++;
            TimeLeft = UnityEngine.Random.Range(options.MinLetterSpawn, options.MaxLetterSpawn);
        }

        if(Prompt.Count > 0 && Prompt[Prompt.Count - 1].LetterState != LetterState.NEUTRAL)
        {
            ResetPromptsAndGetScore();
            CurrentPrompt++; 


            if (CurrentPrompt < options.MiniGamePrompts.Length)
            {
                LoadPrompt();
            }
            else
            {
                SceneManager.LoadScene("SC01");
            }
        }
    }

    Sprite GetSprite(char Character)
    {
        return CharacterSprites[Character];
    }

    void ResetPromptsAndGetScore()
    {
        int success = 0;
        int total = 0;
        foreach(var item in Prompt)
        {
            total++;
            if(item.LetterState == LetterState.HIT)
            {
                success++;
            }
            Destroy(item.gameObject);
        }
        Prompt.Clear();
        foreach(var item in Flyers)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        CurrentScore += (success * ScoreMultiplier);
        float percentageCorrect = (float)success / total;
        if(percentageCorrect < 0.33f)
        {
            ScoreMultiplier = Math.Max(0.5f, ScoreMultiplier * 0.5f);
            LetterSpeed = Math.Max(0.5f, LetterSpeed * 0.5f);
        }
        else if(percentageCorrect >= 0.66f)
        {
            ScoreMultiplier = Math.Min(options.MaxLetterSpeed, ScoreMultiplier * 2.0f);
            LetterSpeed = Math.Min(options.MaxLetterSpeed, LetterSpeed * 2.0f);
        }
        Flyers.Clear();
        SendIt = 0;
    }
    
    void LoadPrompt()
    {
        Vector3 Position = StartPosition;
        Vector3 flyerPosition = FlyingStart;
        foreach (char Character in options.MiniGamePrompts[CurrentPrompt])
        {
            float xIncrement = 0.2f;

            if (Character != ' ')
            {
                LetterObject letter = Instantiate(LetterPrefab, Position, Quaternion.identity);
                MovingLetter flyer = Instantiate(MovingPrefab, flyerPosition, Quaternion.identity);
                flyer.Letter = letter.Letter = Character;

                letter.SetSprite(GetSprite(Character));
                flyer.SetSprite(GetSprite(Character));
                letter.Target = flyer;
                xIncrement = letter.Renderer.bounds.size.x;
                Prompt.Add(letter);
                Flyers.Add(flyer);
            }
            Position.x += xIncrement;
            flyerPosition.x += xIncrement;
        }
    }
}
