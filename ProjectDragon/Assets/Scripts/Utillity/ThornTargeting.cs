using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornTargeting : MonoBehaviour
{
    int damage;
    Vector3 targetPosition;
    Animator anim;
    public string poolItemName = "ThornTargeting";

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ThornTargeting Create(int _damage, string poolItemName, Vector3 position, Transform parent = null)
    {

        GameObject TargetingObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        ThornTargeting targeting = TargetingObject.transform.GetComponent<ThornTargeting>();
        targeting.gameObject.SetActive(true);
        targeting.Init(_damage, position);
        return targeting;
    }

    private void Init(int _damage, Vector3 _position)
    {
        damage = _damage;
        transform.position = _position;
        anim.Play("ThornTargetIlde");

    }
}
