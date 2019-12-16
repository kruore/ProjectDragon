
// ==============================================================
// Portal Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

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

    //포탈에 부딪혔을 경우 미니맵 확대
    private void OnTriggerEnter2D(Collider2D collision)
    {
#if UNITY_EDITOR
        Debug.Log(isPortalOn);
#endif
        if(collision.CompareTag("Player") && isPortalOn)
        {
            RoomManager.MiniMapMaximalize();
        }
    }
    //포탈 범위에서 나가면 미니맵 축소
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isPortalOn)
        {
            RoomManager.MiniMapMinimalize();
        }
    }
}
