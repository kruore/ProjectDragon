﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public float itemDropPercentage = 0.0f;
    public float hp = 1;
    public GameObject party;
    public t_Grid astar;

    private void Awake()
    {
        //Room room = GameObject.FindGameObjectWithTag("RoomManager").GetComponentInChildren<Room>();
        
        //astar = transform.parent.GetComponentInChildren<t_Grid>();
        party = GetComponentInChildren<ParticleSystem>().gameObject;
        party.SetActive(false);
    }
    private void Start()
    {
        astar = transform.parent.GetComponentInChildren<t_Grid>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Player"))
        {
            hp--;
            if (hp < 1)
            {
                astar.RescanPath(GetComponent<BoxCollider2D>());
                StartCoroutine(Effect());
                DropItem();

            }
        }
    }

    //public void HPChanged(float _damage)
    //{
    //    hp--;
    //    if (hp < 1)
    //    {
    //        StartCoroutine(Effect());
    //        DropItem();
    //    }
    //}

    IEnumerator Effect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        party.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    void DropItem()
    {
        float rand = Random.Range(0.0f, 99.9f);

        if(itemDropPercentage > rand)
        {
            //아이템 생성
        }
    }
}
