using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public enum Size
    {
        Small,
        Middle
    }
    public enum State
    {
        Phase1,
        Phase2,
        Destroy
    }
    public float hp = 100;
    public Size rootSize = Size.Small;
    public State rootState = State.Phase1;
    public Sprite crackedRoot;

    private bool isSpriteChange = false;
    private float halfHP;

    private void Awake()
    {
        halfHP = hp / 2.0f;
        if (crackedRoot == null)
        {
            ResourceLoad();
        }
    }

    private void ResourceLoad()
    {
        string resourceName = "Object/Sprite/";
        switch (rootSize)
        {
            case Size.Small:
                resourceName += "crackedRoot_Small";
                break;
            case Size.Middle:
                resourceName += "crackedRoot_Middle";
                break;
        }
        crackedRoot = Resources.Load<Sprite>(resourceName);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Skill"))
        {
            hp -= 50.0f;

            if (hp <= halfHP && rootState.Equals(State.Phase1))
            {
                ChangeSprite();
            }
            else if (hp <= 0.0f && rootState.Equals(State.Phase2))
            {
                rootState = State.Destroy;
                StartCoroutine(Effect());
            }
        }
    }

    private void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = crackedRoot;
        isSpriteChange = true;
        rootState = State.Phase2;
    }

    IEnumerator Effect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
