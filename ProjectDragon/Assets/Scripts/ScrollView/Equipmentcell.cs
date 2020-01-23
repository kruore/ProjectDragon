using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Equipmentcell : UIReuseScrollViewCell
{
    public UISprite equipIcon, itemIcon;
    public UISprite rarity, activeIcon;
    public UILabel Itemname, Itemvalue;
    public EuipmentcellData cell;
    public bool change = false;
    GameObject click;
    private void Start()
    {
        UIEventListener.Get(gameObject).onPress += Buttonpress;
    }
    public override void UpdateData(IReuseCellData _CellData)
    {
        Debug.Log("cell");
        EuipmentcellData item = _CellData as EuipmentcellData;
        if (item == null)
            return;
        cell = item;
        if (LobbyManager.inst.itemclassselect.Equals(cell.Class) || LobbyManager.inst.itemclassselect.Equals(ItemState.기본))
        {
            if (cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum) || cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
            {
                equipIcon.gameObject.SetActive(true);
            }
            else
            {
                equipIcon.gameObject.SetActive(false);
            }
            bool check = true;
            if (LobbyManager.inst.selectData.Equals(cell.inventoryNum))
            {
                gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "List_On";
            }
            else
            {
                gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "List_Off";
            }
            Itemname.text = cell.name;
            itemIcon.spriteName = cell.imageName;
            if (!item.Class.Equals(CLASS.갑옷))
            {
                Itemvalue.text = "공격력:" + cell.stat.ToString();
                activeIcon.gameObject.SetActive(true);
            }
            else
            {
                Itemvalue.text = "체력:\t" + cell.stat.ToString();
                activeIcon.gameObject.SetActive(false);
            }
        }

    }
    public void ButtonActive()
    {
        Debug.Log("cellclick");
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        bool check = true;
        switch (LobbyManager.inst.lobbystate)
        {
            case LobbyState.Nomal:
                if(!cell.Class.Equals(CLASS.갑옷))
                {
                    LobbyManager.inst.BGI.SetActive(true);
                    GameObject itembgi = LobbyManager.inst.BGI.transform.Find("ItemBGI").gameObject;
                    itembgi.transform.Find("name").gameObject.GetComponent<UILabel>().text= Database.Inst.weapons[cell.DB_Num].name.ToString();
                    itembgi.transform.Find("discription").gameObject.GetComponent<UILabel>().text="";
                    itembgi.transform.Find("rare").gameObject.GetComponent<UILabel>().text= Database.Inst.weapons[cell.DB_Num].rarity.ToString();
                    itembgi.transform.Find("mindamagenum").gameObject.GetComponent<UILabel>().text=Database.Inst.weapons[cell.DB_Num].atk_Min.ToString();
                    itembgi.transform.Find("maxdamagenum").gameObject.GetComponent<UILabel>().text= Database.Inst.weapons[cell.DB_Num].atk_Max.ToString();
                    itembgi.transform.Find("attackspeednum").gameObject.GetComponent<UILabel>().text= Database.Inst.weapons[cell.DB_Num].atk_Speed.ToString();
                    itembgi.transform.Find("knockbacknum").gameObject.GetComponent<UILabel>().text= Database.Inst.weapons[cell.DB_Num].nuckback_Percentage.ToString();
                    itembgi.transform.Find("optiondiscription").gameObject.GetComponent<UILabel>().text="";
                }
                break;
            #region delete
            //    GameObject Equipanel;
            //    Database.Inventory Equipdata;
            //    if (!cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum) && !cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
            //    {
            //        GameObject changeEquipanel;
            //        Database.Inventory changeEquipdata;
            //        if (!cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum))
            //        {
            //            if (!cell.m_Class.Equals(CLASS.갑옷))
            //            {
            //                LobbyManager.inst.BGID.SetActive(true);
            //                GameManager.Inst.Scenestack.Push("ChangeEquip");
            //                Equipanel = LobbyManager.inst.changeEquip.transform.Find("Equippanel").gameObject;
            //                changeEquipanel = LobbyManager.inst.changeEquip.transform.Find("ChangeItempanel").gameObject;
            //                Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
            //                ChangeEquippanel();
            //                changeEquipdata = Database.Inst.playData.inventory[cell.inventoryNum];
            //                ChangeEquip(Equipanel, Equipdata,Database.Inst.weapons[changeEquipdata.DB_Num].damage);
            //                ChangeEquip(changeEquipanel, changeEquipdata, Database.Inst.weapons[Equipdata.DB_Num].damage);
            //                LobbyManager.inst.changeequipdata = cell.inventoryNum;
            //                LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiWeapon_InventoryNum;
            //            }
            //            else
            //            {
            //                LobbyManager.inst.BGID.SetActive(true);
            //                GameManager.Inst.Scenestack.Push("ChangeEquip");
            //                Equipanel = LobbyManager.inst.changeEquip.transform.Find("Equippanel").gameObject;
            //                changeEquipanel = LobbyManager.inst.changeEquip.transform.Find("ChangeItempanel").gameObject;
            //                Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
            //                ChangeEquippanel();
            //                changeEquipdata = Database.Inst.playData.inventory[cell.inventoryNum];
            //                ChangeEquip(Equipanel, Equipdata, Database.Inst.weapons[changeEquipdata.DB_Num].damage);
            //                ChangeEquip(changeEquipanel, changeEquipdata, Database.Inst.armors[Equipdata.DB_Num].hp);
            //                LobbyManager.inst.changeequipdata = cell.inventoryNum;
            //                LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiArmor_InventoryNum;
            //            }
            //        }

            //        else
            //        {
            //            LobbyManager.inst.BGID.SetActive(true);
            //            GameManager.Inst.Scenestack.Push("ChangeEquip");
            //            Equipanel = LobbyManager.inst.changeEquip.transform.Find("Equippanel").gameObject;
            //            changeEquipanel = LobbyManager.inst.changeEquip.transform.Find("ChangeItempanel").gameObject;
            //            Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
            //            ChangeEquippanel();
            //            changeEquipdata = Database.Inst.playData.inventory[cell.inventoryNum];
            //            ChangeEquip(Equipanel, Equipdata, Database.Inst.weapons[changeEquipdata.DB_Num].damage);
            //            ChangeEquip(changeEquipanel, changeEquipdata, Database.Inst.armors[Equipdata.DB_Num].hp);
            //            LobbyManager.inst.changeequipdata = cell.inventoryNum;
            //            LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiArmor_InventoryNum;
            //        }
            //    }
            //    else if (cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
            //    {
            //        LobbyManager.inst.BGI.SetActive(true);
            //        GameManager.Inst.Scenestack.Push("CurrentEquip");
            //        CurrentEquippanel();
            //        //Equipanel = LobbyManager.inst.Inventoryback.transform.Find("CurrentEquip/Equippanel").gameObject;
            //        Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
            //        //ChangeEquip(Equipanel, Equipdata, -1);
            //        LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiWeapon_InventoryNum;
            //    }
            //    else if (cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum))
            //    {
            //        LobbyManager.inst.BGI.SetActive(true);
            //        GameManager.Inst.Scenestack.Push("CurrentEquip");
            //        CurrentEquippanel();
            //        Equipanel = LobbyManager.inst.Inventoryback.transform.Find("CurrentEquip/Equippanel").gameObject;
            //        Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
            //        ChangeEquip(Equipanel, Equipdata, -1);
            //        LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiArmor_InventoryNum;
            //    }
            //    break;
            //case LobbyState.Enchant:
            //    LobbyManager.inst.Inventoryback.transform.Find("Enchantpanel").gameObject.SetActive(false);
            //    GameObject EnchantEnter;
            //    EnchantEnter = LobbyManager.inst.Inventoryback.transform.Find("EnchantEnter").gameObject;
            //    EnchantEnter.SetActive(true);
            //    EquipWeaponIcon(EnchantEnter.transform.Find("EquipBGIcollection/EquipBGI").gameObject, Database.Inst.playData.inventory[cell.inventoryNum]);
            //    EquipWeaponIcon(EnchantEnter.transform.Find("EquipBGIchangecollection/EquipBGI").gameObject, Database.Inst.playData.inventory[cell.inventoryNum]);
            //    Debug.Log("clickEnchant");
            //    break;
            //case LobbyState.Lock:
            //    if (LobbyManager.inst.Selecteditem.Count > 0)
            //    {
            //        foreach (int Select in LobbyManager.inst.Selecteditem)
            //        {
            //            if (Select.Equals(cell.inventoryNum))
            //            {
            //                LobbyManager.inst.Selecteditem.Remove(Select);
            //                gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.white;
            //                gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.white;
            //                LobbyManager.inst.Inventoryback.transform.Find("Lock/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count.ToString());
            //                check = false;
            //                break;

            //            }
            //        }
            //        if (check)
            //        {
            //            gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.grey;
            //            gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.grey;
            //            LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);
            //            LobbyManager.inst.Inventoryback.transform.Find("Lock/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count.ToString());
            //        }
            //    }
            //    else
            //    {
            //        gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.grey;
            //        gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.grey;
            //        LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);
            //        LobbyManager.inst.Inventoryback.transform.Find("Lock/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count.ToString());
            //    }
            //    break;
            //case LobbyState.Decomposition:

            //    if (LobbyManager.inst.Selecteditem.Count > 0)
            //    {
            //        foreach (int Select in LobbyManager.inst.Selecteditem)
            //        {
            //            if (Select.Equals(cell.inventoryNum))
            //            {
            //                LobbyManager.inst.Selecteditem.Remove(Select);
            //                gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.white;
            //                gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.white;
            //                //LobbyManager.inst.jamcounts[0] -= DataTransaction.Inst.Convert_EquipmenttoJam(Database.Inst.playData.inventory[cell.inventoryNum]);
            //                LobbyManager.inst.Inventoryback.transform.Find("Decomposition/Jamcount/Jam1").GetComponent<UILabel>().text = LobbyManager.inst.jamcounts[0].ToString();
            //                LobbyManager.inst.Inventoryback.transform.Find("Decomposition/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count);
            //                check = false;
            //                break;

            //            }
            //        }
            //        if (check)
            //        {
            //            gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.grey;
            //            gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.grey;
            //            LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);
            //            //LobbyManager.inst.jamcounts[0] += DataTransaction.Inst.Convert_EquipmenttoJam(Database.Inst.playData.inventory[cell.inventoryNum]);
            //            LobbyManager.inst.Inventoryback.transform.Find("Decomposition/Jamcount/Jam1").GetComponent<UILabel>().text = LobbyManager.inst.jamcounts[0].ToString();
            //            LobbyManager.inst.Inventoryback.transform.Find("Decomposition/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count);
            //        }
            //    }
            //    else
            //    {
            //        gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.grey;
            //        gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.grey;
            //        LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);
            //        //LobbyManager.inst.jamcounts[0] += DataTransaction.Inst.Convert_EquipmenttoJam(Database.Inst.playData.inventory[cell.inventoryNum]);
            //        LobbyManager.inst.Inventoryback.transform.Find("Decomposition/Jamcount/Jam1").GetComponent<UILabel>().text = LobbyManager.inst.jamcounts[0].ToString();
            //        LobbyManager.inst.Inventoryback.transform.Find("Decomposition/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count);
            //    }
            #endregion
            default:
                break;
        }
    }
    public void SkillActive()
    {
        LobbyManager.inst.BGI.SetActive(true);
        GameObject itembgi = LobbyManager.inst.BGI.transform.Find("ItemBGI").gameObject;
        itembgi.transform.Find("name").gameObject.GetComponent<UILabel>().text = cell.name;
        itembgi.transform.Find("discription").gameObject.GetComponent<UILabel>().text = "";
        itembgi.transform.Find("rare").gameObject.GetComponent<UILabel>().text = "";
        itembgi.transform.Find("mindamagenum").gameObject.GetComponent<UILabel>().text = "";
        itembgi.transform.Find("maxdamagenum").gameObject.GetComponent<UILabel>().text = "";
        itembgi.transform.Find("attackspeednum").gameObject.GetComponent<UILabel>().text = "";
        itembgi.transform.Find("knockbacknum").gameObject.GetComponent<UILabel>().text = "";
        itembgi.transform.Find("optiondiscription").gameObject.GetComponent<UILabel>().text = "";
    }
    public void ChangeEquip(GameObject panel, Database.Inventory data, float stat)
    {
        EquipWeaponIcon(panel.transform.Find("EquipBGI").gameObject, data);
        panel.transform.Find("EquipItemname").GetComponent<UILabel>().text = data.name;
        panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().text = data.rarity.ToString();
        panel.transform.Find("EquipItemclass").GetComponent<UILabel>().text = string.Format("종류: {0}", data.Class.ToString());
        panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}", Database.Inst.weapons[data.DB_Num].atk_Min);
        if (!data.Class.Equals(CLASS.갑옷))
        {
            panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}", Database.Inst.weapons[data.DB_Num].atk_Min);
            GameObject ActiveSkill;
            ActiveSkill = panel.transform.Find("ActiveSkill").gameObject;
            ActiveSkill.SetActive(true);
            ActiveSkill.transform.Find("Activename").GetComponent<UILabel>().text = string.Format("액티브: {0}", Database.Inst.skill[data.skill_Index].name);
            ActiveSkill.transform.Find("ActiveDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}%", Database.Inst.skill[data.skill_Index].atk);
            ActiveSkill.transform.Find("ActiveRange").GetComponent<UILabel>().text = string.Format("범위: {0}", Database.Inst.skill[data.skill_Index].skill_Range);
            ActiveSkill.transform.Find("Activemana").GetComponent<UILabel>().text = string.Format("마나: {0}", Database.Inst.skill[data.skill_Index].skill_Duration);
            ActiveSkill.transform.Find("Activecooltime").GetComponent<UILabel>().text = string.Format("쿨타임: {0}", Database.Inst.skill[data.skill_Index].coolTime);
            ActiveSkill.transform.Find("ActiveBGI").Find("ActiveIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[data.skill_Index].imageName;
        }
        else
        {
            panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("체력: {0}", Database.Inst.armors[data.DB_Num].hp);
            GameObject ActiveSkill;
            ActiveSkill = panel.transform.Find("ActiveSkill").gameObject;
            ActiveSkill.SetActive(false);
        }
        if (!stat.Equals(-1))
        {
            panel.transform.Find("AttackDamage").Find("StatGap").gameObject.SetActive(true);
            if (Database.Inst.weapons[data.DB_Num].atk_Min > stat)
            {
                panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.blue;
                panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = string.Format("(+{0})", Mathf.Abs(Database.Inst.weapons[data.DB_Num].atk_Min - stat).ToString());

            }
            else if (Database.Inst.weapons[data.DB_Num].atk_Min.Equals(stat))
            {
                panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.gray;
                panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = "0";
            }
            else
            {
                panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.red;
                panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = string.Format("(-{0})", Mathf.Abs(Database.Inst.weapons[data.DB_Num].atk_Min - stat).ToString());
            }
        }
        else
        {
            panel.transform.Find("AttackDamage").Find("StatGap").gameObject.SetActive(false);
        }
        switch (data.rarity)
        {
            case RARITY.노말:
                panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.gray;
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
    public void EquipWeaponIcon(GameObject IconObject, Database.Inventory data)
    {
        LobbyManager.inst.ChangeItemIcon(IconObject, data);
    }
    public void Buttonpress(GameObject sender, bool state)
    {
       
        if (sender.Equals(gameObject) && !state)
        {
            if (LobbyManager.inst.selectData.Equals(cell.inventoryNum))
            {
                if(cell.Class.Equals(CLASS.갑옷))
                {
                    Database.Inst.playData.equiArmor_InventoryNum = cell.inventoryNum;
                    LobbyManager.inst.SetArmor();
                }
                else
                {
                    Database.Inst.playData.equiWeapon_InventoryNum = cell.inventoryNum;
                    LobbyManager.inst.SetWeapon();
                }
                gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "List_Off";
                LobbyManager.inst.selectData = -1;
                LobbyManager.inst.SetplayerStat();
                //DataTransaction.Inst.SavePlayerData();
            }
            else
            {
                if (cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum)||cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
                {
                    
                }
                else
                {
                    LobbyManager.inst.selectData = cell.inventoryNum;
                    gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "List_On";
                    LobbyManager.inst.UpdateAllScrollview();
                }
            }
        }
    }
}
