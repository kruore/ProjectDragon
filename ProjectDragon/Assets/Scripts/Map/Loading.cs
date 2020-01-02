﻿
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
    public UILabel label;
    public GameObject nextButton;

    //프로그레스 바
    public UIProgressBar loadingBar;
    public UILabel loadingLabel;

    //로딩창 이미지
    public Sprite[] bg_image;

    //지역 이동 연출 관련
    public GameObject regionObj;
    public TweenTransform player;
    public Transform[] points = new Transform[3];

    private void Awake()
    {
        sceneName = "Map_Generator";
        ResourceLoad();

        loadingBar = GameObject.Find("ProgressBar").GetComponent<UIProgressBar>();
        loadingLabel = loadingBar.GetComponent<UILabel>();

        nextButton = GameObject.Find("NextButton");
        nextButton.GetComponent<BoxCollider>().enabled = false;

        regionObj = GameObject.Find("Region");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TweenTransform>();
        //player.enabled = false;

        for(int i = 1; i <= 3; i++)
        {
            points[i - 1] = GameObject.Find("Point" + i).transform;
        }
    }

    void ResourceLoad()
    {
        bg_image = Resources.LoadAll<Sprite>("Loading");
    }

    void Start()
    {
        StartCoroutine(RegionRepresentation());
        //StartCoroutine(LoadSceneAsync());
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
        yield return new WaitForSeconds(2.0f);
        player.to = points[2];
        player.PlayForward();
    }

    void CalculateSceneName()
    {
#if UNITY_EDITOR
        Debug.Log(GameManager.Inst.CurrentStage % 4);
#endif
        if (GameManager.Inst.CurrentStage % 4 == 0)
        {
            int region = GameManager.Inst.CurrentStage / 4;
#if UNITY_EDITOR
            Debug.Log(region);
#endif
            switch (region)
            {
                case 1:
                    sceneName = "MaDongSeok";
                    break;
                case 2:
                    sceneName = "MaDongSeok";
                    break;
                case 3:
                    sceneName = "MaDongSeok";
                    break;
                case 4:
                    sceneName = "MaDongSeok";
                    break;
            }
        }
        else
        {
            sceneName = "Map_Generator";
        }
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
                nextButton.GetComponent<BoxCollider>().enabled = true;
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
        asyncOperation.allowSceneActivation = true;
    }
}
