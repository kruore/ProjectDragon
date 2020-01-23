
// ==============================================================
// TitleManager
// All presentation and Load db, login, data checking manager
// 
//  AUTHOR: Kim Dong Ha
// CREATED: 2020-01-22
// UPDATED:
//===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //Database
    public string nickName;
    public SEX sex;
    public CLASS Item_Class;

    //ScreenTransition
    public Camera camera;

    public GameObject mainScene; //메인 화면
    public GameObject nickNameScene; //닉네임 화면

    //게임 로고 연출
    public GameObject gameLogo;
    public UITexture gameLogo_Effect;
    public GameObject gameLogo_Label;

    private void Awake()
    {
        Initialized();
    }

    /// <summary>
    /// Initialized Variable and Scene
    /// </summary>
    private void Initialized()
    {
        camera = GameObject.FindGameObjectWithTag("ScreenTransitions").GetComponent<Camera>();
        
        #region GameLogo
        mainScene = transform.Find("MainScene").Find("BGImage").gameObject;
        gameLogo = mainScene.transform.Find("GameLogoPanel").gameObject;
        gameLogo_Effect = gameLogo.transform.Find("Effect").GetComponent<UITexture>();
        gameLogo_Label = mainScene.transform.Find("TTSPanel").gameObject;

        mainScene.GetComponent<BoxCollider>().enabled = false;
        gameLogo.SetActive(false);
        gameLogo_Label.SetActive(false);
        #endregion

        #region NickName
        nickNameScene = transform.Find("NickNameScene").Find("BGImage").gameObject;

        nickNameScene.SetActive(false);
        nickNameScene.transform.Find("NickNameSettingImage").Find("Failed").gameObject.SetActive(false);
        #endregion
    }

    private void Start()
    {
        SoundManager.Inst.Ds_BGMPlayerDB(1);
        StartCoroutine(Presentation_Logo());
    }

    #region Title Main
    /// <summary>
    /// Presentation GameLogo
    /// </summary>
    /// <returns></returns>
    private IEnumerator Presentation_Logo()
    {
        //start delay
        yield return new WaitForSeconds(2.0f);
        //GameLogo On
        gameLogo.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        float playTime = 1.0f;
        float time = 0.0f;
        float alpha = 1.0f;

        while(time <= playTime)
        {
            time += Time.deltaTime;
            alpha = Mathf.Lerp(1.0f, 0.0f, time / playTime);
            gameLogo_Effect.color = new Color(1.0f,1.0f,1.0f,alpha);
            yield return null;
        }

        gameLogo_Label.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        mainScene.GetComponent<BoxCollider>().enabled = true;
    }

    public void Button_LogIn()
    {
#if UNITY_EDITOR
        Debug.Log("LogIn");
        Debug.Log(GameManager.Inst.CheckingPlayData());
#endif
        gameLogo_Label.SetActive(false);
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        StartCoroutine(camera.GetComponent<ScreenTransitions>().Fade(2.0f, true));

        switch (GameManager.Inst.CheckingPlayData())
        {
            case 0: //최초 실행시
                //구글 로그인
                StartCoroutine(FirstPlay());
                break;
            case 1: //플레이중인 데이터가 없을 시
                break;
            case 2: //플레이중인 데이터가 있을 시
                break;
            default:
#if UNITY_EDITOR
                Debug.LogError("Play Data is Boom");
#endif
                break;
        }
    }
    private IEnumerator FirstPlay()
    {
        yield return new WaitForSeconds(2.0f);
        mainScene.SetActive(false);
        StartCoroutine(camera.GetComponent<ScreenTransitions>().Fade(1.0f, false));
        nickNameScene.SetActive(true);
    }
    #endregion

    #region NickName

    public void NickNameInputSubmit()
    {
#if UNITY_EDITOR
        Debug.Log("NickName Submit");
#endif
        UIInput input = nickNameScene.transform.Find("NickNameSettingImage").Find("Input").GetComponent<UIInput>();
        nickName = input.label.text;
    }

    public void Button_NickNameConfirm()
    {
#if UNITY_EDITOR
        Debug.Log("NickName Confirm");
#endif
        string temp = string.Empty;

        foreach(char c in nickName)
        {
            if ('a' <= c && c <= 'z') temp += c;
            else if ('A' <= c && c <= 'Z') temp += c;
            else if ('0' <= c && c <= '9') temp += c;
            else if (0xAC00 <= c && c <= 0xD7AF) temp += c;
            else
            {
#if UNITY_EDITOR
                Debug.Log("NickName Failed");
#endif
                nickNameScene.transform.Find("NickNameSettingImage").Find("Failed").gameObject.SetActive(true);
                return;
            }
        }

        StartCoroutine(NickNameSucceed());
#if UNITY_EDITOR
        Debug.Log("NickName Succeed");
#endif
    }
    private IEnumerator NickNameSucceed()
    {
        yield return null;
    }

    public void Button_Close(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    #endregion
    #region Character & Weapon Select
    public void Button_Boy()
    {
        sex = SEX.Male;
        Event_CharactorSelectFade();
    }
    public void Button_Girl()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        sex = SEX.Female;
        Event_CharactorSelectFade();
    }
    public void Button_Sword()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        Item_Class = CLASS.검;
        Event_WeaponSelectFade();
    }
    public void Button_Wand()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        Item_Class = CLASS.지팡이;
        Event_WeaponSelectFade();
    }
    #endregion

    public void Event_CharactorSelectFade()
    {
        Database.Inst.playData.sex = sex;
        GameObject.Find("CharactorSelectScene").GetComponent<TweenAlpha>().enabled = true;
    }

    public void Event_WeaponSelectFade()
    {
        GameManager.Inst.GivePlayerBasicItem(Item_Class);
        GameObject.Find("WeaponSelectScene").GetComponent<TweenAlpha>().enabled = true;
    }

    public void Event_Destroy(GameObject _gameObject)
    {
        Destroy(_gameObject);
    }

    public void Event_NextScene()
    {
        SceneManager.LoadScene("Lobby");
    }

    //private IEnumerator translate(int _cutCount)
    //{
    //    switch (_cutCount)
    //    {
    //        case 0:
    //            uiCamera.rect = new Rect(0.15f, 0.0f, 0.7f, 1.0f);
    //            StartCoroutine(CameraScaling(1.0f, 0.5f));
    //            tweenTransform.to = points[0];
    //            tweenTransform.PlayForward();
    //            break;
    //        case 1:
    //            tweenTransform.to = points[1];
    //            tweenTransform.PlayForward();
    //            break;
    //        case 2:
    //            tweenTransform.to = points[2];
    //            tweenTransform.PlayForward();
    //            break;
    //        case 3:
    //            uiCamera.rect = new Rect(0.2f, 0.0f, 0.6f, 1.0f);
    //            StartCoroutine(CameraScaling(1.0f, 0.6f));
    //            tweenTransform.to = points[3];
    //            tweenTransform.PlayForward();
    //            break;
    //        case 4:
    //            tweenTransform.to = points[4];
    //            tweenTransform.PlayForward();
    //            break;
    //        case 5:
    //            tweenTransform.to = points[5];
    //            tweenTransform.PlayForward();
    //            break;
    //        case 6:
    //            uiCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
    //            StartCoroutine(CameraScaling(1.0f, 1.0f));
    //            tweenTransform.to = points[6];
    //            tweenTransform.PlayForward();
    //            break;
    //        case 7:
    //            cartoon.enabled = true;
    //            break;
    //    }
    //    yield return new WaitForSeconds(1.2f);

    //    if (_cutCount != 7) nextCutButton.SetActive(true);
    //}

    //private IEnumerator CartoonTransition()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    uiCamera.rect = new Rect(0.15f, 0.0f, 0.7f, 1.0f);
    //    StartCoroutine(CameraScaling(1.0f, 0.5f));
    //    tweenTransform.to = points[0];
    //    tweenTransform.PlayForward();
    //    yield return new WaitForSeconds(2.0f);
    //    tweenTransform.to = points[1];
    //    tweenTransform.PlayForward();
    //    yield return new WaitForSeconds(2.0f);
    //    tweenTransform.to = points[2];
    //    tweenTransform.PlayForward();
    //    yield return new WaitForSeconds(2.0f);
    //    uiCamera.rect = new Rect(0.2f, 0.0f, 0.6f, 1.0f);
    //    StartCoroutine(CameraScaling(1.0f, 0.6f));
    //    tweenTransform.to = points[3];
    //    tweenTransform.PlayForward();
    //    yield return new WaitForSeconds(2.0f);
    //    tweenTransform.to = points[4];
    //    tweenTransform.PlayForward();
    //    yield return new WaitForSeconds(2.0f);
    //    tweenTransform.to = points[5];
    //    tweenTransform.PlayForward();
    //    yield return new WaitForSeconds(2.0f);
    //    uiCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
    //    StartCoroutine(CameraScaling(1.0f, 1.0f));
    //    tweenTransform.to = points[6];
    //    tweenTransform.PlayForward();
    //    cartoon.enabled = true;
    //}

    private IEnumerator CameraScaling(float _transitionTime, float _size)
    {
        float time = 0.0f;
        float size = 0.0f;
        while (time <= _transitionTime)
        {
            time += Time.deltaTime;
            size = Mathf.Lerp(camera.orthographicSize, _size, time * (1.0f / _transitionTime));
            camera.orthographicSize = size;
            yield return new WaitForFixedUpdate();
        }
    }
}