using UnityEditor;
using UnityEngine;

public class NGUIApplyFont
{

    static void OnDynamicFont(Object obj)
    {
        foreach (GameObject o in Object.FindObjectsOfType(typeof(GameObject)))
        {
            UILabel label = o.GetComponent<UILabel>();
            if (null == label)
            {
                continue;
            }

            label.trueTypeFont = (Font)obj;
        }
    }
}