﻿
// ==============================================================
// Npc Behavior
//
// 2019-12-31: implementation Npc Animation
//
//  AUTHOR: Kim Dong Ha
// CREATED: 2019-12-31
// UPDATED: 2019-12-31
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBehavior : MonoBehaviour
{
    public float aniWait_Min = 1.0f, aniWait_Max = 2.0f;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        float waitTime = 0.0f;
        if (aniWait_Min < 1.0f) aniWait_Min = 1.0f;
        if (aniWait_Max < 1.5f) aniWait_Max = 1.5f;

        while (true)
        {
            waitTime = Random.Range(aniWait_Min, aniWait_Max);
            yield return new WaitForSeconds(waitTime);
            animator.Play("Npc_Trader_Idle");
            Debug.Log("asdfasdf");
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(Animation());
    }
}
