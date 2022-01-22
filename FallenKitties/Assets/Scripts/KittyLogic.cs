using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittyLogic : MonoBehaviour
{
    [Header("Kitty Self References")]
    public SpriteRenderer SpriteRenderer;

    [Header("Configuration")]
    public Sprite[] KittiesSprites;
    public float MinVelocity;
    public float MaxVelocity;
    public float VelocityFactor;

    private float velocity;

    // Update is called once per frame
    void Update()
    {
        Fall();
    }

    private void CreateKitty()
    {
        SpriteRenderer.sprite = SelectKittySprite();
        SpriteRenderer.color = SelectKittyColor();
        velocity = CalculateKittyVelocity();
    }

    private Sprite SelectKittySprite()
    {
        int index = (int)Mathf.Clamp(GameManager.GetRandomNumber(0f, KittiesSprites.Length), 0, KittiesSprites.Length-1);
        return KittiesSprites[index];
    }

    private Color SelectKittyColor()
    {
        Color color = new Color();
        color.r = GameManager.GetRandomNumber(0, 1);
        color.g = GameManager.GetRandomNumber(0, 1);
        color.b = GameManager.GetRandomNumber(0, 1);
        color.a = 1;

        return color;
    }

    private float CalculateKittyVelocity()
    {
        return GameManager.GetRandomNumber(MinVelocity, MaxVelocity + VelocityFactor * GameManager.Instance.GetGameLevel());
    }

    private void Fall()
    {
        Vector3 newPosition = transform.position;
        newPosition.y -= velocity * Time.deltaTime;
        transform.position = newPosition;
    }

    public void Activate(Vector3 _position)
    {
        transform.position = _position;
        gameObject.SetActive(true);
        CreateKitty();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            gameObject.SetActive(false);
            GameManager.Instance.AddDeadKitty();
        }
    }
}
