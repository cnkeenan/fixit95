using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterObject : MonoBehaviour
{
    public char Letter;
    private void Awake()
    {
        transform.localScale = new Vector2(2, 2);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetSprite()
    {
        Sprite newSprite = (Sprite)Resources.Load($"Sprites/Letter_{Letter}", typeof(Sprite));
        GetComponent<SpriteRenderer>().sprite = newSprite;
        
    }
}
