using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    public float speed;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();    
    }

    private void FixedUpdate()
    {
        
        if(Input.GetKey(KeyCode.W))
        {
            rigidbody2D.velocity = new Vector2(0.0f, speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody2D.velocity = new Vector2(0.0f, -speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody2D.velocity = new Vector2(-speed, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody2D.velocity = new Vector2(speed, 0.0f);
        }    
    }

}
