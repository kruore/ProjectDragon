using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EuipmentcellData : IReuseCellData
{
    #region CellData
    public int m_Index;
    public int Index { get { return m_Index; } set { m_Index = value; } }
    public int m_DB_Num;
    public int DB_Num { get { return m_DB_Num; } set { m_DB_Num = value; } }
    public string m_name;
    public string name { get { return m_name; } set { m_name = value; } }
    public int m_itemValue;
    public int itemValue { get { return m_itemValue; } set { m_itemValue = value; } }
    public RARITY m_rarity;
    public RARITY rarity { get { return m_rarity; } set { m_rarity = value; } }
    public Item_CLASS m_item_Class;
    public Item_CLASS item_Class { get { return m_item_Class; } set { m_item_Class = value; } }
    public int m_upgrade_Level;
    public int upgrade_Level { get { return m_upgrade_Level; } set { m_upgrade_Level = value; } }
    public int m_upgrade_Count;
    public int upgrade_Count { get { return m_upgrade_Count; } set { m_upgrade_Count = value; } }
    public string m_imageName;
    public string imageName { get { return m_imageName; } set { m_imageName = value; } }
    public int m_amount;
    public int amount { get { return m_amount; } set { m_amount = value; } }
    public bool m_isEquipment;
    public bool isEquipment { get { return m_isEquipment; } set { m_isEquipment = value; } }
    #endregion
}
