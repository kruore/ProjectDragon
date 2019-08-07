using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipmentcell : UIReuseScrollViewCell
{
    public UISprite equipIcon, itemIcon;
    public UISprite enchantlevel;
    public UILabel Itemname, Itemvalue, Attackpercent, Activemana, Activecooltime, Activetarget, Activerange;
    public EuipmentcellData cell;
    public override void UpdateData(IReuseCellData _CellData)
    {
        EuipmentcellData item = _CellData as EuipmentcellData;
        Debug.Log(cell.name + "++++" + item.name);
        if (!cell.inventoryNum.Equals(item.inventoryNum) || !cell.name.Equals(item.name))
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
                Debug.Log(item.stat.ToString());
                Itemvalue.text = "공격력:" + cell.stat;
                Attackpercent.text = "공격력:" + Database.Inst.skill[item.skill_index].attack_Power.ToString() + "%";
                Activemana.text = "소모마나량:" + Database.Inst.skill[item.skill_index].coolDown.ToString();
                Activecooltime.text = "쿨타임:" + Database.Inst.skill[item.skill_index].coolDown.ToString();
                Activetarget.text = "대상:" + Database.Inst.skill[item.skill_index].attack_Type;
                Activerange.text = "범위:" + Database.Inst.skill[item.skill_index].attack_Range.ToString();
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
    private void ChangeEquippanel()
    {
        LobbyManager.inst.changeEquip.SetActive(true);
        LobbyManager.inst.BGID.SetActive(true);
    }
    public void ButtonActive()
    {
        GameObject Equipanel, changeEquipanel;
        Database.Inventory Equipdata, changeEquipdata;
        if (!cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum))
        {
            ChangeEquippanel();
            Equipanel = LobbyManager.inst.changeEquip.transform.Find("Equippanel").gameObject;
            changeEquipanel = LobbyManager.inst.changeEquip.transform.Find("ChangeItempanel").gameObject;
            Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
            changeEquipdata = Database.Inst.playData.inventory[cell.inventoryNum];
            ChangeEquip(Equipanel, Equipdata, changeEquipdata.stat);
            ChangeEquip(changeEquipanel, changeEquipdata, Equipdata.stat);

        }
        else if (!cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
        {

        }
    }
    public void ChangeEquip(GameObject panel, Database.Inventory data, float stat)
    {
        GameManager.Inst.Scenestack.Push("ChangeEquip");
        panel.transform.Find("EquipBGI").Find("EquipIcon").GetComponent<UISprite>().spriteName = data.imageName;
        panel.transform.Find("EquipBGI").Find("EnchantLevel").GetComponent<UISprite>().spriteName = string.Format("강화수치_{0}", data.upgrade_Level.ToString());
        panel.transform.Find("EquipItemname").GetComponent<UILabel>().text = data.name;
        panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().text = data.rarity.ToString();
        panel.transform.Find("EquipItemclass").GetComponent<UILabel>().text =  string.Format("종류: {0}", data.item_Class.ToString());
        panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}", data.stat);
        panel.transform.Find("Activename").GetComponent<UILabel>().text = string.Format("액티브: {0}", Database.Inst.skill[data.skill_Index].name) ;
        panel.transform.Find("ActiveDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}%", Database.Inst.skill[data.skill_Index].attack_Power*100);
        panel.transform.Find("Activetarget").GetComponent<UILabel>().text = string.Format("대상: {0}", Database.Inst.skill[data.skill_Index].attack_Type);
        panel.transform.Find("ActiveRange").GetComponent<UILabel>().text = string.Format("범위: {0}", Database.Inst.skill[data.skill_Index].attack_Range);
        panel.transform.Find("Activemana").GetComponent<UILabel>().text = string.Format("마나: {0}", Database.Inst.skill[data.skill_Index].active_Time);
        panel.transform.Find("Activecooltime").GetComponent<UILabel>().text = string.Format("쿨타임: {0}", Database.Inst.skill[data.skill_Index].coolDown);
        Debug.Log(panel.transform.Find("ActiveBGI").Find("ActiveIcon").name);
        panel.transform.Find("ActiveBGI").Find("ActiveIcon").GetComponent<UISprite>().spriteName =Database.Inst.skill[data.skill_Index].name;

        if (data.stat > stat)
        {
            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.blue;
            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = string.Format("(+{0})", Mathf.Abs(data.stat - stat).ToString());

        }
        else if (data.stat.Equals(stat))
        {
            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.gray;
            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = "0";
        }
        else
        {
            Debug.Log(stat);
            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.red;
            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = string.Format("(-{0})", Mathf.Abs(data.stat - stat).ToString());
        }
        switch (data.rarity)
        {
            case RARITY.노말:
                panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.gray;
                break;
            case RARITY.레어:
                panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.blue;
                break;
            case RARITY.유니크:
                panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = new Color(156, 91, 025);
                break;
            case RARITY.레전드:
                panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.yellow;
                break;
            default:
                break;
        }
    }
}
