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

    private bool isPortalOn = false;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Animator>().Play("t_PortalCreate");
    }

    public void PortalOn()
    {

    }
}
