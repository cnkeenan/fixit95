using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLetter : MonoBehaviour
{
    public char Letter;
    public Renderer Renderer;
    private bool IsMoving = false;
    private float Speed = 1.0f;
    public BoxCollider2D Collider;

    private void Awake()
    {
        transform.localScale = new Vector2(1, 1);
        Renderer = GetComponent<Renderer>();
        Collider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving)
        {
            Vector3 move = new Vector3(0, 1, 0);
            transform.position += move * Speed * Time.deltaTime;
        }
    }

    public void SetSprite(Sprite spriteToSet)
    {
        GetComponent<SpriteRenderer>().sprite = spriteToSet;
        Collider.size = spriteToSet.bounds.size;
        Collider.offset = (spriteToSet.bounds.size * 0.5f) + spriteToSet.bounds.min;
    }

    public void StartMoving(float speed = 1.0f)
    {
        IsMoving = true;
        Speed = speed;
    }
}
