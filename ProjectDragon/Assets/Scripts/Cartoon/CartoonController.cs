
// ==============================================================
// Cartoon Controller
// Automatic Cartoon View
// 
// 2020-01-31: Add Button Interact Sound
//
//  AUTHOR: Kim Dong Ha
// CREATED: 2020-01-09
// UPDATED: 2020-01-31
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonController : MonoBehaviour
{
    public string cartoonName = string.Empty;
    public bool isCartoonEnd = false;
    public float cameraTranslateTime = 1.0f;
    public int cutCount;

    public CartoonData cartoonData;
    public Camera uiCamera;
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject skipButton;
    public GameObject[] cuts;

    public bool isTranslateCamera = false;
    public int currentCut = 0;
    public float screenX, screenY;

    public void CartoonLoad()
    {
        if(cartoonName.Equals(string.Empty))
        {
#if UNITY_EDITOR
            Debug.Log("cartoonName is null");
#endif      
            gameObject.SetActive(false);
            return;
        }

        GameObject cuts = Instantiate(Resources.Load("Cartoon/" + GameManager.Inst.Sex.ToString() + "/" + cartoonName), gameObject.transform) as GameObject;
        cartoonData = cuts.GetComponent<CartoonData>();
    }

    private void OnEnable()
    {
        if (gameObject.GetComponentsInChildren<Transform>().Length - 1 == 3)
        {
            CartoonLoad();
            if (gameObject.activeSelf.Equals(false)) return;
        }

        uiCamera = GameObject.FindGameObjectWithTag("ScreenTransitions").GetComponent<Camera>();
        cuts = cartoonData.cuts;
        cutCount = cartoonData.cutCount;

        screenX = GetComponent<UIWidget>().localSize.x;
        screenY = GetComponent<UIWidget>().localSize.y;
#if UNITY_EDITOR
        Debug.Log(screenX);
        Debug.Log(screenY);
#endif  
        //nextButton.GetComponent<UITexture>().SetAnchor(uiCamera.transform);
        //prevButton.GetComponent<UITexture>().SetAnchor(uiCamera.transform);
        nextButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        prevButton.transform.localPosition = cuts[currentCut].transform.localPosition - new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        skipButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 150.0f, screenY / 2 - 90.0f, 0.0f);
        prevButton.SetActive(false);
        uiCamera.transform.position = cuts[currentCut].transform.position;
    }

    private void OnDisable()
    {
        if (cartoonData != null) Destroy(cartoonData.gameObject);
    }

    public void ResizeButtonPostion()
    {
        nextButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        prevButton.transform.localPosition = cuts[currentCut].transform.localPosition - new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        skipButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 150.0f, screenY / 2 - 90.0f, 0.0f);
    }

    public void Button_NextCut()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        if (isTranslateCamera)
        {
            return;
        }

        currentCut++;
        if(cutCount <= currentCut)
        {
            currentCut = cutCount - 1;
            StartCoroutine(CartoonEnding());
        }
        else
        {
            StartCoroutine(TranslateCamera());
        }

        ResizeButtonPostion();

        if (0 != currentCut)
        {
            prevButton.SetActive(true);
        }
    }
    public void Button_Skip()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        StartCoroutine(CartoonEnding());
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
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        if (isTranslateCamera)
        {
            return;
        }

        currentCut--;
        if (0 >= currentCut)
        {
            currentCut = 0;
            prevButton.SetActive(false);
        }

        StartCoroutine(TranslateCamera());
        ResizeButtonPostion();
    }

    private IEnumerator TranslateCamera()
    {
        isTranslateCamera = true;
        float time = 0.0f;
        Vector3 pos;
        while (time <= cameraTranslateTime)
        {
            time += Time.deltaTime;
            pos = Vector3.Lerp(uiCamera.transform.position, cuts[currentCut].transform.position, time * (1 / cameraTranslateTime));
            uiCamera.transform.position = pos;
            yield return null;
        }
        isTranslateCamera = false;
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
