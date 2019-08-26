using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LobbyState { Nomal, Enchant, Lock }
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager inst;
    bool isnight = true;
    GameObject particle;
    public GameObject objectPool, fireobject, playeranimation, playerimg;
    public GameObject filterobject, arrangementobject;
    public AudioClip fire;
    public UISpriteAnimation anim;
    #region equipobject
    public GameObject useJameConfirm, useJam, changeEquip, BGID, BGI;
    public GameObject Inventoryback, Currentweapon, CurrentArmor, CurrentActive, equipCharactor, scrollview;
    public int changeequipdata,currentEquipdata,selectData;
    public LobbyState lobbystate = LobbyState.Nomal;
    public List<int> Selecteditem;
    #endregion
    private void Awake()
    {
        inst = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        Selecteditem = new List<int>();
        selectData = 0;
        Database.Inst.playData.equiWeapon_InventoryNum = 3;
        Database.Inst.playData.equiArmor_InventoryNum = 10;
        if (isnight)
        {
            SoundManager.Inst.Ds_BgmPlayer(fire);
            fireobject.SetActive(true);
        }
        else
        {
        }
        string classname = "null";
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        if (item.item_Class.Equals(Item_CLASS.검))
        {
            classname = "Worrior";
            if (Database.Inst.playData.sex.Equals(SEX.Male))
            {
                playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
        }
        else if (item.item_Class.Equals(Item_CLASS.활))
        {
            classname = "Archer";
        }
        else if (item.item_Class.Equals(Item_CLASS.지팡이))
        {
            classname = "Wizard";
        }


        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);

        playerimg.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        equipCharactor.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        ChangeItemIcon(Currentweapon, item);
        Currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = item.stat.ToString();
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;


        //테스트용 코드
        GameManager.Inst.Scenestack.Push("Enchant");
        lobbystate = LobbyState.Enchant;

    }
    // Update is called once per frame
    void Update()
    {
        //터치시 파티클생성
        if (Input.GetMouseButtonUp(0))
        {
            if (particle != null)
            {
                Destroy(particle);
            }
            particle = Instantiate<GameObject>(Resources.Load<GameObject>("Star_A"));
            particle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            particle.transform.parent = GameObject.Find("UI Root/Panel").transform;
            Vector3 wp = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
            particle.transform.position = wp;
            Destroy(particle, 0.5f);
        }
    }
    public void TouchBackButton()
    {
        Debug.Log(ButtonManager.TouchBackButton());
    }
    public void ObjectControl()
    {
        ButtonManager.ObjectlistControl();
    }
    public void ChangeEquip()
    {
        TouchBackButton();
        if (Database.Inst.playData.inventory[changeequipdata].item_Class.Equals(Item_CLASS.갑옷))
        {
            Database.Inst.playData.equiArmor_InventoryNum = changeequipdata;
        }
        else if (!Database.Inst.playData.inventory[changeequipdata].item_Class.Equals(Item_CLASS.아이템))
        {
            Database.Inst.playData.equiWeapon_InventoryNum = changeequipdata;
        }
        string classname = "null";
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        if (item.item_Class.Equals(Item_CLASS.검))
        {
            classname = "Worrior";
            if (Database.Inst.playData.sex.Equals(SEX.Male))
            {
                playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
        }
        else if (item.item_Class.Equals(Item_CLASS.활))
        {
            classname = "Archer";
        }
        else if (item.item_Class.Equals(Item_CLASS.지팡이))
        {
            classname = "Wizard";
        }
        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);
        playerimg.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        equipCharactor.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        ChangeItemIcon(Currentweapon, item);
        Currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = item.stat.ToString();
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        scrollview.GetComponent<GUITestScrollView>().EV_UpdateAll();
    }
    public void ChangeItemIcon(GameObject Icon, Database.Inventory data)
    {
        Icon.transform.Find("EquipIcon").GetComponent<UISprite>().spriteName = data.name;
        Icon.transform.Find("EnchantLevel").GetComponent<UISprite>().spriteName = string.Format("강화수치_{0}", data.upgrade_Level.ToString());
        Icon.transform.Find("Rarity").gameObject.GetComponent<UISprite>().spriteName = string.Format("레어도_{0}", data.rarity.ToString());
        if (data.isLock)
        {
            Icon.transform.Find("IsLock").GetComponent<UISprite>().spriteName = "Lock";
        }
        else
        {
            Icon.transform.Find("IsLock").GetComponent<UISprite>().spriteName = "Unlock";
        }
    }
    public void CurrentEquipLock()
    {
        //Database.Inst.[currentEquipdata]
    }
}