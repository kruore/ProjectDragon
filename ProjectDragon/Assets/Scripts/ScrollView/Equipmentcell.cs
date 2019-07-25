using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipmentcell : UIReuseScrollViewCell
{
    public UISprite equipIcon, itemIcon;
    public UISprite enchantlevel;
    public EuipmentcellData cell;
    public override void UpdateData(IReuseCellData _CellData)
    {
        EuipmentcellData item = _CellData as EuipmentcellData;
        cell = item;
        enchantlevel.spriteName = string.Format("강화수치_{0}", item.upgrade_Level.ToString());
        if (item.isEquipment)
        {
            if (equipIcon == null)
            {
                foreach (UISprite uISprite in LobbyManager.inst.ObjectPool.transform.GetComponentsInChildren<UISprite>())
                {
                    if (uISprite.spriteName.Equals(string.Format("장착표시")))
                    {
                        equipIcon = uISprite;
                        equipIcon.transform.parent = gameObject.transform;
                        equipIcon.transform.localPosition = new Vector3(100, -100);
                        break;
                    }
                }
            }

        }
        else
        {
            if (equipIcon != null)
            {
                equipIcon.gameObject.transform.parent = LobbyManager.inst.ObjectPool.transform;
                equipIcon.transform.localPosition = Vector3.zero;
                equipIcon = null;
            }
        }
        if(item.name.Equals(""))
        {
            itemIcon.spriteName = "None";
        }
        else
        {
            itemIcon.spriteName = item.name;
        }
        
        if (item == null)

            return;
    }
    private void OnDisable()
    {

    }

}
