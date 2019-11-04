using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string poolItemName = "ProjectileObj";

    public bool inited, isplayskill;
    public int damage;
    public float m_angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;
        }
    }
    public float angle, speed, lifetime, generationtime, targetpointrangex, targetpointrangey;

    Rigidbody2D rb2d;
    Animator anim;
    Projectile projectile;

    void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    public IEnumerator Reset;

    private void FixedUpdate()
    {
        if (inited)
        {
            if (generationtime < 3)
            {
                generationtime += Time.deltaTime;
            }
            else
            {
                StartCoroutine(Reset);
            }
        }
    }



    //Create Projectile 
    public Projectile Create(float _angle, float _speed, int _damage, RuntimeAnimatorController _projectileAnimator, string poolItemName, bool _isplayskill, Vector3 position, Transform parent = null)
    {

        GameObject projectileObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        projectile = projectileObject.transform.GetComponent<Projectile>();
        projectile.gameObject.SetActive(true);
        projectile.ProjectileInit(_angle, _speed, _damage, _projectileAnimator, _isplayskill, position);
        return projectile;

        //ObjectPool.Instance.PushToPool("ProjectileObj", projectileObject);

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
    public void ProjectileInit(float _angle, float _speed, int _damage, RuntimeAnimatorController _projectileAnimator, bool _isplayskill, Vector3 position)
    {
        Reset = ResetProjectile();
        inited = true;
        gameObject.transform.position = position;
        isplayskill = _isplayskill;
        m_angle = _angle;
        speed = _speed;
        damage = _damage;
        if (anim == null)
        {
            anim = gameObject.GetComponent<Animator>();
        }
        if (rb2d == null)
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }
        anim.runtimeAnimatorController = _projectileAnimator;
        anim.Play("ProjecTileTest");
        anim.SetBool("Destory", false);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, m_angle));

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(-Mathf.Cos((m_angle - 90) / 360 * 2 * Mathf.PI) * speed, -Mathf.Sin((m_angle - 90) / 360 * 2 * Mathf.PI) * speed);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Wall"))
        {
            StartCoroutine(Reset);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.GetComponent<Player>().HPChanged(damage);
            if (Reset != null)
            {
                StartCoroutine(Reset);
                Reset = null;

            }
        }
        if (collision.tag.Equals("Wall"))
        {
            StartCoroutine(Reset);
            Reset = null;
        }
        

    }
    //private void OnBecameInvisible()
    //{
    //    if (gameObject.GetComponent<CircleCollider2D>().enabled)
    //    {
    //        if (Reset != null)
    //        {
    //            StartCoroutine(Reset);
    //            Reset = null;
    //        }

    //    }
    //}
    IEnumerator ResetProjectile()
    {
        generationtime = 0;
        float cliptime = 0;
        anim.SetBool("Destory", true);
        inited = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("ProjectileDestroy"))
        {
            yield return null;
        }
        cliptime = anim.GetCurrentAnimatorStateInfo(0).length;
        rb2d.velocity = Vector3.zero;
        //Debug.Log(cliptime);
        yield return new WaitForSecondsRealtime(cliptime);
        //Push ObjectPoolList
        ObjectPool.Instance.PushToPool(poolItemName, gameObject, transform);
        yield return null;
    }
}