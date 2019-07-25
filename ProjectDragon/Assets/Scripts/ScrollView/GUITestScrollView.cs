using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// 메인 클래스. 여기서 grid에 데이터를 추가시켜준다.
public class GUITestScrollView : MonoBehaviour
{
    public int count;
    private UIReuseGrid grid;
    public List<Database.Inventory> inventories;
    public UIReuseGrid Grid
    {
        get
        {
            return grid;
        }
    }

    void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
    }

    void Start()
    {

        inventories = Database.Inst.playData.inventory;
        // 임의의 데이터가 생성해서 gird에 추가시켜둔다.
        // ItemCellData 는 IReuseCellData 상속받아서 구현된 데이터 클래스다.
        count = inventories.Count;
        if (count < 16)
        {
            for (int i = 0; i < 16; ++i)
            {
                EuipmentcellData cell = new EuipmentcellData();
                if (i < count)
                {
                    cell.amount = inventories[i].amount;
                    cell.DB_Num = inventories[i].DB_Num;
                    cell.imageName = inventories[i].imageName;
                    cell.isEquipment = inventories[i].isEquipment;
                    cell.itemValue = inventories[i].itemValue;
                    cell.item_Class = inventories[i].item_Class;
                    cell.name = inventories[i].name;
                    cell.Index = inventories[i].num;
                    cell.rarity = inventories[i].rarity;
                    cell.upgrade_Count = inventories[i].upgrade_Count;
                    cell.upgrade_Level = inventories[i].upgrade_Level;
                }
                else
                {
                    cell.amount = 0;
                    cell.DB_Num = 0;
                    cell.imageName = "none";
                    cell.isEquipment = false;
                    cell.itemValue = 0;
                    cell.item_Class = 0;
                    cell.name = "";
                    cell.Index = 0;
                    cell.rarity = RARITY.Normal;
                    cell.upgrade_Count = 0;
                    cell.upgrade_Level = 0;
                }
                grid.AddItem(cell, false);
            }
        }
        else
        {
            if(count%4!=0)
            {
                count+=(int)(4-count%4);
            }
            for (int i = 0; i < count; ++i)
            {
                EuipmentcellData cell = new EuipmentcellData();
                if (i < inventories.Count)
                {
                    cell.amount = inventories[i].amount;
                    cell.DB_Num = inventories[i].DB_Num;
                    cell.imageName = inventories[i].imageName;
                    cell.isEquipment = inventories[i].isEquipment;
                    cell.itemValue = inventories[i].itemValue;
                    cell.item_Class = inventories[i].item_Class;
                    cell.name = inventories[i].name;
                    cell.Index = inventories[i].num;
                    cell.rarity = inventories[i].rarity;
                    cell.upgrade_Count = inventories[i].upgrade_Count;
                    cell.upgrade_Level = inventories[i].upgrade_Level;
                }
                else
                {
                    cell.amount = 0;
                    cell.DB_Num = 0;
                    cell.imageName = "none";
                    cell.isEquipment = false;
                    cell.itemValue = 0;
                    cell.item_Class = 0;
                    cell.name = "";
                    cell.Index = 0;
                    cell.rarity = RARITY.Normal;
                    cell.upgrade_Count = 0;
                    cell.upgrade_Level = 0;
                }
                grid.AddItem(cell, false);
            }
        }
        grid.UpdateAllCellData();
    }

    #region Event
    public void EV_Add()
    {
        EuipmentcellData cell = new EuipmentcellData();
        cell.DB_Num = grid.MaxCellData;
        cell.imageName = string.Format("name:{0}", cell.DB_Num);
        grid.AddItem(cell, true);
    }

    public void EV_Remove()
    {
        grid.RemoveItem(grid.GetCellData(0), true);
    }

    public void EV_RemoveAll()
    {
        grid.ClearItem(true);
    }
    #endregion
}
