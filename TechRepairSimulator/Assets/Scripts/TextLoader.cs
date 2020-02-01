using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLoader : MonoBehaviour
{
    public LetterObject LetterPrefab;
    public MovingLetter MovingPrefab;
    private static string testString = "www.google.com";
    List<LetterObject> Prompt = new List<LetterObject>();
    List<MovingLetter> Flyers = new List<MovingLetter>();
    public Vector3 StartPosition;
    public Vector3 FlyingStart;
    private Dictionary<char, Sprite> CharacterSprites = new Dictionary<char, Sprite>();
    private float TimeLeft = 1.0f;
    private int SendIt = 0;
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

        Vector3 Position = StartPosition;
        Vector3 flyerPosition = FlyingStart;
        foreach(char Character in testString)
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

    // Update is called once per frame
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        if(TimeLeft <= 0.0f && SendIt < Prompt.Count)
        {
            Flyers[SendIt].StartMoving(2.0f);
            SendIt++;
            TimeLeft = 1.0f;
        }
    }

    Sprite GetSprite(char Character)
    {
        return CharacterSprites[Character];
    }
}
