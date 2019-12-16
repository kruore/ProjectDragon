using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoSingleton<ObjectPool>
{
    [SerializeField]
    public List<SkillObjectPool> objectPool = new List<SkillObjectPool>();

    void Awake()
    {
      //ObjectPool.Inst.objectPool = new List<SkillObjectPool>();
        for (int ix = 0; ix < objectPool.Count; ++ix)
        {
            objectPool[ix].Initialize(transform);
        }
        
    }
    

    public bool PushToPool(string itemName, GameObject item, Transform parent = null)
    {
        SkillObjectPool pool = GetPoolItem(itemName);
        if (pool == null)
            return false;

        pool.PushSkill_IntoSkillPool(item, parent == null ? transform : parent);
        return true;
    }

    public GameObject PopFromPool(string itemName, Transform parent = null)
    {
        SkillObjectPool pool = GetPoolItem(itemName);
        if (pool == null)
            return null;

        return pool.PopSkill_GetOutSkillPool(parent);
    }

    SkillObjectPool GetPoolItem(string itemName)
    {
        for (int ix = 0; ix < objectPool.Count; ++ix)
        {
            if (objectPool[ix].SkillName.Equals(itemName))
                return objectPool[ix];
        }
        Debug.LogWarning("There's no matched pool list.");
        return null;
    }
}