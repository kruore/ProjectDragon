using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EuipmentcellData : IReuseCellData
{
    #region CellData
    public int m_index;
    public int Index { get { return m_index; } set { m_index = value; } }
    public int m_inventoryNum;
    public int inventoryNum { get { return m_inventoryNum; } set { m_inventoryNum = value; } }
    public int m_DB_Num;
    public int DB_Num { get { return m_DB_Num; } set { m_DB_Num = value; } }
    public string m_name;
    public string name { get { return m_name; } set { m_name = value; } }
    public float m_stat;
    public float stat{ get { return m_stat; } set { m_stat = value; } }
    public bool m_isLock;
    public bool isLock { get { return m_isLock; }set { m_isLock = value; } }
    public int m_itemValue;
    public int itemValue { get { return m_itemValue; } set { m_itemValue = value; } }
    public RARITY m_rarity;
    public RARITY rarity { get { return m_rarity; } set { m_rarity = value; } }
    public Item_CLASS m_item_Class;
    public Item_CLASS item_Class { get { return m_item_Class; } set { m_item_Class = value; } }
    public string m_imageName;
    public string imageName { get { return m_imageName; } set { m_imageName = value; } }
    public int m_amount;
    public int amount { get { return m_amount; } set { m_amount = value; } }
    public int m_skill_index;
    public int skill_index { get { return m_skill_index; } set { m_skill_index = value; } }
    #endregion
}
