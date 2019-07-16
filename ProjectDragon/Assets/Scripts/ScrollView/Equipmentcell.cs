using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipmentcell : UIReuseScrollViewCell
{
    public UISprite EquipIcon;
    public UISprite enchantlevel;
    public override void UpdateData(IReuseCellData CellData)
    {
        EuipmentcellData item = CellData as EuipmentcellData;
        if(item.m_enchantlevel>0)
        {
            
        }
        if(item.m_equip)
        {
            EquipIcon=LobbyManager.inst.Equipobject[0].GetComponent<UISprite>();
            EquipIcon.transform.parent = gameObject.transform;
            EquipIcon.transform.localPosition = Vector3.zero;
        }
        if(CellData.name==null)
        EquipIcon.spriteName = "Box6";
        if (item == null)
            
            return;
    }

}
