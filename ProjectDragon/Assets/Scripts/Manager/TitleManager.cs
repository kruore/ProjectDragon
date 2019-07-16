using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.width * 16 / 9,Screen.width, true);
        GameManager.Inst.singletonF();
    }
    public void GotoLobby()
    {
        ButtonManager.GotoLobby();
    }
}
