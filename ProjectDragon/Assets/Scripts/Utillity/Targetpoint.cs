using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//특정 지점에 타격을 가할 때 생성할 투사체 목적지(데미지의 주체)
public class Targetpoint : MonoBehaviour
{
    public string poolItemName = "TargetPointObj";
    public float projecTileReady, projecTileStart, projecTileEnd;
    Targetpoint targetpointobj;

    [SerializeField]
    GameObject player;
    public int AttackPoint;
    public ParticleSystem[] explosion;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("ReadyTime", projecTileReady);
        GetComponent<Animator>().SetFloat("StartTime", projecTileStart);
        GetComponent<Animator>().SetFloat("EndTime", projecTileEnd);
        explosion = GetComponentsInChildren<ParticleSystem>();
    }
    void Explosion()
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i].Play();
        }
    }
    public void ProjecTileEnd()
    {
        Debug.Log(player);
        if (!(player == null))
        {
            player.GetComponent<Player>().HPChanged(AttackPoint);
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
   

    public void ResetProjectile()
    {
        ObjectPool.Instance.PushToPool(poolItemName, gameObject, transform);
    }
    public Targetpoint Create(float _speed, int _damage, RuntimeAnimatorController _Animator, string poolItemName, Vector3 position, Transform parent = null)
    {

        GameObject projectileObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        targetpointobj = projectileObject.transform.GetComponent<Targetpoint>();
        targetpointobj.transform.position = position;
        GetComponent<Animator>().runtimeAnimatorController = _Animator;
        targetpointobj.gameObject.SetActive(true);
        targetpointobj.GetComponent<Animator>().Play("ProjecTileReady");
        return targetpointobj;

        //ObjectPool.Instance.PushToPool("ProjectileObj", projectileObject);

    }
}
