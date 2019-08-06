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
        
        if (!cell.inventoryNum.Equals(item.inventoryNum)||cell.inventoryNum.Equals(0))
        {
            Debug.Log(cell.inventoryNum);
            cell = item;
            enchantlevel.spriteName = string.Format("강화수치_{0}", item.upgrade_Level.ToString());
            if (item.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum) || item.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum))
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
            if (!item.item_Class.Equals(Item_CLASS.갑옷) && !item.item_Class.Equals(Item_CLASS.아이템))
            {
                Itemvalue.text = "공격력:\t" + item.stat.ToString();
                Attackpercent.text= "공격력:\t" + Database.Inst.skill[item.skill_index].attack_Power.ToString()+"%";
                Activemana.text = "소모마나량:\t" + Database.Inst.skill[item.skill_index].coolDown.ToString();
                Activecooltime.text = "쿨타임:\t" + Database.Inst.skill[item.skill_index].coolDown.ToString();
                Activetarget.text = "대상:\t" + Database.Inst.skill[item.skill_index].attack_Type;
                Activerange.text = "범위:\t" + Database.Inst.skill[item.skill_index].attack_Range.ToString();
                Attackpercent.gameObject.SetActive(true);
                Activemana.gameObject.SetActive(true);
                Activecooltime.gameObject.SetActive(true);
                Activetarget.gameObject.SetActive(true);
                Activerange.gameObject.SetActive(true);
            }
            else if (item.item_Class.Equals(Item_CLASS.갑옷))
            {
                Itemvalue.text = "체력:\t" + item.stat.ToString();
                Attackpercent.gameObject.SetActive(false);
                Activemana.gameObject.SetActive(false);
                Activecooltime.gameObject.SetActive(false);
                Activetarget.gameObject.SetActive(false);
                Activerange.gameObject.SetActive(false);
            }
        }
        if (item == null)

            return;
    }
    private void ChangeEquip()
    {
        LobbyManager.inst.changeEquip.SetActive(true);
        LobbyManager.inst.BGID.SetActive(true);
    }
    public void ButtonActive()
    {
        GameObject Equipanel,changeEquipanel;
        Database.Inventory Equipdata,changeEquipdata;
        if (!cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum))
        {
            ChangeEquip();
            Equipanel=LobbyManager.inst.changeEquip.transform.Find("Equippanel").gameObject;
            Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
            changeEquipdata = Database.Inst.playData.inventory[cell.inventoryNum];
            Equipanel.transform.Find("EquipBGI").Find("EquipIcon").GetComponent<UISprite>().spriteName = Equipdata.imageName;
            Equipanel.transform.Find("EquipBGI").Find("EnchantLevel").GetComponent<UISprite>().spriteName = "강화수치_" + Equipdata.upgrade_Level.ToString();
            Equipanel.transform.Find("EquipItemname").GetComponent<UILabel>().text = Equipdata.name;
            Equipanel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().text = Equipdata.rarity.ToString();
            Equipanel.transform.Find("EquipItemclass").GetComponent<UILabel>().text = "종류:\t" + Equipdata.item_Class.ToString();
            Equipanel.transform.Find("AttackDamage").GetComponent<UILabel>().text = "공격력:\t" + Equipdata.stat.ToString();
            Equipanel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = (Equipdata.stat - changeEquipdata.stat).ToString();

            switch (Equipdata.rarity)
            {
                case RARITY.노말:
                    Equipanel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.gray;
                    break;
                case RARITY.레어:
                    Equipanel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.blue;
                    break;
                case RARITY.유니크:
                    Equipanel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = new Color(156,91,025);
                    break;
                case RARITY.레전드:
                    Equipanel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.yellow;
                    break;
                default:
                    break;
            }
            
        }
        else if(!cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
        {

        }
    }
}
