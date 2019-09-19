using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public UILabel test;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.width * 16 / 9,Screen.width, true);
        GameManager.Inst.singletonF();
        DataTransaction.Inst.ToString();
        test.text = Database.Inst.playData.inventory.Count.ToString();
    }
    public void GotoLobby()
    {
        ButtonManager.GotoLobby();
    }
}
