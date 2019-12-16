
// ==============================================================
// Editor: PlayerPref method
// 
//  AUTHOR: Kim Dong Ha
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrefsMenu : MonoBehaviour
{
    //[MenuItem("PlayerPrefs/DeleteAll")]
    static public void PlayerPrefsDeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

   
}
