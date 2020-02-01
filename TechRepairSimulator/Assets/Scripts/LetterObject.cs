using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum LetterState
{
    HIT,
    MISSED,
    NEUTRAL
};

public class LetterObject : MonoBehaviour
{
    public char Letter;
    public Renderer Renderer;
    private LetterState LetterState = LetterState.NEUTRAL;
    public BoxCollider2D Collider;
    private SpriteRenderer SpriteRenderer;
    public MovingLetter Target;

    private void Awake()
    {
        transform.localScale = new Vector2(1, 1);
        Renderer = GetComponent<Renderer>();
        Collider = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetSprite(Sprite spriteToSet)
    {
        SpriteRenderer.sprite = spriteToSet;

        Collider.size = spriteToSet.bounds.size;
        Collider.offset = (spriteToSet.bounds.size * 0.5f) + spriteToSet.bounds.min;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(LetterState == LetterState.NEUTRAL && other.gameObject == Target.gameObject)
        {
            ScaleAround(Target.gameObject, Target.transform.position + (SpriteRenderer.sprite.bounds.size*0.5f), new Vector3(2.0f, 2.0f, 2.0f));

            // finally, actually perform the scale/translation

            if(Input.GetKeyDown(Letter.ToString()))
            {
                LetterState = LetterState.HIT;
                SpriteRenderer.material.color = new Color(0.0f, 1.0f, 0.0f);
                Debug.Log(LetterState);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(LetterState == LetterState.NEUTRAL && other.gameObject == Target.gameObject)
        {
            LetterState = LetterState.MISSED;
            SpriteRenderer.material.color = new Color(1.0f, 0.0f, 0.0f);
            Debug.Log(LetterState);
        }
    }

    public void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.transform.localPosition;
        Vector3 B = pivot;

        Vector3 C = A - B; // diff from object pivot to desired pivot/origin

        float RS = newScale.x / target.transform.localScale.x; // relataive scale factor

        // calc final position post-scale
        Vector3 FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.transform.localScale = newScale;
        target.transform.localPosition = FP;
    }
}
