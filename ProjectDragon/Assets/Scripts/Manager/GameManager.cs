using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public string loadingnextscene, AsyncLoadSceneName;
    public GameObject QuitObject;
    bool quit;
    float quittime;
    public Stack<string> Scenestack = new Stack<string>();

    private void Awake()
    {
        ScreensizeReadjust();
    }
    private void OnApplicationPause(bool pause)
    {
        ScreensizeReadjust();
    }
    public void ScreensizeReadjust()
    {
        Screen.SetResolution(Screen.width * 16 / 9, Screen.width, false);

        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.fullScreen = true;
        Screen.autorotateToPortrait = false;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = true;

        Screen.autorotateToLandscapeRight = true;

        Camera.main.backgroundColor = Color.black;
        Camera camera = Camera.main;

        Rect rect = camera.rect;
        //float scaleheight = ((float)Screen.height / (float)Screen.width) / ((float)16 / (float)9);
        //float scalewidth = 1f / scaleheight;
        //if (scaleheight < 1)
        //{
        //    rect.height = scaleheight;
        //    rect.y = (1f - scaleheight) / 2f;
        //}
        //else
        //{
        //    rect.width = scalewidth;
        //    rect.x = (1f - scalewidth) / 2f;
        //}
        //camera.rect = rect;
    }

    void Update()
    {
        if (Application.platform.Equals(RuntimePlatform.Android))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (0.2f+quittime>Time.time)
                {
                    QuitObject.SetActive(true);
                }
                else
                {
                    ButtonManager.TouchBackButton();
                    quittime = Time.time;
                }
            }
        }
    }
}
