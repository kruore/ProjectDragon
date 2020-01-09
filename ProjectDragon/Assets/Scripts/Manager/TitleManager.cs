using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    #region 민석이형꺼
    //public UILabel test;
    //public List<GameObject> cameraset;
    //Vector3 Uicamera = new Vector3(8, 1.5f, 0);


    //// Start is called before the first frame update
    //void Start()
    //{
    //    GameManager.Inst.ScreensizeReadjust();
    //    DataTransaction.Inst.ToString();
    //    //test.text = Database.Inst.playData.inventory.Count.ToString();
    //}

    //private void Update()
    //{
    //    //카메라를타겟으로
    //    //transform.position =
    //    //Vector3.MoveTowards(transform.position, target, 2f);

    //    //Vector3 velo = Vector3.zero;

    //    //transform.position =
    //    //Vector3.SmoothDamp(transform.position, target, ref velo, 0.1f);

    //    transform.position =
    //        Vector3.Lerp(transform.position, Uicamera, 0.1f);
    //}

    //float speed = 20.0f;

    //private void FixedUpdate()
    //{
    //    float h = Input.GetAxis("Horizontal");
    //    float v = Input.GetAxis("Vertical");

    //    h = h * speed * Time.deltaTime;
    //    v = v * speed * Time.deltaTime;

    //    transform.Translate(Vector3.right * h);
    //    transform.Translate(Vector3.forward * v);
    //}

    //public void GotoLobby()
    //{
    //    ButtonManager.GotoLobby();
    //}

    //public void ButtonClick()
    //{
    //    Debug.Log("다음 씬으로 이동합니다.");
    //    Application.LoadLevel("넘어갈 씬??");

    //}

    //public void Open() { gameObject.SetActive(true); }
    //public void Close() { gameObject.SetActive(false); }
    //public void buttonclose()
    //{
    //    UIButton.current.gameObject.SetActive(false);
    //}
    //public void Objectclose()
    //{
    //    GameObject.Find("UI Root/");
    //}
    //public void ObjectControl()
    //{
    //    ButtonManager.ObjectlistControl();
    //}
    ////public void aa()
    ////{
    ////	Database.Inst.playData.sex=SEX.
    ////}
    ////  public void bb()
    ////{
    ////	Database.Inst.playData.inventory[0].;
    ////	DataTransaction.Inst.SavePlayerData()
    ////}

    #endregion

    //Database
    public SEX sex;
    public CLASS Item_Class;

    //만화 연출용
    public int cutCount = 0;
    public GameObject nextCutButton;
    public Transform[] points;
    public Camera uiCamera;
    public TweenAlpha cartoon;
    public TweenTransform tweenTransform;

    //로그인
    public TweenAlpha loginBG;

    private void Awake()
    {
        Cartoon();
        loginBG = GameObject.Find("LogInScene").GetComponent<TweenAlpha>();
    }
    private void Start() {
        SoundManager.Inst.Ds_BGMPlayerDB(1);
    }
    private void Cartoon()
    {
        cartoon = GameObject.Find("CartoonBG").GetComponent<TweenAlpha>();
        cartoon.enabled = false;
        nextCutButton = GameObject.Find("NextCutButton");
        uiCamera = GameObject.Find("Camera").GetComponent<Camera>();
        tweenTransform = uiCamera.GetComponent<TweenTransform>();

        tweenTransform.from = uiCamera.transform;

        //StartCoroutine(CartoonTransition());
    }

    public void Button_NextCut()
    {
        nextCutButton.SetActive(false);
        StartCoroutine(translate(cutCount));
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        cutCount++;
    }
    public void Button_LogIn()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        //구글 로그인 넣는 곳
        loginBG.enabled = true;
    }
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
    private IEnumerator translate(int _cutCount)
    {
        switch (_cutCount)
        {
            case 0:
                uiCamera.rect = new Rect(0.15f, 0.0f, 0.7f, 1.0f);
                StartCoroutine(CameraScaling(1.0f, 0.5f));
                tweenTransform.to = points[0];
                tweenTransform.PlayForward();
                break;
            case 1:
                tweenTransform.to = points[1];
                tweenTransform.PlayForward();
                break;
            case 2:
                tweenTransform.to = points[2];
                tweenTransform.PlayForward();
                break;
            case 3:
                uiCamera.rect = new Rect(0.2f, 0.0f, 0.6f, 1.0f);
                StartCoroutine(CameraScaling(1.0f, 0.6f));
                tweenTransform.to = points[3];
                tweenTransform.PlayForward();
                break;
            case 4:
                tweenTransform.to = points[4];
                tweenTransform.PlayForward();
                break;
            case 5:
                tweenTransform.to = points[5];
                tweenTransform.PlayForward();
                break;
            case 6:
                uiCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                StartCoroutine(CameraScaling(1.0f, 1.0f));
                tweenTransform.to = points[6];
                tweenTransform.PlayForward();
                break;
            case 7:
                cartoon.enabled = true;
                break;
        }
        yield return new WaitForSeconds(1.2f);

        if (_cutCount != 7) nextCutButton.SetActive(true);
    }

    private IEnumerator CartoonTransition()
    {
        yield return new WaitForSeconds(1.0f);
        uiCamera.rect = new Rect(0.15f, 0.0f, 0.7f, 1.0f);
        StartCoroutine(CameraScaling(1.0f, 0.5f));
        tweenTransform.to = points[0];
        tweenTransform.PlayForward();
        yield return new WaitForSeconds(2.0f);
        tweenTransform.to = points[1];
        tweenTransform.PlayForward();
        yield return new WaitForSeconds(2.0f);
        tweenTransform.to = points[2];
        tweenTransform.PlayForward();
        yield return new WaitForSeconds(2.0f);
        uiCamera.rect = new Rect(0.2f, 0.0f, 0.6f, 1.0f);
        StartCoroutine(CameraScaling(1.0f, 0.6f));
        tweenTransform.to = points[3];
        tweenTransform.PlayForward();
        yield return new WaitForSeconds(2.0f);
        tweenTransform.to = points[4];
        tweenTransform.PlayForward();
        yield return new WaitForSeconds(2.0f);
        tweenTransform.to = points[5];
        tweenTransform.PlayForward();
        yield return new WaitForSeconds(2.0f);
        uiCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        StartCoroutine(CameraScaling(1.0f, 1.0f));
        tweenTransform.to = points[6];
        tweenTransform.PlayForward();
        cartoon.enabled = true;
    }

    private IEnumerator CameraScaling(float _transitionTime, float _size)
    {
        float time = 0.0f;
        float size = 0.0f;
        while (time <= _transitionTime)
        {
            time += Time.deltaTime;
            size = Mathf.Lerp(uiCamera.orthographicSize, _size, time * (1.0f / _transitionTime));
            uiCamera.orthographicSize = size;
            yield return new WaitForFixedUpdate();
        }
    }
}