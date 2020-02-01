using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLoader : MonoBehaviour
{
    public LetterObject letterPrefab;
    private static string testString = "Hi my name is Armand";
    List<LetterObject> letterObjects = new List<LetterObject>();
    public Vector3 StartPosition;
    private Dictionary<char, Sprite> CharacterSprites;
    // Start is called before the first frame update
    void Start()
    {
        LetterObject firstLetter = Instantiate(letterPrefab, StartPosition, Quaternion.identity);
        firstLetter.Letter = testString[0];
        firstLetter.SetSprite();
        letterObjects.Add(firstLetter);
        Sprite[] sprites = Resources.LoadAll<Sprite>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
