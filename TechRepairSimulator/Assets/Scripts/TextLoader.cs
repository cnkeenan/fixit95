using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextLoader : MonoBehaviour
{
    public LetterObject LetterPrefab;
    public MovingLetter MovingPrefab;
    private static string[] TestStrings = new string[]
        {
            "how to fix hp printer",
            "jobs near me that dont suck",
            "hp printer model 2501 jammed",
            "please better jobs near me"
        };
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
    public float ScoreThreshold;
    public float MinLetterSpawn;
    public float MaxLetterSpawn;
    private void Awake()
    {
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
        LoadPrompt();
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        if(TimeLeft <= 0.0f && SendIt < Prompt.Count)
        {
            Flyers[SendIt].StartMoving(LetterSpeed);
            SendIt++;
            TimeLeft = UnityEngine.Random.Range(MinLetterSpawn, MaxLetterSpawn);
        }

        if(Prompt.Count > 0 && Prompt[Prompt.Count - 1].LetterState != LetterState.NEUTRAL)
        {
            ResetPromptsAndGetScore();
            CurrentPrompt++; 


            if (CurrentPrompt < TestStrings.Length)
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
            ScoreMultiplier = Math.Min(8.0f, ScoreMultiplier * 2.0f);
            LetterSpeed = Math.Min(8.0f, LetterSpeed * 2.0f);
        }
        Flyers.Clear();
        SendIt = 0;
    }
    
    void LoadPrompt()
    {
        Vector3 Position = StartPosition;
        Vector3 flyerPosition = FlyingStart;
        foreach (char Character in TestStrings[CurrentPrompt])
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
