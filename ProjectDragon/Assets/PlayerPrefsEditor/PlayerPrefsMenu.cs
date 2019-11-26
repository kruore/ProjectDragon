using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsMenu : MonoBehaviour
{
    [MenuItem("PlayerPrefs/DeleteAll")]
    static public void PlayerPrefsDeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

   
}
