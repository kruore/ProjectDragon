using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipmentcell : UIReuseScrollViewCell
{
    public UISprite equipIcon, itemIcon;
    public UISprite enchantlevel;
    public UILabel  Itemname,Itemvalue,Attackpercent,Activemana, Activecooltime, Activetarget,Activerange;
    public EuipmentcellData cell;
    public override void UpdateData(IReuseCellData _CellData)
    {
        EuipmentcellData item = _CellData as EuipmentcellData;
        cell = item;
        enchantlevel.spriteName = string.Format("강화수치_{0}", item.upgrade_Level.ToString());
        if (item.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum)|| item.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum))
        {
            if (equipIcon == null)
            {
                foreach (UISprite uISprite in LobbyManager.inst.objectPool.transform.GetComponentsInChildren<UISprite>())
                {
                    if (uISprite.spriteName.Equals(string.Format("장착표시")))
                    {
                        equipIcon = uISprite;
                        equipIcon.transform.parent = gameObject.transform;
                        equipIcon.transform.localPosition = new Vector3(100, -57.5f);
                        break;
                    }
                }
            }

        }
        else
        {
            if (equipIcon != null)
            {
                equipIcon.gameObject.transform.parent = LobbyManager.inst.objectPool.transform;
                equipIcon.transform.localPosition = Vector3.zero;
                equipIcon = null;
            }
        }
        Itemname.text = item.name;
        itemIcon.spriteName = item.name;
        if (!item.item_Class.Equals(Item_CLASS.Armor) && !item.item_Class.Equals(Item_CLASS.item))
        {
            Itemvalue.text = "공격력:\t"+item.stat.ToString();
        }
        Debug.Log(Database.Inst.skill.Count);
        Debug.Log(Database.Inst.skill[item.skill_index].coolDown.ToString());
        Activemana.text = "소모마나량:\t"+Database.Inst.skill[item.skill_index].coolDown.ToString();
        Debug.Log(Activemana.text);
        Activecooltime.text = "쿨타임:\t" + Database.Inst.skill[item.skill_index].coolDown.ToString();
        Activetarget.text = "대상:\t" + Database.Inst.skill[item.skill_index].attack_Type;
        Activerange.text = "범위:\t" + Database.Inst.skill[item.skill_index].attack_Range.ToString();

        //item.itemValue;

        if (item == null)

            return;
    }
    private void ChangeEquip()
    {
        LobbyManager.inst.changeEquip.SetActive(true);
        LobbyManager.inst.BGID.SetActive(true);
    }

}
