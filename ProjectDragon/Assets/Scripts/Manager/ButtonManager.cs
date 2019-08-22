using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class ButtonManager
{
    #region public
    /// <summary>
    /// 핸드폰에서의 취소버튼을 한번 눌렀을시 동작하게 할 함수 "이전동작으로 돌아가기",닫기버튼에서도 동일동작할것.
    /// </summary>
    public static string TouchBackButton()
    {
        string debug;
        if (GameManager.Inst.Scenestack.Count > 0)
        {
            switch (debug=GameManager.Inst.Scenestack.Pop())
            {
                case "Main":
                    //StopAllCoroutines();
                    GameManager.Inst.Scenestack.Push("Main");
                    return "Main";
                    break;
                case "ChangeEquip":
                    //StopAllCoroutines();
                    LobbyManager.inst.changeEquip.SetActive(false);
                    LobbyManager.inst.BGID.SetActive(false);
                    return "ChangeEquip";
                    break;
                case "CurrentEquip":
                    //StopAllCoroutines();
                    LobbyManager.inst.Inventoryback.transform.Find("CurrentEquip").gameObject.SetActive(false);
                    LobbyManager.inst.BGI.SetActive(false);
                    break;
                case "Lock":
                    LobbyManager.inst.Inventoryback.transform.Find("Lock").gameObject.SetActive(false);
                    LobbyManager.inst.BGID.SetActive(false);
                    break;
                case "EquipPanel":
                    GameObject.Find("UI Root").transform.Find("EquipPanel").gameObject.SetActive(false);
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
                default:
                    Debug.Log(debug);
                    break;
            }
            
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
        return null;
    }
    /// <summary>
    /// SetActiveObject에있는 activeobject모두켜기,deactiveobject모두끄기
    /// </summary>
    public static void ObjectlistControl()
    {
        if (UIButton.current.gameObject.GetComponent<SetActiveObject>() != null)
        {
            SetActiveObject objectlistscirpt = UIButton.current.gameObject.GetComponent<SetActiveObject>();
            
            foreach (GameObject obj in objectlistscirpt.activeObject)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in objectlistscirpt.deactiveObject)
            {
                obj.SetActive(false);
            }
            objectlistscirpt.objectset = !objectlistscirpt.objectset;
        }
        else
        {
            Debug.LogError("SetActiveObject가 없어요.");
        }
    }
    /// <summary>
    /// SetActiveObject안의 값을 이용하여 특정 오브젝트를 끄고 켜기
    /// </summary>
    /// <param name="setting">true값이라면 항상activeobject켜고 deactiveobject끄기 false라면 activeobject만 끄기 </param>
    public static void ObjectlistControl(bool setting)
    {
        if (UIButton.current.gameObject.GetComponent<SetActiveObject>() != null)
        {
            SetActiveObject objectlistscirpt = UIButton.current.gameObject.GetComponent<SetActiveObject>();
            if (setting)
            {
                foreach (GameObject obj in objectlistscirpt.activeObject)
                {
                    obj.SetActive(true);
                }
                foreach (GameObject obj in objectlistscirpt.deactiveObject)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                foreach (GameObject obj in objectlistscirpt.activeObject)
                {
                    obj.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("SetActiveObject가 없어요.");
        }
    }
    #endregion
    /// <summary>
    /// 로비씬전환
    /// </summary>
    public static void GotoLobby()
    {
        SceneManager.LoadScene("Lobby");

    }
}
