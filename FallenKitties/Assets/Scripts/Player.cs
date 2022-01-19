using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Camera Camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Camera)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 newPosition = new Vector3(Camera.ScreenToWorldPoint(touch.position).x, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
