using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float lerpSpeed = 1.0f;

    private bool west = false;
    private bool east = false;
    private bool north = false;
    private bool south = false;

    private float posX;
    private float posY;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void LateUpdate()
    {
        bool isdoor = false;
        float prePosX = posX;
        float prePosY = posY;
        posX = player.localPosition.x;
        posY = player.localPosition.y;

        if ((new Vector2(prePosX, prePosY) - new Vector2(posX, posY)).magnitude > 2.0f)
        {
            isdoor = true;
        }

        if (west == true && (prePosX - posX) > 0.0f)
        {
            posX = prePosX;
        }
        if (east == true && (prePosX - posX) < 0.0f)
        {
            posX = prePosX;
        }
        if (north == true && (prePosY - posY) < 0.0f)
        {
            posY = prePosY;
        }
        if (south == true && (prePosY - posY) > 0.0f)
        {
            posY = prePosY;
        }

        if (isdoor)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(posX, posY, transform.localPosition.z), lerpSpeed);
            Debug.Log("Lerp");
        }
        else transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "west")
        {
            west = true;
        }
        if (collision.tag == "east")
        {
            east = true;
        }
        if (collision.tag == "north")
        {
            north = true;
        }
        if (collision.tag == "south")
        {
            south = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "west")
        {
            west = false;
        }
        if (collision.tag == "east")
        {
            east = false;
        }
        if (collision.tag == "north")
        {
            north = false;
        }
        if (collision.tag == "south")
        {
            south = false;
        }
    }
}
