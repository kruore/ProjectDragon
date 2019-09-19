using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Equipmentcell : UIReuseScrollViewCell
{
    public UISprite equipIcon, itemIcon;
    public UISprite enchantlevel, rarity, Lock;
    public UILabel Itemname, Itemvalue, Attackpercent, Activemana, Activecooltime, Activetarget, Activerange;
    public EuipmentcellData cell;
    float time,myYposition;
    private void Start()
    {
        UIEventListener.Get(gameObject).onPress += Buttonpress;
    }
    public override void UpdateData(IReuseCellData _CellData)
    {
        EuipmentcellData item = _CellData as EuipmentcellData;
        cell = item;
        enchantlevel.spriteName = string.Format("강화수치_{0}", item.upgrade_Level.ToString());
        rarity.spriteName = string.Format("레어도_{0}", item.rarity.ToString());
        gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.white;
        gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.white;
        bool check = true;
        if (LobbyManager.inst.Selecteditem.Count > 0)
        {
            foreach (int Select in LobbyManager.inst.Selecteditem)
            {
                if (Select.Equals(cell.inventoryNum))
                {
                    gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.gray;
                    gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.gray;
                    check = false;
                    Debug.Log(Select);
                    break;
                }
            }
            if (check)
            {
                gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.white;
                gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.white;
            }
        }
        else
        {
            gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.white;
            gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.white;
        }
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
            else
            {
                equipIcon.gameObject.SetActive(true);
            }
        }
        else
        {
            if (equipIcon != null)
            {
                //equipIcon.gameObject.transform.parent = LobbyManager.inst.objectPool.transform;
                //equipIcon.transform.localPosition = Vector3.zero;
                //equipIcon = null;
                equipIcon.gameObject.SetActive(false);
            }
        }
        if (cell.isLock)
        {
            Lock.spriteName = "Lock";
        }
        else
        {
            Lock.spriteName = "Unlock";
        }
        Itemname.text = item.name;
        itemIcon.spriteName = item.name;
        if (!item.item_Class.Equals(Item_CLASS.갑옷) && !item.item_Class.Equals(Item_CLASS.아이템))
        {
            Itemvalue.text = "공격력:" + item.stat.ToString();
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

        if (item == null)

            return;
    }
    private void ChangeEquippanel()
    {
        LobbyManager.inst.Inventoryback.transform.Find("ChangeEquip").gameObject.SetActive(true);
        LobbyManager.inst.BGID.SetActive(true);
    }
    private void CurrentEquippanel()
    {
        LobbyManager.inst.Inventoryback.transform.Find("CurrentEquip").gameObject.SetActive(true);
        LobbyManager.inst.BGI.SetActive(true);
    }
    // private void
    public void ButtonActive()
    {
        GameObject Equipanel;
        Database.Inventory Equipdata;
        bool check = true;
        switch (LobbyManager.inst.lobbystate)
        {
            case LobbyState.Nomal:
                if (!cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum) && !cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
                {
                    GameObject changeEquipanel;
                    Database.Inventory changeEquipdata;
                    if (!cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum))
                    {
                        LobbyManager.inst.BGID.SetActive(true);
                        GameManager.Inst.Scenestack.Push("ChangeEquip");
                        Equipanel = LobbyManager.inst.changeEquip.transform.Find("Equippanel").gameObject;
                        changeEquipanel = LobbyManager.inst.changeEquip.transform.Find("ChangeItempanel").gameObject;
                        Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
                        ChangeEquippanel();
                        changeEquipdata = Database.Inst.playData.inventory[cell.inventoryNum];
                        ChangeEquip(Equipanel, Equipdata, changeEquipdata.stat);
                        ChangeEquip(changeEquipanel, changeEquipdata, Equipdata.stat);
                        LobbyManager.inst.changeequipdata = cell.inventoryNum;
                        LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiWeapon_InventoryNum;
                    }
                    else
                    {
                        LobbyManager.inst.BGID.SetActive(true);
                        GameManager.Inst.Scenestack.Push("ChangeEquip");
                        Equipanel = LobbyManager.inst.changeEquip.transform.Find("Equippanel").gameObject;
                        changeEquipanel = LobbyManager.inst.changeEquip.transform.Find("ChangeItempanel").gameObject;
                        Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
                        ChangeEquippanel();
                        changeEquipdata = Database.Inst.playData.inventory[cell.inventoryNum];
                        ChangeEquip(Equipanel, Equipdata, changeEquipdata.stat);
                        ChangeEquip(changeEquipanel, changeEquipdata, Equipdata.stat);
                        LobbyManager.inst.changeequipdata = cell.inventoryNum;
                        LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiArmor_InventoryNum;
                    }
                }
                else if (cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
                {
                    LobbyManager.inst.BGI.SetActive(true);
                    GameManager.Inst.Scenestack.Push("CurrentEquip");
                    CurrentEquippanel();
                    Equipanel = LobbyManager.inst.Inventoryback.transform.Find("CurrentEquip/Equippanel").gameObject;
                    Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
                    ChangeEquip(Equipanel, Equipdata, -1);
                    LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiWeapon_InventoryNum;
                }
                else if (cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum))
                {
                    LobbyManager.inst.BGI.SetActive(true);
                    GameManager.Inst.Scenestack.Push("CurrentEquip");
                    CurrentEquippanel();
                    Equipanel = LobbyManager.inst.Inventoryback.transform.Find("CurrentEquip/Equippanel").gameObject;
                    Equipdata = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
                    ChangeEquip(Equipanel, Equipdata, -1);
                    LobbyManager.inst.currentEquipdata = Database.Inst.playData.equiArmor_InventoryNum;
                }
                break;
            case LobbyState.Enchant:
                LobbyManager.inst.Inventoryback.transform.Find("Enchantpanel").gameObject.SetActive(false);
                GameObject EnchantEnter;
                EnchantEnter = LobbyManager.inst.Inventoryback.transform.Find("EnchantEnter").gameObject;
                EnchantEnter.SetActive(true);
                EquipWeaponIcon(EnchantEnter.transform.Find("EquipBGIcollection/EquipBGI").gameObject, Database.Inst.playData.inventory[cell.inventoryNum]);
                EquipWeaponIcon(EnchantEnter.transform.Find("EquipBGIchangecollection/EquipBGI").gameObject, Database.Inst.playData.inventory[cell.inventoryNum]);
                break;
            case LobbyState.Lock:
                if (LobbyManager.inst.Selecteditem.Count > 0)
                {
                    foreach (int Select in LobbyManager.inst.Selecteditem)
                    {
                        if (Select.Equals(cell.inventoryNum))
                        {
                            LobbyManager.inst.Selecteditem.Remove(Select);
                            gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.white;
                            gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.white;
                            LobbyManager.inst.Inventoryback.transform.Find("Lock/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count.ToString());
                            check = false;
                            break;

                        }
                    }
                    if (check)
                    {
                        gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.grey;
                        gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.grey;
                        LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);
                        LobbyManager.inst.Inventoryback.transform.Find("Lock/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count.ToString());
                    }
                }
                else
                {
                    gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.grey;
                    gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.grey;
                    LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);
                    LobbyManager.inst.Inventoryback.transform.Find("Lock/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count.ToString());
                }
                break;
            case LobbyState.Decomposition:
                
                if (LobbyManager.inst.Selecteditem.Count > 0)
                {
                    foreach (int Select in LobbyManager.inst.Selecteditem)
                    {
                        if (Select.Equals(cell.inventoryNum))
                        {
                            LobbyManager.inst.Selecteditem.Remove(Select);
                            gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.white;
                            gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.white;
                            Database.Inst.playData.inventory[0].amount-= DataTransaction.Inst.Convert_EquipmenttoJam(Database.Inst.playData.inventory[cell.inventoryNum]);
                            LobbyManager.inst.Inventoryback.transform.Find("Decomposition/Jamcount/Jam1").GetComponent<UILabel>().text = Database.Inst.playData.inventory[0].amount.ToString();
                            LobbyManager.inst.Inventoryback.transform.Find("Decomposition/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count);
                            check = false;
                            break;

                        }
                    }
                    if (check)
                    {
                        gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.grey;
                        gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.grey;
                        LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);
                        Database.Inst.playData.inventory[0].amount += DataTransaction.Inst.Convert_EquipmenttoJam(Database.Inst.playData.inventory[cell.inventoryNum]);
                        LobbyManager.inst.Inventoryback.transform.Find("Decomposition/Jamcount/Jam1").GetComponent<UILabel>().text = Database.Inst.playData.inventory[0].amount.ToString();
                        LobbyManager.inst.Inventoryback.transform.Find("Decomposition/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count);
                    }
                }
                else
                {
                    gameObject.transform.Find("EquipBGI").GetComponent<UISprite>().color = Color.grey;
                    gameObject.transform.Find("ItemInfoBGI").GetComponent<UISprite>().color = Color.grey;
                    LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);
                    Database.Inst.playData.inventory[0].amount += DataTransaction.Inst.Convert_EquipmenttoJam(Database.Inst.playData.inventory[cell.inventoryNum]);
                    LobbyManager.inst.Inventoryback.transform.Find("Decomposition/Jamcount/Jam1").GetComponent<UILabel>().text = Database.Inst.playData.inventory[0].amount.ToString();
                    LobbyManager.inst.Inventoryback.transform.Find("Decomposition/SelectionCount").GetComponent<UILabel>().text = string.Format("선택 개수 : {0}개", LobbyManager.inst.Selecteditem.Count);
                }
                break;
            default:
                break;
        }
    }
    public void ChangeEquip(GameObject panel, Database.Inventory data, float stat)
    {
        EquipWeaponIcon(panel.transform.Find("EquipBGI").gameObject, data);
        //panel.transform.Find("EquipBGI").Find("EquipIcon").GetComponent<UISprite>().spriteName = data.imageName;
        //panel.transform.Find("EquipBGI").Find("EnchantLevel").GetComponent<UISprite>().spriteName = string.Format("강화수치_{0}", data.upgrade_Level.ToString());
        //panel.transform.Find("EquipBGI").Find("Rarity").GetComponent<UISprite>().spriteName = string.Format("레어도_{0}", data.rarity.ToString());
        panel.transform.Find("EquipItemname").GetComponent<UILabel>().text = data.name;
        panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().text = data.rarity.ToString();
        panel.transform.Find("EquipItemclass").GetComponent<UILabel>().text = string.Format("종류: {0}", data.item_Class.ToString());
        panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}", data.stat);
        if (data.item_Class.Equals(Item_CLASS.검) || data.item_Class.Equals(Item_CLASS.활) || data.item_Class.Equals(Item_CLASS.지팡이))
        {
            panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}", data.stat);
            GameObject ActiveSkill;
            ActiveSkill = panel.transform.Find("ActiveSkill").gameObject;
            ActiveSkill.SetActive(true);
            ActiveSkill.transform.Find("Activename").GetComponent<UILabel>().text = string.Format("액티브: {0}", Database.Inst.skill[data.skill_Index].name);
            ActiveSkill.transform.Find("ActiveDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}%", Database.Inst.skill[data.skill_Index].attack_Power * 100);
            ActiveSkill.transform.Find("Activetarget").GetComponent<UILabel>().text = string.Format("대상: {0}", Database.Inst.skill[data.skill_Index].attack_Type);
            ActiveSkill.transform.Find("ActiveRange").GetComponent<UILabel>().text = string.Format("범위: {0}", Database.Inst.skill[data.skill_Index].attack_Range);
            ActiveSkill.transform.Find("Activemana").GetComponent<UILabel>().text = string.Format("마나: {0}", Database.Inst.skill[data.skill_Index].active_Time);
            ActiveSkill.transform.Find("Activecooltime").GetComponent<UILabel>().text = string.Format("쿨타임: {0}", Database.Inst.skill[data.skill_Index].coolDown);
            ActiveSkill.transform.Find("ActiveBGI").Find("ActiveIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[data.skill_Index].name;
        }
        else
        {
            panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("체력: {0}", data.stat);
            GameObject ActiveSkill;
            ActiveSkill = panel.transform.Find("ActiveSkill").gameObject;
            ActiveSkill.SetActive(false);
        }
        if (data.isLock)
        {
            panel.transform.Find("Lock").GetComponent<UISprite>().spriteName = "Lock";
            panel.transform.Find("Lock").GetComponent<UIButton>().normalSprite = "Lock";
            panel.transform.Find("Lock").GetComponent<UIButton>().hoverSprite = "Lock";
        }
        else
        {
            panel.transform.Find("Lock").GetComponent<UISprite>().spriteName = "Unlock";
            panel.transform.Find("Lock").GetComponent<UIButton>().normalSprite = "Unlock";
            panel.transform.Find("Lock").GetComponent<UIButton>().hoverSprite = "Unlock";
        }
        if (!stat.Equals(-1))
        {
            panel.transform.Find("AttackDamage").Find("StatGap").gameObject.SetActive(true);
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
                panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.red;
                panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = string.Format("(-{0})", Mathf.Abs(data.stat - stat).ToString());
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
    public void EquipWeaponIcon(GameObject IconObject, Database.Inventory data)
    {
        LobbyManager.inst.ChangeItemIcon(IconObject, data);
    }
    public void Buttonpress(GameObject sender, bool state)
    {
        if(sender.Equals(gameObject)&& state)
        {
            Debug.Log("Press");
            StartCoroutine("TimeRange");
        }
        else if(sender.Equals(gameObject) && !state)
        {
            if(time>1.5f&&(Mathf.Abs(myYposition-gameObject.transform.position.y)<0.05)&&LobbyManager.inst.lobbystate.Equals(LobbyState.Nomal))
            {
                Debug.Log(Mathf.Abs(myYposition - gameObject.transform.position.y));
                LobbyManager.inst.Inventoryback.transform.Find("Lock").gameObject.SetActive(true);
                LobbyManager.inst.BGID.SetActive(true);
                GameManager.Inst.Scenestack.Push("Lock");
                LobbyManager.inst.lobbystate = LobbyState.Lock;
            }
            StopCoroutine("TimeRange");
            Debug.Log(time);
            Debug.Log("Presscancle");
        }
    }
    public IEnumerator TimeRange()
    {
        time = 0;
        myYposition=transform.position.y;
        while(true)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
