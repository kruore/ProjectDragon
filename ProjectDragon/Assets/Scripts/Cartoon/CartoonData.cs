﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonData : MonoBehaviour
{
    public int cutCount = 0;
    public GameObject[] cuts;

    private void Awake()
    {
        if (cutCount.Equals(0))
        {
            cutCount = gameObject.GetComponentsInChildren<UISprite>().Length;
#if UNITY_EDITOR
            Debug.Log(cutCount);
#endif    
        }
        cuts = new GameObject[cutCount];

        for (int i = 0; i < cutCount; i++)
        {
            cuts[i] = gameObject.transform.Find("Cut" + (i + 1)).gameObject;
        }
    }
}
