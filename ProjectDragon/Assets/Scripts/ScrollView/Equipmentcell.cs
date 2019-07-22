using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipmentcell : UIReuseScrollViewCell
{
    public UISprite equipIcon;
    public UISprite enchantlevel;
    public override void UpdateData(IReuseCellData _CellData)
    {
        Debug.Log(string.Format("Index={0}, Name={1},Power={2},Equip={3},enchantlevel={4}",_CellData.Index.ToString(), _CellData.name.ToString(), _CellData.power.ToString(), _CellData.equip.ToString(), _CellData.enchantlevel.ToString()));
        EuipmentcellData item = _CellData as EuipmentcellData;
        if(enchantlevel!=null)
        {
            enchantlevel.gameObject.transform.parent = LobbyManager.inst.ObjectPool.transform;
            enchantlevel.transform.localPosition = Vector3.zero;
            enchantlevel = null;
        }
        if(item.enchantlevel>0)
        {
            foreach(UISprite uISprite in LobbyManager.inst.ObjectPool.transform.GetComponentsInChildren<UISprite>())
            {
                if(uISprite.spriteName.Equals(string.Format("enchantlevel{0}",item.enchantlevel.ToString())))
                {
                    enchantlevel = uISprite;
                    enchantlevel.transform.parent = gameObject.transform;
                    enchantlevel.transform.position = gameObject.transform.position;
                }
            }
        }
        if(item.m_equip)
        {

            foreach (UISprite uISprite in LobbyManager.inst.ObjectPool.transform.GetComponentsInChildren<UISprite>())
            {
                if (uISprite.spriteName.Equals(string.Format("Btn_Icon_Check")))
                { 
                    equipIcon = uISprite;
                    equipIcon.transform.parent = gameObject.transform;
                    equipIcon.transform.localPosition = new Vector3(-100, -60);
                    break;
                }
            }
            
        }
        if(_CellData.name==null)
        equipIcon.spriteName = "Box6";
        if (item == null)
            
            return;
    }
    private void OnDisable()
    {
        if(enchantlevel!=null)
        {
            enchantlevel.gameObject.SetActive(true);
            enchantlevel.gameObject.transform.parent = LobbyManager.inst.ObjectPool.transform;
            enchantlevel.transform.localPosition = Vector3.zero;
            enchantlevel = null;
        }
    }

}
