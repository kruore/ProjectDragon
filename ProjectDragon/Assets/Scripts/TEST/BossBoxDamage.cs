using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossBoxDamage : MonoBehaviour
{
    public float ResetSpeed;
    public float AttackReadySpeed;
    public float AttackSpeed;
    public int AttackPoint;
    bool AttackAnim;
    private float m_Currenttime;
    [SerializeField]
    GameObject player;
    BoxCollider2D bcollider2D;
    TweenPosition childTween;
    public GameObject child;
    // Start is called before the first frame update
    void Start()
    {
        bcollider2D = gameObject.AddComponent<BoxCollider2D>();
        bcollider2D.autoTiling = true;
        bcollider2D.isTrigger = true;
        child = gameObject.transform.Find("팔").gameObject;
        GetComponent<Animator>().SetFloat("AttackReadySpeed", AttackReadySpeed);
        GetComponent<Animator>().SetFloat("AttackSpeed", AttackSpeed);
        GetComponent<Animator>().SetFloat("ResetSpeed", ResetSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //// if (Attack)
        //{
        //    currenttime += Time.deltaTime;
        //    if (!timecheck)
        //    {
        //        Attack =ArmState.Attack;
        //        timecheck = true;
        //    }
        //}
    }
    public void HitPlayer()
    {
        //플레이어
        Debug.Log(player);
        if (!(player==null))
        {
            player.GetComponent<Player>().HPChanged(AttackPoint,false,0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            player = null;
        }
    }
    public void Attackfalse()
    {

    }
}
