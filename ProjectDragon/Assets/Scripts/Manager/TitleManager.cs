using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public AudioClip TitleBGM;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Inst.Ds_BgmPlayer(TitleBGM);
        GameManager.Inst.ScreensizeReadjust();
        GameManager.Inst.ToString();
        //효과음
        //SoundManager.Inst.Ds_PlaySingle();
    }
    public void GotoLobby()
    {
        ButtonManager.GotoLobby();
    }
}
