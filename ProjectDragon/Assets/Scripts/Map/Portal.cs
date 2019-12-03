using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool IsPortalOn
    {
        get { return isPortalOn; }
        set
        {
            isPortalOn = value;
            gameObject.SetActive(isPortalOn);
        }
    }

    private RoomManager RoomManager;
    private bool isPortalOn = false;

    private void Start()
    {
        RoomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        GetComponent<Animator>().Play("t_PortalCreate");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(isPortalOn);
        if(collision.CompareTag("Player") && isPortalOn)
        {
            RoomManager.MiniMapMaximalize();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isPortalOn)
        {
            RoomManager.MiniMapMinimalize();
        }
    }
}
