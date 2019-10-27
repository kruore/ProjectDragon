using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum LobbyState { Nomal, Enchant, Lock, Decomposition }
public enum ItemState { 기본, 검, 활, 지팡이, 갑옷 }
public enum ItemRarity { 기본, 노말, 레어, 유니크, 레전드 }
public class LobbyManager : MonoBehaviour
{
    public static LobbyManager inst;
    bool isnight = true;
    GameObject particle;
    public GameObject fireobject, playeranimation, playerimg;
    public GameObject filterobject;
    public AudioClip fire;
    public UISpriteAnimation anim;
    #region equipobject
    public GameObject changeEquip, BGID, BGI;
    public GameObject Inventoryback, Currentweapon, CurrentArmor, CurrentActive, equipCharactor, scrollview;
    public int changeequipdata, currentEquipdata, selectData, enchantJam, m_sortSelect;
    public LobbyState lobbystate = LobbyState.Nomal;
    public List<int> Selecteditem;
    public ItemState itemclassselect;
    public ItemRarity ItemRarityselect;
    public float sizeoption=10;
    public List<GUITestScrollView> gUITestScrollViews;
    public static string teststring;
    public int[] jamcounts = new int[3];
    double distance;
    string test = "";
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
        Debug.Log(DataTransaction.Inst.gameObject.name);

    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Inst.ScreensizeReadjust();
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
        Selecteditem = new List<int>();
        sizeoption = 2;
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
        //Currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = item.stat.ToString();
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        CurrentActive.transform.Find("IconIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].skill_Index].imageName;
        //GameObject.Find("UI Root/LobbyPanel/Player/HPCountBG/HP").GetComponent<UISprite>().fillAmount =Database.Inst.playData.currentHp/Database.Inst.playData.;
        //GameObject.Find("UI Root/LobbyPanel/Player/Mana/ManaCountBG/ManaCountLabel").GetComponent<UILabel>().text=


        //테스트용 코드
        //GameManager.Inst.Scenestack.Push("Enchant");
        //lobbystate = LobbyState.Decomposition;

    }
    // Update is called once per frame
    void Update()
    {


        //터치시 파티클생성
        if (Input.GetMouseButtonDown(0))
        {
            Camera camera11 = GameObject.Find("UI Root/Camera").gameObject.GetComponent<Camera>();
            Vector3 touchpoint011 = camera11.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawRay(touchpoint011, transform.forward * 10, Color.red, 0.3f);
            RaycastHit2D hit0 = Physics2D.Raycast(touchpoint011, transform.forward, 10);
            if (hit0)
            {
                Debug.Log(hit0.transform.gameObject.name);
            }
            test = "mousebuttondown0";
        }
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

            test = "mousebuttonup";
        }
        if (Input.touchCount.Equals(1))
        {
            distance = -1;
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
            test = "TouchCount.Equal1";
        }

