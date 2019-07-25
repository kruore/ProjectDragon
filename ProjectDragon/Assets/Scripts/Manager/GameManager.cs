using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public string loadingnextscene, AsyncLoadSceneName;
    public Stack<string> Scenestack = new Stack<string>();

    public void singletonF()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;

        Screen.autorotateToPortrait = false;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = true;

        Screen.autorotateToLandscapeRight = true;
    }
    
    void Update()
    {
        if (Application.platform.Equals(RuntimePlatform.Android))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ButtonManager.TouchBackButton();
            }
        }
    }
}
