using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class ButtonManager
{
    /// <summary>
    /// 핸드폰에서의 취소버튼을 한번 눌렀을시 동작하게 할 함수 "이전동작으로 돌아가기"
    /// </summary>
    public static void TouchBackButton()
    {
        if (GameManager.Inst.Scenestack.Count > 0)
        {
            switch (GameManager.Inst.Scenestack.Pop())
            {
                case "Main":
                    //StopAllCoroutines();
                    GameManager.Inst.Scenestack.Push("Main");
                    break;
                case "mapscene":
                    //StopAllCoroutines();
                    SceneManager.LoadScene("Main");
                    break;
                case "InventoryLobby":
                    //StopAllCoroutines();
                    SceneManager.LoadScene("Main");
                    break;
                case "Inventorypanel":
                    //DeactiveInventorypanel();
                    break;
                case "CardList":
                    //DeactiveCardListpanel();
                    break;
                case "ARscene":
                    //StopAllCoroutines();
                    SceneManager.LoadScene("mapscene");
                    break;
                case "Loading":
                    SceneManager.LoadScene("mapscene");
                    break;
                case "Battle2":
                    SceneManager.LoadScene("Main");
                    break;
                case "ToolBox":
                    //DeactiveToolboxpanel();
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }
    /// <summary>
    /// 로비씬전환
    /// </summary>
    public static void GotoLobby()
    {
        SceneManager.LoadScene("Lobby");

    }
}
