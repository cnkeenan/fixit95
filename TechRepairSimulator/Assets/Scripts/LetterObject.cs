using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LetterState
{
    HIT,
    MISSED,
    NEUTRAL
};

public class LetterObject : MonoBehaviour
{
    public char Letter;
    public Renderer Renderer;
    public LetterState LetterState = LetterState.NEUTRAL;
    public BoxCollider2D Collider;
    private SpriteRenderer SpriteRenderer;
    public MovingLetter Target;
    public AudioClip AudioSuccess;
    public AudioClip AudioFailed;
    public Vector2 ColliderBuffer;
    public float ClipVolume;
    private bool TargetWithinBounds = false;

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
        if(TargetWithinBounds && LetterState == LetterState.NEUTRAL)
        {
            if (Input.GetKeyDown(Letter.ToString().ToLower()))
            {
                Debug.Log($"Hit: {Letter}");
                HandleHit();
            }
            else if (Input.anyKeyDown)
            {
                Debug.Log($"Wrong: {Letter}");
                HandleMiss();
            }
        }
    }

    public void SetSprite(Sprite spriteToSet)
    {
        SpriteRenderer.sprite = spriteToSet;

        Collider.size = spriteToSet.bounds.size;
        Collider.size += ColliderBuffer;
        Collider.offset = (spriteToSet.bounds.size * 0.5f) + spriteToSet.bounds.min;
    }

    public void HandleHit()
    {
        Shrink();
        LetterState = LetterState.HIT;
        Destroy(Target.gameObject);
        SpriteRenderer.material.color = new Color(0.0f, 1.0f, 0.0f);
        AudioSource.PlayClipAtPoint(AudioSuccess, transform.position, 1.0f);
    }

    public void HandleMiss()
    {
        Shrink();
        LetterState = LetterState.MISSED;
        Destroy(Target.gameObject);
        SpriteRenderer.material.color = new Color(1.0f, 0.0f, 0.0f);
        AudioSource.PlayClipAtPoint(AudioFailed, transform.position, 1.0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Target != null && other.gameObject == Target.gameObject)
        {
            Grow();
            TargetWithinBounds = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (LetterState == LetterState.NEUTRAL && other.gameObject == Target.gameObject)
        {
            TargetWithinBounds = false;
            Debug.Log($"Exit Miss: {Letter}");
            HandleMiss();
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

    public void Shrink()
    {
        ScaleAround(Target.gameObject, Target.transform.position + SpriteRenderer.sprite.bounds.size, new Vector3(1.0f, 1.0f, 1.0f));
    }

    public void Grow()
    {
        ScaleAround(Target.gameObject, Target.transform.position + (SpriteRenderer.sprite.bounds.size * 0.5f), new Vector3(2.0f, 2.0f, 2.0f));
    }
}
