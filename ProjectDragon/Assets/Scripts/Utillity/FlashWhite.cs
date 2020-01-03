﻿/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-16////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashWhite : MonoBehaviour
{
    [SerializeField] float delay = 0.1f;
    Material originalMaterial;
    public Material effectMaterial;
    SpriteRenderer render;

    private void Awake()
    {
        render = this.GetComponent<SpriteRenderer>();
        originalMaterial = render.material;

    }

    public IEnumerator Flash()
    {
        if(this.gameObject== GameObject.FindGameObjectWithTag("Player"))
        {
            Debug.Log("플레이어 코루틴 진입");
        }
        render.material = effectMaterial;
        yield return new WaitForSeconds(delay);
        render.material = originalMaterial;
        yield return null;
    }


}
