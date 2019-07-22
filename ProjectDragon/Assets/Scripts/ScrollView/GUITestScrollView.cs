using UnityEngine;
using System.Collections;

// 메인 클래스. 여기서 grid에 데이터를 추가시켜준다.
public class GUITestScrollView : MonoBehaviour 
{
    public int count;
    private UIReuseGrid grid;

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

	void Start () 
    {
		// 임의의 데이터가 생성해서 gird에 추가시켜둔다.
		// ItemCellData 는 IReuseCellData 상속받아서 구현된 데이터 클래스다.
		for( int i=0; i< count; ++i )
        {
            EuipmentcellData cell = new EuipmentcellData();
			cell.Index = i;
			cell.ImgName = string.Format( "name:{0}", i );
            cell.name = string.Format("name:{0}", i);
            cell.m_power=0;
            cell.equip = false;
            cell.enchantlevel = 0;
            if(i==1||i==3)
            {
                cell.equip = true;
            }
			grid.AddItem( cell, false );
        }
		grid.UpdateAllCellData();
	}

	#region Event
	public void EV_Add()
	{
        EuipmentcellData cell = new EuipmentcellData();
		cell.Index = grid.MaxCellData;
		cell.ImgName = string.Format( "name:{0}", cell.Index );
		grid.AddItem( cell, true );
	}

	public void EV_Remove()
	{
		grid.RemoveItem( grid.GetCellData(0), true );
	}

	public void EV_RemoveAll()
	{
		grid.ClearItem(true);
	}
	#endregion
}
