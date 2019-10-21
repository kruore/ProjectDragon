using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public bool inited, isplayskill;
    public int damage;
    public float angle, speed, lifetime, generationtime;
    public string projectilename;
    Rigidbody2D rb2d;
    Animator anim;
    Vector2 PiSet;

    Vector3 _Pos;

    private void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.position = gameObject.transform.parent.position;
            
    }
    private void FixedUpdate()
    {
        if (inited)
        {
            generationtime += Time.deltaTime;
        }
    }
    /// <summary>
    /// 투사체 초기화
    /// </summary>
    /// <param name="_angle"> 투사체 발사각도 위를 기준 0도</param>
    /// <param name="_speed">투사체의 이동 속도</param>
    /// <param name="_damage">투사체의 데미지</param>
    /// <param name="_projectilename">투사체의 이름</param>
    /// <param name="_isplayskill">쏘는이 판별여부(if플레이어=true)</param>
    /// <param name="position">쏘아지는 위치</param>
    public void ProjectileInit(float _angle, float _speed, int _damage, string _projectilename, bool _isplayskill, Vector3 position)
    {
        inited = true;
        _Pos = position;
        gameObject.transform.position = _Pos;
        isplayskill = _isplayskill;
        angle = _angle;
        speed = _speed;
        damage = _damage;
        projectilename = _projectilename;
        anim.Play("ProjecTileTest");
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        anim.SetBool("Destory", false);
        rb2d.velocity = new Vector2(Mathf.Cos((-angle + 90) / 360 * 2 * Mathf.PI) *  speed, Mathf.Sin((-angle + 90) / 360 * 2 * Mathf.PI) * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag.Equals("Player"))
        {
            collision.GetComponent<Player>().HPChanged(damage);
            StartCoroutine(ResetProjectile());
        }
        //else
        //{
        //    if(!collision.tag.Equals("Enemy")||!collision.tag.Equals("Projectile"))
        //    {
        //        StartCoroutine(ResetProjectile());
        //    }
        //}

    }
    private void OnBecameInvisible()
    {
        //if (gameObject.GetComponent<CircleCollider2D>().enabled)
        //{
        //    StartCoroutine(ResetProjectile());
        //    Debug.Log("안보여");
        //}
    }
    IEnumerator ResetProjectile()
    {
        float cliptime=0;
       anim.SetBool("Destory", true);
        inited = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if(clip.name.Equals("ProjectileDestroy"))
            {
                cliptime = clip.length;
            }
        }
        rb2d.velocity = Vector3.zero;
       yield return new WaitForSecondsRealtime(cliptime);
            gameObject.transform.position = _Pos;  //gameObject.transform.parent.position
        yield return null;
    }
}
