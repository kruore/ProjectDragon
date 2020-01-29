
// ==============================================================
// Cartoon Controller
// Automatic Cartoon View
// 
//
//  AUTHOR: Kim Dong Ha
// CREATED: 2020-01-09
// UPDATED: 
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonController : MonoBehaviour
{
    public string cartoonName;
    public bool isCartoonEnd = false;
    public float cameraTranslateTime = 1.0f;
    public int cutCount;

    public CartoonData cartoonData;
    public Camera uiCamera;
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject[] cuts;

    public int currentCut = 0;
    public float screenX;

    private void Awake()
    {
        
    }

    public void CartoonLoad()
    {
        if(cartoonName == null)
        {
#if UNITY_EDITOR
            Debug.Log("cartoonName is null");
#endif      
            gameObject.SetActive(false);
            return;
        }

        GameObject cuts = Instantiate(Resources.Load("Cartoon/" + cartoonName), gameObject.transform) as GameObject;
#if UNITY_EDITOR
        Debug.Log(cuts.name);
#endif      
    }
    private void Start()
    {
        uiCamera = GameObject.FindGameObjectWithTag("ScreenTransitions").GetComponent<Camera>();
        cuts = new GameObject[6];
        for (int i = 0; i < cutCount; i++)
        {
            cuts[i] = gameObject.transform.Find("Cut" + (i + 1)).gameObject;
        }

        screenX = GetComponent<UIWidget>().localSize.x;
        //nextButton.GetComponent<UITexture>().SetAnchor(uiCamera.transform);
        //prevButton.GetComponent<UITexture>().SetAnchor(uiCamera.transform);
        nextButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        prevButton.transform.localPosition = cuts[currentCut].transform.localPosition - new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        prevButton.SetActive(false);
        uiCamera.transform.position = cuts[currentCut].transform.position;
    }

    public void ResizeButtonPostion()
    {
        nextButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        prevButton.transform.localPosition = cuts[currentCut].transform.localPosition - new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
    }

    public void Button_NextCut()
    {
        currentCut++;
        if(cutCount <= currentCut)
        {
            currentCut = cutCount;
            StartCoroutine(CartoonEnding());
        }
        else
        {
            StartCoroutine(TranslateCamera());
        }

        if(0 != currentCut)
        {
            prevButton.SetActive(true);
        }
    }
    private IEnumerator CartoonEnding()
    {
        StartCoroutine(uiCamera.GetComponent<ScreenTransitions>().Fade(2.0f, true));
        yield return new WaitForSeconds(2.0f);
        isCartoonEnd = true;
        gameObject.SetActive(false);
    }

    public void Button_PreCut()
    {
        currentCut--;
        if (0 >= currentCut)
        {
            currentCut = 0;
            prevButton.SetActive(false);
        }
        else
        {
            StartCoroutine(TranslateCamera());
        }
    }

    private IEnumerator TranslateCamera()
    {
        float time = 0.0f;
        Vector3 pos;
        while (time <= cameraTranslateTime)
        {
            time += Time.deltaTime;
            pos = Vector3.Lerp(uiCamera.transform.position, cuts[currentCut].transform.position, time * (1 / cameraTranslateTime));
            uiCamera.transform.position = pos;
            yield return null;
        }
    }

    private IEnumerator CameraScaling(float _size)
    {
        float time = 0.0f;
        float size = 0.0f;
        while (time <= cameraTranslateTime)
        {
            time += Time.deltaTime;
            size = Mathf.Lerp(uiCamera.orthographicSize, _size, time * (1.0f / cameraTranslateTime));
            uiCamera.orthographicSize = size;
            yield return new WaitForFixedUpdate();
        }
    }
}
