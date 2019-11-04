using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public float itemDropPercentage = 0.0f;
    public float hp = 1;
    public GameObject party;
    private void Awake()
    {
        party = GetComponentInChildren<ParticleSystem>().gameObject;
        party.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Player"))
        {
            hp--;
            if (hp < 1)
            {
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
