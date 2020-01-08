// ==============================================================
// Cracked ThornPoint
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-01-08
// UPDATED: 2020-01-08
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornPoint : MonoBehaviour
{
    public string poolItemName = "ThornPoint";
    public float projecTileReady, projecTileStart, projecTileEnd;
    ThornPoint thornPoint;
    int attackDamage = 0;

    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("ReadyTime", projecTileReady);
        GetComponent<Animator>().SetFloat("StartTime", projecTileStart);
        GetComponent<Animator>().SetFloat("EndTime", projecTileEnd);
    }
    /// <summary>
    /// 애니메이션 이벤트 함수
    /// </summary>
    public void AttackOn()
    {
        if ((player != null))
        {
            player.GetComponent<Player>().HPChanged(attackDamage,false,0);
        }
    }
    /// <summary>
    /// 애니메이션 이벤트 함수
    /// </summary>
    public void ResetProjectile()
    {
        ObjectPool.Instance.PushToPool(poolItemName, gameObject);
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

    

    public ThornPoint Create(int _damage, RuntimeAnimatorController _Animator, string poolItemName, Vector3 position, Transform parent = null)
    {

        GameObject projectileObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        thornPoint = projectileObject.transform.GetComponent<ThornPoint>();
        thornPoint.attackDamage = _damage;
        thornPoint.transform.position = position;
        thornPoint.GetComponent<Animator>().runtimeAnimatorController = _Animator;
        thornPoint.gameObject.SetActive(true);
        thornPoint.GetComponent<Animator>().Play("ProjecTileReady");
        return thornPoint;

    }

}
