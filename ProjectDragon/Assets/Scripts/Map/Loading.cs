
// ==============================================================
// Async Loader
// Load the map asynchronously.
//
// 2019-12-26: Add Loading Progress Bar
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-26
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public string sceneName = string.Empty;
    AsyncOperation asyncOperation;
    public UILabel TouchToStart;
    public bool changedRegion;

    //프로그레스 바
    public UIProgressBar loadingBar;
    public UILabel loadingLabel;

    //로딩 이미지 총개
    public int imageCount = 0;

    //지역 이동 연출 관련
    public GameObject regionObj;
    public TweenTransform player;
    public Transform[] points = new Transform[3];

    //페이드 아웃
    public ScreenTransitions screenTransitions;

    private void Awake()
    {
        sceneName = "Map_Generator";
        GameManager.Inst.CurrentStage++;

        changedRegion = (GameManager.Inst.CurrentStage % 4 == 1) ? true : false;

        Init();
    }

    private void Init()
    {
        if (imageCount != 0)
        {
            int rand = Random.Range(0, imageCount);
            GetComponent<UISprite>().spriteName = "bg" + (rand + 1);
        }
        loadingBar = GameObject.Find("ProgressBar").GetComponent<UIProgressBar>();
        loadingLabel = loadingBar.GetComponent<UILabel>();

        TouchToStart = GameObject.Find("TouchToStart").GetComponent<UILabel>();
        TouchToStart.gameObject.SetActive(false);

        screenTransitions = GameObject.FindGameObjectWithTag("Object").GetComponent<ScreenTransitions>();

        regionObj = GameObject.Find("Region");
        if (changedRegion)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<TweenTransform>();
            player.enabled = false;
            for (int i = 1; i <= 3; i++)
            {
                points[i - 1] = GameObject.Find("Point" + i).transform;
            }
        }
        else
        {
            regionObj.SetActive(false);
        }
    }

    void Start()
    {
        StartCoroutine(screenTransitions.Fade(0.5f, false));

        if (changedRegion)
        {
            StartCoroutine(RegionRepresentation());
        }
        StartCoroutine(LoadSceneAsync());
    }

    //지역 넘어가는 연출
    IEnumerator RegionRepresentation()
    {
        yield return new WaitForSeconds(2.0f);
        player.to = points[0];
        player.PlayForward();
        yield return new WaitForSeconds(2.0f);
        player.to = points[1];
        player.PlayForward();
        yield return new WaitForSeconds(1.0f);
        player.to = points[2];
        player.PlayForward();
        yield return new WaitForSeconds(1.0f);
        regionObj.GetComponent<TweenAlpha>().enabled = true;
    }

    //AsyncLoad
    IEnumerator LoadSceneAsync()
    {
        //CalculateSceneName();

        yield return null;

        float percent = 0.0f;
        ProgressBar(percent);
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            percent = asyncOperation.progress;
            ProgressBar(percent);
            yield return null;

            if (percent >= 0.9f)
            {
                ProgressBar(percent);
                yield return new WaitForSeconds(1.0f);
                //Load Complete
                ProgressBar(1.0f);
                TouchToStart.gameObject.SetActive(true);
                break;
            }
        }
    }

    void ProgressBar(float _percent)
    {
        loadingBar.value = _percent;
        _percent *= 100.0f;
        loadingLabel.text = "Loading..." + _percent + "%";
    }

    public void Button_NextScene()
    {
        StartCoroutine(screenTransitions.Fade(1.0f, true));
        Invoke("NextScene", 1.1f);
    }

    public void NextScene()
    {
        asyncOperation.allowSceneActivation = true;
    }
}
