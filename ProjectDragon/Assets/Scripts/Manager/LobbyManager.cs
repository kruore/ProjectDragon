using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LobbyState { Nomal, Enchant, Lock, Decomposition }
public enum ItemState { 기본, 검, 활, 지팡이, 갑옷 }
public enum ItemRarity { 기본, 노말, 레어, 유니크, 레전드 }
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
    public int changeequipdata, currentEquipdata, selectData, enchantJam, m_sortSelect;
    public LobbyState lobbystate = LobbyState.Nomal;
    public List<int> Selecteditem;
    public ItemState itemclassselect;
    public ItemRarity ItemRarityselect;
    public List<GUITestScrollView> gUITestScrollViews;
    public UILabel test;
    public static string teststring;
    public int sortSelect
    {
        get
        {
            return m_sortSelect;
        }
        set
        {
            m_sortSelect = value;
            InventorySort(m_sortSelect, scrollview.GetComponent<GUITestScrollView>());
        }
    }
    #endregion
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("sex", 1);
        DataTransaction.Inst.SavePlayerData();
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
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
        CurrentActive.transform.Find("IconIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].skill_Index].imageName;

        //테스트용 코드
        //GameManager.Inst.Scenestack.Push("Enchant");
        //lobbystate = LobbyState.Decomposition;

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
        test.text = teststring;
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
        CurrentActive.transform.Find("IconIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].skill_Index].imageName;
        UpdateAllScrollview();
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
        Database.Inst.playData.inventory[currentEquipdata].isLock = !Database.Inst.playData.inventory[currentEquipdata].isLock;
        if (Database.Inst.playData.inventory[currentEquipdata].isLock)
        {
            UIButton.current.normalSprite = "Lock";
            UIButton.current.hoverSprite = "Lock";
        }
        else
        {
            UIButton.current.normalSprite = "Unlock";
            UIButton.current.hoverSprite = "Unlock";
        }
        ChangeItemIcon(Currentweapon, Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum]);
        UpdateAllScrollview();
        DataTransaction.Inst.SavePlayerData();
    }
    public void ChageEquipLock()
    {
        Database.Inst.playData.inventory[changeequipdata].isLock = !Database.Inst.playData.inventory[changeequipdata].isLock;
        if (Database.Inst.playData.inventory[changeequipdata].isLock)
        {
            UIButton.current.normalSprite = "Lock";
            UIButton.current.hoverSprite = "Lock";
        }
        else
        {
            UIButton.current.normalSprite = "Unlock";
            UIButton.current.hoverSprite = "Unlock";
        }
        UpdateAllScrollview();
        DataTransaction.Inst.SavePlayerData();
    }
    public void UpdateAllScrollview()
    {
        foreach (GUITestScrollView scrollView in gUITestScrollViews)
        {
            scrollView.EV_UpdateAll();
        }

    }
    public void ListLock()
    {
        foreach (int item in Selecteditem)
        {
            Database.Inst.playData.inventory[item].isLock = !Database.Inst.playData.inventory[item].isLock;
        }
        Selecteditem.Clear();
        lobbystate = LobbyState.Nomal;
        UpdateAllScrollview();
        TouchBackButton();
        DataTransaction.Inst.SavePlayerData();

    }
    public void EnchantState()
    {
        GameManager.Inst.Scenestack.Push("Enchant");
        lobbystate = LobbyState.Enchant;
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
    }
    public void DecompositionState()
    {
        GameManager.Inst.Scenestack.Push("Decomposition");
        lobbystate = LobbyState.Decomposition;
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
        for (int i = 1; i == 3; i++)
        {
            GameObject jamcount;
            jamcount = Inventoryback.transform.Find(string.Format("Decomposition/Jamcount{0}", i)).gameObject;
            jamcount.GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[i - 1].amount);
        }

    }
    public void NomalState()
    {
        GameManager.Inst.Scenestack.Push("EquipPanel");
        Debug.Log(GameManager.Inst.Scenestack);
        lobbystate = LobbyState.Nomal;
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
    }
    public void EnchantJamCountMinus()
    {
        if (enchantJam > 0)
        {
            enchantJam--;
            Inventoryback.transform.Find("EnchantEnter/JamIcon1/CountBGI/Count").GetComponent<UILabel>().text = enchantJam.ToString();
        }
    }
    public void EnchantJamCountPlus()
    {
        if (Database.Inst.playData.inventory[0].amount > enchantJam)
        {
            enchantJam++;
            Inventoryback.transform.Find("EnchantEnter/JamIcon1/CountBGI/Count").GetComponent<UILabel>().text = enchantJam.ToString();
        }
    }
    public void EnchantJamCountMax()
    {
        Inventoryback.transform.Find("EnchantEnter/JamIcon1/CountBGI/Count").GetComponent<UILabel>().text = enchantJam.ToString();
    }
    public void InventorySort(int sortmethods, GUITestScrollView scrollview)
    {
        if (sortmethods.Equals(0))
        {
            Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
            {
                if (a.num > b.num)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
        }
        else if (sortmethods.Equals(1))
        {
            Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
            {
                if (a.rarity > b.rarity)
                {
                    return 1;
                }
                else if (a.rarity.Equals(b.rarity))
                {
                    if (a.item_Class > b.item_Class)
                    {
                        return 1;
                    }
                    else if (a.item_Class.Equals(b.item_Class))
                    {
                        return a.num.CompareTo(b.num);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            });
        }
        else if (sortmethods.Equals(2))
        {
            Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
            {
                if (a.item_Class > b.item_Class)
                {
                    return 1;
                }
                else if (a.item_Class.Equals(b.item_Class))
                {
                    if (a.rarity > b.rarity)
                    {
                        return 1;
                    }
                    else if (a.rarity.Equals(b.rarity))
                    {
                        return a.num.CompareTo(b.num);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            });
        }
        scrollview.EV_UpdateAll();
    }
    public void togglevaluechageitemclass()
    {
        Debug.Log(UIToggle.current.gameObject.GetComponent<UISprite>().spriteName);
        if (UIToggle.current.gameObject.GetComponent<UISprite>().spriteName.Equals("test") && UIToggle.current.value)
        {
            itemclassselect = ItemState.검;
            Debug.Log(UIToggle.current.transform.parent.parent.name);
        }
        else if (UIToggle.current.gameObject.GetComponent<UISprite>().spriteName.Equals("제작아이콘") && UIToggle.current.value)
        {
            itemclassselect = ItemState.활;
            Debug.Log(UIToggle.current.transform.parent.parent.name);
        }
        else if (UIToggle.current.gameObject.GetComponent<UISprite>().spriteName.Equals("장비아이콘") && UIToggle.current.value)
        {
            itemclassselect = ItemState.지팡이;
            Debug.Log(UIToggle.current.transform.parent.parent.name);
        }
        else if (UIToggle.current.gameObject.GetComponent<UISprite>().spriteName.Equals("채집아이콘") && UIToggle.current.value)
        {
            itemclassselect = ItemState.갑옷;
            Debug.Log(UIToggle.current.transform.parent.parent.name);
        }
        if (lobbystate.Equals(LobbyState.Nomal))
        {
            scrollview.GetComponent<GUITestScrollView>().EV_UpdateAll();
        }
        else if (lobbystate.Equals(LobbyState.Enchant))
        {
            Inventoryback.transform.Find("Enchantpanel/ScrollView").GetComponent<GUITestScrollView>().EV_UpdateAll();
        }
        else if (lobbystate.Equals(LobbyState.Decomposition))
        {
            Inventoryback.transform.Find("Decomposition/ScrollView").GetComponent<GUITestScrollView>().EV_UpdateAll();
        }
    }
    public void togglevaluechageitemrarity()
    {
        Debug.Log(UIToggle.current.gameObject.GetComponent<UISprite>().spriteName);
        if (UIToggle.current.gameObject.GetComponent<UISprite>().spriteName.Equals("레어도_노말") && UIToggle.current.value)
        {
            ItemRarityselect = ItemRarity.노말;
            Debug.Log(UIToggle.current.transform.parent.parent.name);
        }
        else if (UIToggle.current.gameObject.GetComponent<UISprite>().spriteName.Equals("레어도_레어") && UIToggle.current.value)
        {
            ItemRarityselect = ItemRarity.레어;
            Debug.Log(UIToggle.current.transform.parent.parent.name);
        }
        else if (UIToggle.current.gameObject.GetComponent<UISprite>().spriteName.Equals("레어도_레전드") && UIToggle.current.value)
        {
            ItemRarityselect = ItemRarity.레전드;
            Debug.Log(UIToggle.current.transform.parent.parent.name);
        }
        else if (UIToggle.current.gameObject.GetComponent<UISprite>().spriteName.Equals("레어도_유니크") && UIToggle.current.value)
        {
            ItemRarityselect = ItemRarity.유니크;
            Debug.Log(UIToggle.current.transform.parent.parent.name);
        }
        if (lobbystate.Equals(LobbyState.Nomal))
        {
            scrollview.GetComponent<GUITestScrollView>().EV_UpdateAll();
        }
        else if (lobbystate.Equals(LobbyState.Enchant))
        {
            Inventoryback.transform.Find("Enchantpanel/ScrollView").GetComponent<GUITestScrollView>().EV_UpdateAll();
        }
        else if (lobbystate.Equals(LobbyState.Decomposition))
        {
            Inventoryback.transform.Find("Decomposition/ScrollView").GetComponent<GUITestScrollView>().EV_UpdateAll();
        }
    }
    public void sortbutton()
    {
        switch (sortSelect)
        {
            case 0:
                sortSelect++;
                UIButton.current.GetComponent<UILabel>().text = "등급순";
                break;
            case 1:
                sortSelect++;
                UIButton.current.GetComponent<UILabel>().text = "종류순";
                break;
            case 2:
                sortSelect = 0;
                UIButton.current.GetComponent<UILabel>().text = "획득순";
                break;
            default:
                break;
        }
    }

}