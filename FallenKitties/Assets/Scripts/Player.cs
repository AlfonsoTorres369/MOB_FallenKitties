using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Camera Camera;

    private bool enabledInput = true;


    private void Start()
    {
        if(GameManager.Instance)
        {
            GameManager.Instance.OnStopGame += Deactivate;
            GameManager.Instance.OnPause += PausePlayer;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Camera && enabledInput)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 newPosition = new Vector3(Camera.ScreenToWorldPoint(touch.position).x, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null)
        {
            collision.gameObject.SetActive(false);

            if(GameManager.Instance)
                GameManager.Instance.AddScore();
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetInput(bool _newValue)
    {
        enabledInput = _newValue;
    }

    public void PausePlayer(bool _status)
    {
        SetInput(!_status);
    }
}
