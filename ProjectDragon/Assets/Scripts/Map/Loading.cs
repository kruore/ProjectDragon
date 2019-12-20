using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public string nextScene;
    public UILabel label;
    AsyncOperation asyncOperation;
    // Start is called before the first frame update
    public GameObject next_Button;

    private void Awake()
    {
        //다음 씬이 뭔지 판단해야 한다.
    }
    void Start()
    {
        next_Button = GameObject.Find("NextButton");
        StartCoroutine(LoadSceneAsync(nextScene));
    }

    IEnumerator LoadSceneAsync(string _sceneName)
    {
        yield return null;
        int percent;
        asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
        asyncOperation.allowSceneActivation = false;

        while(!asyncOperation.isDone)
        {
            yield return null;
            percent = Mathf.RoundToInt(asyncOperation.progress * 100.0f);
            label.text = percent.ToString() + "%";
            if(percent >= 90.0f)
            {
                yield return new WaitForSeconds(1.0f);
                label.text = "Next";
                next_Button.GetComponent<BoxCollider>().enabled = true;
                break;
            }
        }
    }

    public void NextButton()
    {
        asyncOperation.allowSceneActivation = true;
    }
}
