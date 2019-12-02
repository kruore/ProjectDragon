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

    private void Start()
    {
        GetComponent<Animator>().Play("t_PortalCreate");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
