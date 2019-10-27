using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Inst.ScreensizeReadjust();
        DataTransaction.Inst.ToString();
    }
    public void GotoLobby()
    {
        ButtonManager.GotoLobby();
    }
}
