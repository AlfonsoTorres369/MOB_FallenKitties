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

    // Kitty Logic
    private float velocity;
    private bool enabledMovement = true;

    private void Start()
    {
        if(GameManager.Instance)
        {
            GameManager.Instance.OnStopGame += Deactivate;
            GameManager.Instance.OnPause += PauseKitty;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Fall();
    }

    // --------- Kitties Configuration ---------
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
    // ------------------------------------

    // --------- Kitty Logic ---------
    private void Fall()
    {
        if(enabledMovement)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= velocity * Time.deltaTime;
            transform.position = newPosition;
        }
    }

    public void Activate(Vector3 _position)
    {
        transform.position = _position;
        gameObject.SetActive(true);
        CreateKitty();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void PauseKitty(bool _status)
    {
        enabledMovement = !_status;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            Deactivate();
            GameManager.Instance.SubstractLife();
        }
    }

    // ------------------------------------
}