        else if (Input.touchCount.Equals(2))
        {
            Camera camera = GameObject.Find("UI Root/Camera").gameObject.GetComponent<Camera>();
            Vector3 touchpoint0 = camera.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector3 touchpoint1 = camera.ScreenToWorldPoint(Input.GetTouch(1).position);
            
            RaycastHit2D hit0 = Physics2D.Raycast(touchpoint0, transform.forward, 10);
            RaycastHit2D hit1 = Physics2D.Raycast(touchpoint1, transform.forward, 10);
            if (hit0 && hit1)
            {

                if (hit0.transform.gameObject.name.Equals("playersizebox") && hit1.transform.gameObject.name.Equals("playersizebox"))
                {
                    if (distance.Equals(-1))
                    {
                        distance = Vector3.Distance(touchpoint0, touchpoint1);
                        Debug.Log("call");
                    }
                    else
                    {
                        if (distance < Vector3.Distance(touchpoint0, touchpoint1))
                        {
                            float currentdistance = Vector3.Distance(touchpoint0, touchpoint1);
                            Vector3 imagescale = new Vector3(playerimg.transform.localScale.x - ((float)distance - currentdistance)/sizeoption, playerimg.transform.localScale.y - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.z - ((float)distance - currentdistance) / sizeoption);
                            if (imagescale.x <= 1.5 && imagescale.y <= 1.5)
                            {
                                playerimg.transform.localScale = imagescale;
                                
                                Debug.Log(imagescale.x + " ," + imagescale.y + " ," + imagescale.z);
                            }
                            else
                            {
                                playerimg.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                                Debug.Log("one"+imagescale.x + " ," + imagescale.y + " ," + imagescale.z);
                            }
                            test = string.Format("touchpoint0:{0},{1},{2}\n touchpint1:{3},{4},{5}\n distance:{6}\n currentdistance:{7}", touchpoint0.x, touchpoint0.y, touchpoint0.z, touchpoint1.x, touchpoint1.y, touchpoint1.z, distance, (((float)distance - Vector3.Distance(touchpoint0, touchpoint1)) / sizeoption));
                            distance = Vector3.Distance(touchpoint0, touchpoint1);
                        }
                        else if (distance > Vector3.Distance(touchpoint0, touchpoint1))
                        {
                            float currentdistance = Vector3.Distance(touchpoint0, touchpoint1);
                            Vector3 imagescale = new Vector3(playerimg.transform.localScale.x - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.y - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.z -((float)distance - currentdistance) / sizeoption);
                            if (imagescale.x > 0.5 && imagescale.y > 0.5)
                            {
                                playerimg.transform.localScale = imagescale;
                                Debug.Log(imagescale.x+" ," +imagescale.y + " ," +imagescale.z);
                            }
                            else
                            {
                                playerimg.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                                Debug.Log(imagescale.x + " ," + imagescale.y + " ," + imagescale.z+"half");
                            }
                            test = string.Format("touchpoint0:{0},{1},{2}\n touchpint1:{3},{4},{5}\n distance:{6}\n currentdistance:{7}", touchpoint0.x, touchpoint0.y, touchpoint0.z, touchpoint1.x, touchpoint1.y, touchpoint1.z, distance, (((float)distance - Vector3.Distance(touchpoint0, touchpoint1)) / sizeoption));
                            distance = Vector3.Distance(touchpoint0, touchpoint1);
                        }
                    }
                }
            }
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended))
            {
                distance = -1;
            }
        }
        //GameObject.Find("Panel/Label").GetComponent<UILabel>().text = test;
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
        CurrentActive.transform.Find("IconIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].imageName;
        UpdateAllScrollview();
        DataTransaction.Inst.SavePlayerData();
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
            UIButton.current.transform.parent.Find("EquipBGI/IsLock").GetComponent<UISprite>().spriteName = "Lock";
        }
        else
        {
            UIButton.current.normalSprite = "Unlock";
            UIButton.current.hoverSprite = "Unlock";
            UIButton.current.transform.parent.Find("EquipBGI/IsLock").GetComponent<UISprite>().spriteName = "Unlock";
        }
        ChangeItemIcon(Currentweapon, Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum]);
        UpdateAllScrollview();
        DataTransaction.Inst.SavePlayerData();
    }
    public void ChangeEquipLock()
    {
        Database.Inst.playData.inventory[changeequipdata].isLock = !Database.Inst.playData.inventory[changeequipdata].isLock;
        if (Database.Inst.playData.inventory[changeequipdata].isLock)
        {
            UIButton.current.normalSprite = "Lock";
            UIButton.current.hoverSprite = "Lock";
            UIButton.current.transform.parent.Find("EquipBGI/IsLock").GetComponent<UISprite>().spriteName = "Lock";
        }
        else
        {
            UIButton.current.normalSprite = "Unlock";
            UIButton.current.hoverSprite = "Unlock";
            UIButton.current.transform.parent.Find("EquipBGI/IsLock").GetComponent<UISprite>().spriteName = "Unlock";
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
        for (int i = 1; i <= 3; i++)
        {
            GameObject jamcount;
            jamcount = Inventoryback.transform.Find(string.Format("Enchantpanel/Jamcount/Jam{0}", i)).gameObject;
            jamcount.GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[i - 1].amount);
            jamcounts[i - 1] = Database.Inst.playData.inventory[i - 1].amount;
        }
    }
    public void DecompositionState()
    {
        GameManager.Inst.Scenestack.Push("Decomposition");
        lobbystate = LobbyState.Decomposition;
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
        for (int i = 1; i <= 3; i++)
        {
            GameObject jamcount;
            jamcount = Inventoryback.transform.Find(string.Format("Decomposition/Jamcount/Jam{0}", i)).gameObject;
            jamcount.GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[i - 1].amount);
            jamcounts[i - 1] = Database.Inst.playData.inventory[i - 1].amount;
        }
        Debug.Log(jamcounts[0]);
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
    public void togglevalueChangeitemclass()
    {
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
    public void togglevalueChangeitemrarity()
    {
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
    public void DecompositionConfirm()
    {
        Debug.Log(Database.Inst.playData.inventory.Count);
        for (int i = Selecteditem.Count - 1; i >= 0; i--)
        {
            Debug.Log(i);
            //DataTransaction.Inst.Delete_Inventory_Item(Selecteditem[i]);
        }
        Debug.Log(Database.Inst.playData.inventory.Count);
        ButtonManager.TouchBackButton();
        for (int i = 0; i < 3; i++)
        {
            Database.Inst.playData.inventory[i].amount = jamcounts[i];
        }
        DataTransaction.Inst.SavePlayerData();
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        ChangeItemIcon(Currentweapon, item);
        Currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = item.stat.ToString();
    }
    public void QuitApplicationinlobby()
    {
        DataTransaction.Inst.SavePlayerData();
        Application.Quit();
    }
    public void QuitApplicationinlobbyCancle()
    {
        GameObject.Find("UI Root/LobbyPanel/Quit").SetActive(false);
    }
    public void GotoBattle()
    {
        SceneManager.LoadScene("Battle");
    }
}