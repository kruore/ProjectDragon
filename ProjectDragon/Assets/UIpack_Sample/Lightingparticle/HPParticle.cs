using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPParticle : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int maxHP = collision.GetComponent<Character>().maxHp;
            collision.GetComponent<Character>().HPChanged(-(maxHP - collision.GetComponent<Character>().HP),false,0);
            gameObject.SetActive(false);
        }
    }
}
