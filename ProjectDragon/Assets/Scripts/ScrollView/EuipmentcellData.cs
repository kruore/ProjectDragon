using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EuipmentcellData : IReuseCellData
{
    #region CellData
    public int m_Index;
    public int Index
    {
        get
        {
            return m_Index;
        }
        set
        {
            m_Index = value;
        }
    }
    public int m_enchantlevel;
    public int enchantlevel
    {
        get
        {
            return m_enchantlevel;
        }
        set
        {
            m_enchantlevel = value;
        }
    }
    public string m_name;
    public string name
    {
        get
        {
            return m_name;
        }
        set
        {
            m_name = value;
        }
    }
    public int m_power;
    public int power
    {
        get
        {
            return m_power;
        }
        set
        {
            m_power = value;
        }
    }
    public bool m_equip;
    public bool equip
    {
        get
        {
            return m_equip;
        }
        set
        {
            m_equip = value;
        }
    }
    #endregion

    public string ImgName;
}
