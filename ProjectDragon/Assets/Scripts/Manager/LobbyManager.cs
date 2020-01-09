﻿//////////////////////////////////////////////////////////MADE BY Koo KyoSeok///2019-12-16/////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum LobbyState { Nomal, Enchant, Lock, Decomposition }
public enum ItemState { 기본, 검, 활, 지팡이, 갑옷 }
public enum ItemRarity { 기본, 노말, 레어, 유니크, 레전드 }
public class LobbyManager : MonoBehaviour
{
    bool statepanelbutton;
    public AudioClip LobbyBGM;
    public string BattleScenename;
    public static LobbyManager inst;
    bool isnight = true;
    GameObject particle;
    public GameObject fireobject, playeranimation, playerimg, equipCharactor, playerStat, equipanel;
    public GameObject filterobject;
    public AudioClip fire;
    public GameObject Statpanel, EquipStatPanel;
    #region equipobject
    public GameObject changeEquip, BGID, BGI;
    public GameObject Inventoryback, Currentweapon, CurrentArmor, CurrentActive, scrollview, EuiptIcons;
    public List<UISprite> Weapon, Armor, weaponicon, armoricon, activeicon;
    public int changeequipdata, currentEquipdata, selectData = -1, enchantJam, m_sortSelect;
    public LobbyState lobbystate = LobbyState.Nomal;
    public List<int> Selecteditem;
    public ItemState itemclassselect;
    public ItemRarity ItemRarityselect;
    public float sizeoption = 2;
    public List<GUITestScrollView> gUITestScrollViews;
    public static string teststring;
    //public int[] jamcounts = new int[3];
    string classname = "null";
    double distance;
    //public string test = "", test2 = "";
    public UILabel testlabel, testlabel2;
    public UILabel fontchecklabel;
    public int fontcheck = 0;
    public int sortSelect
    {
        get
        {
            return m_sortSelect;
        }
        set
        {
            m_sortSelect = value;
            //InventorySort(m_sortSelect, scrollview.GetComponent<GUITestScrollView>());
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
        GameManager.Inst.name.ToString();
        GameManager.Inst.gameObject.ToString();
        SoundManager.Inst.Ds_BGMPlayerDB(2);
        LobbyObjectSet();
        LobbyStateInit();
        selectData = -1;
        testlabel = GameObject.Find("test").GetComponent<UILabel>();
        testlabel2 = GameObject.Find("test2").GetComponent<UILabel>();
        SetplayerStat();

        //player 체력,마나,스텟 조정
        //playerStat = GameObject.Find("UI Root/LobbyPanel/IconBackgroundU/Player");
        //playerStat.transform.Find("HPCountBG/HP").GetComponent<UISprite>().fillAmount = (float)GameManager.Inst.CurrentHp / (float)GameManager.Inst.MaxHp;
        //playerStat.transform.Find("HPCountBG/Label").GetComponent<UILabel>().text = string.Format("{0}/{1}", GameManager.Inst.CurrentHp, GameManager.Inst.MaxHp);
        //playerStat.transform.Find("Mana/ManaCountBG/ManaCountLabel").GetComponent<UILabel>().text = Database.Inst.playData.mp.ToString();
        //GameManager.Inst.QuitObject = GameObject.Find("UI Root/QuitPanel").transform.Find("QuitBGI").gameObject;
        //SoundManager.Inst.Ds_BgmPlayer(LobbyBGM);
        //itemclassselect = ItemState.기본;
        //ItemRarityselect = ItemRarity.기본;
        //Selecteditem = new List<int>();
        //EuiptIcons = GameObject.Find(string.Format("UI Root/LobbyPanel/IconBackgroundU/EquipItem/EquipLabel"));
        //if (isnight)
        //{
        //    SoundManager.Inst.Ds_BgmPlayer(fire);
        //    fireobject.SetActive(true);
        //}
        //else
        //{
        //}

        //playerimg = GameObject.Find("UI Root/LobbyPanel/Statpanel/Panel/playersizebox/Character");
        //equipCharactor = GameObject.Find("UI Root").transform.Find("EquipPanel/Charactorpanel/playersizebox/Charactor").gameObject;
        //Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        //Weapon.Add(EuiptIcons.transform.Find("Equip1/WeaponImg").GetComponent<UISprite>());
        ////EuiptIcons.transform.Find("Equip1/WeaponImg").GetComponent<UISprite>().spriteName = item.imageName;
        //if (item.Class.Equals(CLASS.검))
        //{
        //    classname = "Worrior";
        //    if (Database.Inst.playData.sex.Equals(SEX.Male))
        //    {
        //        playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
        //        equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
        //    }
        //    else
        //    {
        //        playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
        //        equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
        //    }
        //}
        //else if (item.Class.Equals(CLASS.활))
        //{
        //    classname = "Archer";

        //}
        //else if (item.Class.Equals(CLASS.지팡이))
        //{
        //    classname = "Wizard";
        //}

        //string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);
        //playerimg.GetComponent<UITexture>().mainTexture = Resources.Load<Texture2D>(playerclass);
        //equipCharactor.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        //playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        //ChangeItemIcon(Currentweapon, item);
        ////Currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = item.stat.ToString();
        //Weapon.Add(playerimg.transform.Find("Weapon").GetComponent<UISprite>());
        //Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/SwordIcon/ToggleBox").GetComponent<UIToggle>().activeSprite = Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/SwordIcon/ToggleBox/X").GetComponent<UISprite>();
        //Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/BowIcon/ToggleBox").GetComponent<UIToggle>().activeSprite = Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/BowIcon/ToggleBox/X").GetComponent<UISprite>();
        //Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/ArmorIcon/ToggleBox").GetComponent<UIToggle>().activeSprite = Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/ArmorIcon/ToggleBox/X").GetComponent<UISprite>();
        //Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/WandIcon/ToggleBox").GetComponent<UIToggle>().activeSprite = Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/WandIcon/ToggleBox/X").GetComponent<UISprite>();
        //Weapon.Add(equipCharactor.transform.Find("Weapon").GetComponent<UISprite>());
        //CurrentActive.transform.Find("IconIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].skill_Index].imageName;
        //GameObject.Find("UI Root/LobbyPanel/Player/HPCountBG/HP").GetComponent<UISprite>().fillAmount =Database.Inst.playData.currentHp/Database.Inst.playData.;
        //GameObject.Find("UI Root/LobbyPanel/Player/Mana/ManaCountBG/ManaCountLabel").GetComponent<UILabel>().text=
        //SetWeapon();

        ////테스트용 코드
        ////GameManager.Inst.Scenestack.Push("Enchant");
        ////lobbystate = LobbyState.Decomposition;

    }
    /// <summary>
    /// 로비에서의 오브젝트 관리
    /// </summary>
    public void LobbyObjectSet()
    {
        fireobject = GameObject.Find(string.Format("UI Root/LobbyPanel/LobbyBGI/Fire"));
        playeranimation = GameObject.Find(string.Format("UI Root/LobbyPanel/LobbyBGI/PlayerAnimation"));
        playerimg = GameObject.Find(string.Format("UI Root/LobbyPanel/Statpanel/Panel/playersizebox/Character"));
        equipanel = equipCharactor = GameObject.Find(string.Format("UI Root")).transform.Find(string.Format("EquipPanel")).gameObject;
        equipCharactor = equipanel.transform.Find(string.Format("Charactorpanel/playersizebox/Charactor")).gameObject;
        Statpanel = GameObject.Find(string.Format("UI Root/LobbyPanel/Statpanel/Sprite"));
        Inventoryback = equipanel.transform.Find(string.Format("Inventoryback")).gameObject;
        BGID = Inventoryback.transform.Find(string.Format("BGIPanel/Texture")).gameObject;
        BGI = Inventoryback.transform.Find(string.Format("BGIPanel/BGI")).gameObject;
        CurrentActive = equipanel.transform.Find(string.Format("Charactorpanel/ActiveIconBGI")).gameObject;
        CurrentArmor = equipanel.transform.Find(string.Format("Charactorpanel/ArmorIconBGI")).gameObject;
        Currentweapon = equipanel.transform.Find(string.Format("Charactorpanel/EquipWeaponIconBGI")).gameObject;
        scrollview = Inventoryback.transform.Find("ItemWindow/ScrollView").gameObject;
        EuiptIcons = GameObject.Find(string.Format("UI Root/LobbyPanel/IconBackgroundU/EquipItem/EquipLabel"));
        playerStat = GameObject.Find(string.Format("UI Root/LobbyPanel/IconBackgroundU/Player"));
        EquipStatPanel = equipanel.transform.Find(string.Format("StatBGI")).gameObject;
        FontChangeAll();
    }
    public void ButtonSound1()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
    }
    public void FontChange(UILabel _label,Font _font)
    {

        Transform[] objects = GameObject.Find("UI Root").GetComponentsInChildren<Transform>(true);
        if (_label != null)
        {
            _label.GetComponent<UILabel>().trueTypeFont = _font;
        }
    }
    public void FontChangeAll()
    {
        if (fontcheck < 13)
        {
            fontcheck++;
        }
        else
        {
            fontcheck = 0;
        }
        UILabel[] labels;
        Transform[] objects = GameObject.Find("UI Root").GetComponentsInChildren<Transform>(true);
        labels = GetComponentsInChildren<UILabel>();

        GameObject font = Resources.Load<GameObject>(string.Format("Font/Main", fontcheck));
        Debug.Log(font.GetComponent<UILabel>().fontStyle);
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.GetComponent<UILabel>() != null)
            {
                objects[i].gameObject.GetComponent<UILabel>().trueTypeFont = font.GetComponent<UILabel>().trueTypeFont;
            }
        }
    }
    /// <summary>
    /// 로비 초기화
    /// </summary>
    public void LobbyStateInit()
    {
        GameObject font = Resources.Load<GameObject>(string.Format("Font/Icon", fontcheck));
        UILabel[] buttonIconhead=GameObject.Find("UI Root/LobbyPanel/IconBackgroundR").GetComponentsInChildren<UILabel>();
        for(int i=0;i<buttonIconhead.Length;i++)
        {
            FontChange(buttonIconhead[i],font.GetComponent<UILabel>().trueTypeFont);
        }
        //player 체력,마나,스텟 조정
        playerStat.transform.Find("HPCountBG/HP").GetComponent<UISprite>().fillAmount = (float)GameManager.Inst.CurrentHp / (float)GameManager.Inst.MaxHp;
        playerStat.transform.Find("HPCountBG/Label").GetComponent<UILabel>().text = string.Format("{0}/{1}", GameManager.Inst.CurrentHp, GameManager.Inst.MaxHp);
        playerStat.transform.Find("Mana/ManaCountBG/ManaCountLabel").GetComponent<UILabel>().text = Database.Inst.playData.mp.ToString();

        //SoundManager.Inst.Ds_BgmPlayer(LobbyBGM);
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
        Selecteditem = new List<int>();
        if (isnight)
        {
            //SoundManager.Inst.Ds_BgmPlayer(fire);
            fireobject.SetActive(true);
        }
        else
        {
        }
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        Weapon.Add(EuiptIcons.transform.Find("Equip1/WeaponImg").GetComponent<UISprite>());
        //EuiptIcons.transform.Find("Equip1/WeaponImg").GetComponent<UISprite>().spriteName = item.imageName;
        if (item.Class.Equals(CLASS.검))
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
        else if (item.Class.Equals(CLASS.활))
        {
            classname = "Archer";

        }
        else if (item.Class.Equals(CLASS.지팡이))
        {
            classname = "Wizard";
        }
        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);
        
        playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        ChangeItemIcon(Currentweapon, item);
        Weapon.Add(playerimg.transform.Find("Weapon").GetComponent<UISprite>());
        Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/SwordIcon/ToggleBox").GetComponent<UIToggle>().activeSprite = Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/SwordIcon/ToggleBox/X").GetComponent<UISprite>();
        Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/BowIcon/ToggleBox").GetComponent<UIToggle>().activeSprite = Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/BowIcon/ToggleBox/X").GetComponent<UISprite>();
        Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/ArmorIcon/ToggleBox").GetComponent<UIToggle>().activeSprite = Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/ArmorIcon/ToggleBox/X").GetComponent<UISprite>();
        Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/WandIcon/ToggleBox").GetComponent<UIToggle>().activeSprite = Inventoryback.transform.Find("ArrangementButtons/FilterButtonBGI/OptionPanel/WandIcon/ToggleBox/X").GetComponent<UISprite>();
        Weapon.Add(equipCharactor.transform.Find("Weapon").GetComponent<UISprite>());
        CharactorSkinSet(playerclass);
        SetWeapon();
        statepanelbutton = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (charactormovetime > movetime)
        //{
        //    charactormovedirection = false;
        //}
        //else if(charactormovetime<0)
        //{
        //    charactormovedirection = true;
        //}
        //if(charactormovedirection)
        //{
        //    charactormovetime += Time.deltaTime;
        //}
        //else
        //{
        //    charactormovetime -= Time.deltaTime;
        //}
        //Debug.Log(charactormovetime);
        //playerimg.transform.localPosition = new Vector3(0,  - ((charactormovetime / movetime)* moveposition), 0);
        //equipCharactor.transform.localPosition = new Vector3(0,  - ((charactormovetime / movetime)* moveposition), 0);

        //터치시 파티클생성
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchpoint011 = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawRay(touchpoint011, transform.forward * 10, Color.red, 0.3f);
            RaycastHit2D hit0 = Physics2D.Raycast(touchpoint011, transform.forward, 10);
            if (hit0)
            {
                Debug.Log(hit0.transform.gameObject.name);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (particle != null)
            {
                Destroy(particle);
            }
            particle = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Star_A"));
            particle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            particle.transform.parent = GameObject.Find("UI Root/Panel").transform;
            Vector3 wp = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
            particle.transform.position = wp;
            Destroy(particle, 0.5f);
        }
        if (Input.touchCount > 0)
        {
            if (Input.touchCount.Equals(1))
            {
                distance = -1;
                if (particle != null)
                {
                    Destroy(particle);
                }
                particle = Instantiate<GameObject>(Resources.Load<GameObject>("UI/Star_A"));
                particle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                particle.transform.parent = GameObject.Find("UI Root/Panel").transform;
                Vector3 wp = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, -1));
                particle.transform.position = wp;
                Destroy(particle, 0.5f);
            }

            else if (Input.touchCount.Equals(2))
            {
                Vector3 touchpoint0 = UICamera.currentCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector3 touchpoint1 = UICamera.currentCamera.ScreenToWorldPoint(Input.GetTouch(1).position);

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
                                Vector3 imagescale = new Vector3(playerimg.transform.localScale.x - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.y - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.z - ((float)distance - currentdistance) / sizeoption);
                                if (imagescale.x <= 1.5 && imagescale.y <= 1.5)
                                {
                                    playerimg.transform.localScale = imagescale;
                                    equipCharactor.transform.localScale = imagescale;
                                    Debug.Log(imagescale.x + " ," + imagescale.y + " ," + imagescale.z);
                                }
                                else
                                {
                                    playerimg.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                                    equipCharactor.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                                    Debug.Log("one" + imagescale.x + " ," + imagescale.y + " ," + imagescale.z);
                                }
                                distance = Vector3.Distance(touchpoint0, touchpoint1);
                            }
                            else if (distance > Vector3.Distance(touchpoint0, touchpoint1))
                            {
                                float currentdistance = Vector3.Distance(touchpoint0, touchpoint1);
                                Vector3 imagescale = new Vector3(playerimg.transform.localScale.x - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.y - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.z - ((float)distance - currentdistance) / sizeoption);
                                if (imagescale.x > 0.5 && imagescale.y > 0.5)
                                {
                                    playerimg.transform.localScale = imagescale;
                                    equipCharactor.transform.localScale = imagescale;
                                    Debug.Log(imagescale.x + " ," + imagescale.y + " ," + imagescale.z);
                                }
                                else
                                {
                                    playerimg.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                                    equipCharactor.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                                    Debug.Log(imagescale.x + " ," + imagescale.y + " ," + imagescale.z + "half");
                                }
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
        }
        //GameObject.Find("Panel/Label").GetComponent<UILabel>().text = test;
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }
    //public void OptionPanel()
    //{
    //    UIButton.current.transform.Find("OptionPanel").gameObject.SetActive(true);
    //}
    public void StatpanelSetup()
    {
        if (statepanelbutton)
        {
            Statpanel.GetComponent<TweenPosition>().from = Statpanel.transform.localPosition;
            Statpanel.GetComponent<TweenPosition>().to = Statpanel.transform.localPosition;
            Statpanel.GetComponent<TweenPosition>().to.x = Statpanel.GetComponent<TweenPosition>().to.x + Statpanel.transform.Find("StatBGI").GetComponent<UISprite>().localSize.x - Statpanel.GetComponent<UISprite>().localSize.x;
            Debug.Log(Statpanel.GetComponent<TweenPosition>().from.x + "::" + Statpanel.GetComponent<TweenPosition>().to.x.ToString() + "::" + Statpanel.transform.Find("StatBGI").GetComponent<UISprite>().localSize.x);
            Debug.Log(Statpanel.GetComponent<TweenPosition>().method.ToString());
            statepanelbutton = false;

        }
    }
    public void CharactorSkinSet(string playerclass)
    {
        Texture2D Skin= Resources.Load<Texture2D>(playerclass);
        playerimg.GetComponent<UITexture>().mainTexture = Skin;
        equipCharactor.GetComponent<UITexture>().mainTexture = Skin;
        GameObject.Find("UI Root").transform.Find("Skin/playersizebox/Charactor").GetComponent<UITexture>().mainTexture = Skin;
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
        if (Database.Inst.playData.inventory[changeequipdata].Class.Equals(CLASS.갑옷))
        {
            Database.Inst.playData.equiArmor_InventoryNum = changeequipdata;
        }
        string classname = "null";
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        if (item.Class.Equals(CLASS.검))
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
        else if (item.Class.Equals(CLASS.활))
        {
            classname = "Archer";
        }
        else if (item.Class.Equals(CLASS.지팡이))
        {
            classname = "Wizard";
        }
        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);
        playerimg.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        equipCharactor.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        ChangeItemIcon(Currentweapon, item);
        Currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = Database.Inst.weapons[item.DB_Num].atk_Min.ToString();
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        CurrentActive.transform.Find("IconIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].imageName;
        UpdateAllScrollview();
        GameManager.Inst.SavePlayerData();
    }
    public void ChangeItemIcon(GameObject Icon, Database.Inventory data)
    {
        Icon.transform.Find("EquipIcon").GetComponent<UISprite>().spriteName = data.imageName;

        //if (data.isLock)
        //{
        //    Icon.transform.Find("IsLock").GetComponent<UISprite>().spriteName = "Lock";
        //}
        //else
        //{
        //    Icon.transform.Find("IsLock").GetComponent<UISprite>().spriteName = "Unlock";
        //}
    }
    //public void CurrentEquipLock()
    //{
    //    Database.Inst.playData.inventory[currentEquipdata].isLock = !Database.Inst.playData.inventory[currentEquipdata].isLock;
    //    if (Database.Inst.playData.inventory[currentEquipdata].isLock)
    //    {
    //        UIButton.current.normalSprite = "Lock";
    //        UIButton.current.hoverSprite = "Lock";
    //        UIButton.current.transform.parent.Find("EquipBGI/IsLock").GetComponent<UISprite>().spriteName = "Lock";
    //    }
    //    else
    //    {
    //        UIButton.current.normalSprite = "Unlock";
    //        UIButton.current.hoverSprite = "Unlock";
    //        UIButton.current.transform.parent.Find("EquipBGI/IsLock").GetComponent<UISprite>().spriteName = "Unlock";
    //    }
    //    ChangeItemIcon(Currentweapon, Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum]);
    //    UpdateAllScrollview();
    //    GameManager.Inst.SavePlayerData();
    //}
    //public void ChangeEquipLock()
    //{
    //    Database.Inst.playData.inventory[changeequipdata].isLock = !Database.Inst.playData.inventory[changeequipdata].isLock;
    //    if (Database.Inst.playData.inventory[changeequipdata].isLock)
    //    {
    //        UIButton.current.normalSprite = "Lock";
    //        UIButton.current.hoverSprite = "Lock";
    //        UIButton.current.transform.parent.Find("EquipBGI/IsLock").GetComponent<UISprite>().spriteName = "Lock";
    //    }
    //    else
    //    {
    //        UIButton.current.normalSprite = "Unlock";
    //        UIButton.current.hoverSprite = "Unlock";
    //        UIButton.current.transform.parent.Find("EquipBGI/IsLock").GetComponent<UISprite>().spriteName = "Unlock";
    //    }
    //    UpdateAllScrollview();
    //    GameManager.Inst.SavePlayerData();
    //}
    public void UpdateAllScrollview()
    {
        foreach (GUITestScrollView scrollView in gUITestScrollViews)
        {
            scrollView.EV_UpdateAll();
            Debug.Log(scrollView.gameObject.name);
        }

    }
    //public void ListLock()
    //{
    //    foreach (int item in Selecteditem)
    //    {
    //        Database.Inst.playData.inventory[item].isLock = !Database.Inst.playData.inventory[item].isLock;
    //    }
    //    Selecteditem.Clear();
    //    lobbystate = LobbyState.Nomal;
    //    UpdateAllScrollview();
    //    TouchBackButton();
    //    GameManager.Inst.SavePlayerData();

    //}
    public void SkinState()
    {
        GameManager.Inst.Scenestack.Push("Skin");
    }
    public void EnchantState()
    {
        GameManager.Inst.Scenestack.Push("Enchant");
        lobbystate = LobbyState.Enchant;
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
    }
    public void QuitState()
    {
        GameManager.Inst.Scenestack.Push("Quit");
    }
    //public void DecompositionState()
    //{
    //    GameManager.Inst.Scenestack.Push("Decomposition");
    //    lobbystate = LobbyState.Decomposition;
    //    itemclassselect = ItemState.기본;
    //    ItemRarityselect = ItemRarity.기본;
    //    for (int i = 1; i <= 3; i++)
    //    {
    //        GameObject jamcount;
    //        jamcount = Inventoryback.transform.Find(string.Format("Decomposition/Jamcount/Jam{0}", i)).gameObject;
    //        jamcount.GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[i - 1].amount);
    //        jamcounts[i - 1] = Database.Inst.playData.inventory[i - 1].amount;
    //    }
    //    Debug.Log(jamcounts[0]);
    //}
    public void NomalState()
    {
        GameManager.Inst.Scenestack.Push("EquipPanel");
        Debug.Log(GameManager.Inst.Scenestack);
        lobbystate = LobbyState.Nomal;
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
    }
    public void QuitApplication()
    {
        ButtonManager.GameQuit();
    }
    //public void EnchantJamCountMinus()
    //{
    //    if (enchantJam > 0)
    //    {
    //        enchantJam--;
    //        Inventoryback.transform.Find("EnchantEnter/JamIcon1/CountBGI/Count").GetComponent<UILabel>().text = enchantJam.ToString();
    //    }
    //}
    //public void EnchantJamCountPlus()
    //{
    //    if (Database.Inst.playData.inventory[0].amount > enchantJam)
    //    {
    //        enchantJam++;
    //        Inventoryback.transform.Find("EnchantEnter/JamIcon1/CountBGI/Count").GetComponent<UILabel>().text = enchantJam.ToString();
    //    }
    //}
    //public void EnchantJamCountMax()
    //{
    //    Inventoryback.transform.Find("EnchantEnter/JamIcon1/CountBGI/Count").GetComponent<UILabel>().text = enchantJam.ToString();
    //}
    //public void InventorySort(int sortmethods, GUITestScrollView scrollview)
    //{
    //    if (sortmethods.Equals(0))
    //    {
    //        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
    //        {
    //            if (a.num > b.num)
    //            {
    //                return 1;
    //            }
    //            else
    //            {
    //                return -1;
    //            }
    //        });
    //    }
    //    else if (sortmethods.Equals(1))
    //    {
    //        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
    //        {
    //            if (a.rarity > b.rarity)
    //            {
    //                return 1;
    //            }
    //            else if (a.rarity.Equals(b.rarity))
    //            {
    //                if (a.Class > b.Class)
    //                {
    //                    return 1;
    //                }
    //                else if (a.Class.Equals(b.Class))
    //                {
    //                    return a.num.CompareTo(b.num);
    //                }
    //                else
    //                {
    //                    return -1;
    //                }
    //            }
    //            else
    //            {
    //                return -1;
    //            }
    //        });
    //    }
    //    else if (sortmethods.Equals(2))
    //    {
    //        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
    //        {
    //            if (a.Class > b.Class)
    //            {
    //                return 1;
    //            }
    //            else if (a.Class.Equals(b.Class))
    //            {
    //                if (a.rarity > b.rarity)
    //                {
    //                    return 1;
    //                }
    //                else if (a.rarity.Equals(b.rarity))
    //                {
    //                    return a.num.CompareTo(b.num);
    //                }
    //                else
    //                {
    //                    return -1;
    //                }
    //            }
    //            else
    //            {
    //                return -1;
    //            }
    //        });
    //    }
    //    scrollview.EV_UpdateAll();
    //}
    public void inventoryAcquisitionorder()
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
        UpdateAllScrollview();
    }
    public void inventoryClassorder()
    {
        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
            {
                if (a.rarity > b.rarity)
                {
                    return 1;
                }
                else if (a.rarity.Equals(b.rarity))
                {
                    if (a.Class > b.Class)
                    {
                        return 1;
                    }
                    else if (a.Class.Equals(b.Class))
                    {
                        Debug.Log("EE");
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
        UpdateAllScrollview();
    }
    public void inventorytypeorder()
    {
        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
            {
                if (a.Class < b.Class)
                {
                    return 1;
                }
                else if (a.Class.Equals(b.Class))
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
        UpdateAllScrollview();
    }
    public void togglevalueChangeitemrarity()
    {

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
    //public void DecompositionConfirm()
    //{
    //    Debug.Log(Database.Inst.playData.inventory.Count);
    //    for (int i = Selecteditem.Count - 1; i >= 0; i--)
    //    {
    //        Debug.Log(i);
    //        //GameManager.Inst.Delete_Inventory_Item(Selecteditem[i]);
    //    }
    //    Debug.Log(Database.Inst.playData.inventory.Count);
    //    ButtonManager.TouchBackButton();
    //    for (int i = 0; i < 3; i++)
    //    {
    //        Database.Inst.playData.inventory[i].amount = jamcounts[i];
    //    }
    //    GameManager.Inst.SavePlayerData();
    //    Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
    //    ChangeItemIcon(Currentweapon, item);
    //    Currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = item.stat.ToString();
    //}
    public void QuitApplicationinlobby()
    {
        //GameManager.Inst.SavePlayerData();
        GameObject.Find("UI Root/QuitPanel").transform.Find("QuitBGI").gameObject.SetActive(true);
    }
    public void QuitApplicationinlobbyCancle()
    {
        GameObject.Find("UI Root/QuitPanel/Quit").SetActive(false);
    }
    /// <summary>
    /// 마나 조작시에 해야할 것
    /// </summary>
    /// <param name="_mana"></param>
    public void Manacontrol(int _mana)
    {
        Database.Inst.playData.mp -= _mana;
        playerStat.transform.Find("Mana/ManaCountBG/ManaCountLabel").GetComponent<UILabel>().text = Database.Inst.playData.mp.ToString();
    }
    /// <summary>
    /// 무기 변경시에 로비에서 바뀌어야 할 것
    /// </summary>
    public void SetWeapon()
    {
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        string classname = "null";
        if (item.Class.Equals(CLASS.검))
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
        else if (item.Class.Equals(CLASS.활))
        {
            classname = "Archer";

        }
        else if (item.Class.Equals(CLASS.지팡이))
        {
            classname = "Wizard";
        }
        //Assets / Resources / PlayerCharactor / Female_Worrior_Weapon.mat
        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);

        NGUIAtlas weaponatlas = Resources.Load<NGUIAtlas>(playerclass + "_Weapon");
        for (int i = 0; Weapon.Count > i; i++)
        {
            Weapon[i].atlas = weaponatlas;
            Weapon[i].spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].imageName;
        }
        UpdateAllScrollview();
        SetplayerStat();
    }
    /// <summary>
    /// 아머가 바뀌었을 시에 해야할 것
    /// </summary>
    public void SetArmor()
    {
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);
        UpdateAllScrollview();
    }
    public void Togglebox()
    {
        if (UIToggle.current.value)
        {
            switch (UIToggle.current.transform.parent.gameObject.name)
            {
                case "SwordIcon":
                    itemclassselect = ItemState.검;
                    Debug.Log("sword");
                    break;
                case "BowIcon":
                    itemclassselect = ItemState.활;
                    Debug.Log("bow");
                    break;
                case "ArmorIcon":
                    itemclassselect = ItemState.지팡이;
                    Debug.Log("armor");
                    break;
                case "WandIcon":
                    itemclassselect = ItemState.갑옷;
                    Debug.Log("wand");
                    break;
                default:
                    Debug.Log(UIToggle.current.transform.parent.gameObject.name);
                    break;
            }
            UpdateAllScrollview();
        }
    }
    //public void CurrentWeaponStat()
    //{
    //    Database.Weapon currentweapon = GameManager.Inst.CurrentEquipWeapon;
    //    GameObject optionBGI = UIButton.current.transform.Find("OptionBGI").gameObject;
    //    optionBGI.transform.Find("Name").GetComponent<UILabel>().text = currentweapon.name;
    //    optionBGI.transform.Find("Value").GetComponent<UILabel>().text = currentweapon.damage.ToString();
    //    optionBGI.transform.Find("Name").GetComponent<UILabel>().text = currentweapon.optionTableName;
    //}

    /// <summary>
    /// Statpanel업데이트
    /// </summary>
    public void SetplayerStat()
    {
        Statpanel.transform.Find(string.Format("StatBGI/Damage")).GetComponent<UILabel>().text = string.Format("공격력 : {0}", Database.Inst.playData.atk_Min);
        Statpanel.transform.Find(string.Format("StatBGI/HP")).GetComponent<UILabel>().text = string.Format("체력 :{0}", Database.Inst.playData.currentHp);
        Statpanel.transform.Find(string.Format("StatBGI/Defence")).GetComponent<UILabel>().text = string.Format("피해감소량 : {0}", Database.Inst.playData.mp);
        Statpanel.transform.Find(string.Format("StatBGI/AttackSpeed")).GetComponent<UILabel>().text = string.Format("공격속도:{0}", Database.Inst.playData.atk_Speed);

        GameObject EquipItem = GameObject.Find("UI Root/LobbyPanel/IconBackgroundU/EquipItem/EquipLabel");
        EquipItem.transform.Find(string.Format("Equip1/OptionBGI/Name")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name);
        EquipItem.transform.Find(string.Format("Equip1/OptionBGI/Value")).GetComponent<UILabel>().text = string.Format("공격력:{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].itemValue);
        EquipItem.transform.Find(string.Format("Equip1/OptionBGI/Option")).GetComponent<UILabel>().text = string.Format("옵션:{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].option_Index);

        EquipItem.transform.Find(string.Format("Equip2/OptionBGI/Name")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].name);
        EquipItem.transform.Find(string.Format("Equip2/OptionBGI/Value")).GetComponent<UILabel>().text = string.Format("체력:{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].itemValue);

        EquipItem.transform.Find(string.Format("Equip3/OptionBGI/Name")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].name);
        EquipItem.transform.Find(string.Format("Equip3/OptionBGI/Power")).GetComponent<UILabel>().text = string.Format("공격력:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].atk.ToString());
        EquipItem.transform.Find(string.Format("Equip3/OptionBGI/Mana")).GetComponent<UILabel>().text = string.Format("마나:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].mpCost.ToString());
        EquipItem.transform.Find(string.Format("Equip3/OptionBGI/CoolTime")).GetComponent<UILabel>().text = string.Format("쿨타임:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].coolTime.ToString());
        EquipItem.transform.Find(string.Format("Equip3/OptionBGI/Target")).GetComponent<UILabel>().text = string.Format("대상:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].skill_Range.ToString());
        EquipItem.transform.Find(string.Format("Equip3/OptionBGI/Discription")).GetComponent<UILabel>().text = string.Format("범위:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].description);


        EquipStatPanel.transform.Find("Damage").GetComponent<UILabel>().text = string.Format("공격력 : {0}", Database.Inst.playData.atk_Min);
        EquipStatPanel.transform.Find("HP").GetComponent<UILabel>().text = string.Format("체력 :{0}", Database.Inst.playData.currentHp);
        EquipStatPanel.transform.Find("Defence").GetComponent<UILabel>().text = string.Format("피해감소량 : {0}", Database.Inst.playData.mp);
        EquipStatPanel.transform.Find("AttackSpeed").GetComponent<UILabel>().text = string.Format("공격속도:{0}", Database.Inst.playData.atk_Speed);

        equipanel.transform.Find(string.Format("Charactorpanel/EquipWeaponIconBGI")).Find(string.Format("Stat/EquipItemName")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name);
        equipanel.transform.Find(string.Format("Charactorpanel/EquipWeaponIconBGI")).Find(string.Format("Stat/EquipItemStat")).GetComponent<UILabel>().text = string.Format("공격력:{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].itemValue.ToString());
        equipanel.transform.Find(string.Format("Charactorpanel/ArmorIconBGI")).Find(string.Format("Stat/EquipItemName")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].name);
        equipanel.transform.Find(string.Format("Charactorpanel/ArmorIconBGI")).Find(string.Format("Stat/EquipItemStat")).GetComponent<UILabel>().text = string.Format("체력:{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].itemValue.ToString());
        equipanel.transform.Find(string.Format("Charactorpanel/ActiveIconBGI")).Find(string.Format("Stat/ActiveName")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].name);
        equipanel.transform.Find(string.Format("Charactorpanel/ActiveIconBGI")).Find(string.Format("Stat/ActivePower")).GetComponent<UILabel>().text = string.Format("공격력:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].atk.ToString());
        equipanel.transform.Find(string.Format("Charactorpanel/ActiveIconBGI")).Find(string.Format("Stat/ActiveMana")).GetComponent<UILabel>().text = string.Format("마나:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].mpCost.ToString());
        equipanel.transform.Find(string.Format("Charactorpanel/ActiveIconBGI")).Find(string.Format("Stat/ActiveCoolTime")).GetComponent<UILabel>().text = string.Format("쿨타임:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].coolTime.ToString());
        equipanel.transform.Find(string.Format("Charactorpanel/ActiveIconBGI")).Find(string.Format("Stat/ActiveTarget")).GetComponent<UILabel>().text = string.Format("대상:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].skill_Range.ToString());
        equipanel.transform.Find(string.Format("Charactorpanel/ActiveIconBGI")).Find(string.Format("Stat/ActiveDiscription")).GetComponent<UILabel>().text = string.Format("범위:{0}", Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].description);

    }
    public void GotoBattle()
    {
        SceneManager.LoadScene(BattleScenename);
    }
    /// <summary>
    /// 스킨 움직임 버튼 재생 왼쪽클릭
    /// </summary>
    public void SkinLeft()
    {
        //애니메이션 재생
        //슬롯
        GameObject ItemWindow = GameObject.Find("UI Root/Skin/ItemWindow").gameObject;
        ItemWindow.GetComponent<Animator>().Play("LeftAnim");
    }
    /// <summary>
    /// 스킨 움직임 버튼 재생 오른쪽클릭
    /// </summary>
    public void SkinRight()
    {
        GameObject ItemWindow = GameObject.Find("UI Root/Skin/ItemWindow").gameObject;
        ItemWindow.GetComponent<Animator>().Play("RightAnim");
    }
    /// <summary>
    /// 스킨의 뎁스 원상 복귀
    /// </summary>
    public void SkinDepthorder()
    {
        GameObject ItemWindow = GameObject.Find("UI Root/Skin/ItemWindow").gameObject;

        for (int i = 0; i < 7; i++)
        {
            GameObject Skin = ItemWindow.transform.Find(string.Format("Skin{0}", i)).gameObject;
            Skin.GetComponent<UISprite>().depth = 6 - (Mathf.Abs(3 - i));
            Debug.Log(Skin.transform.GetChild(0).GetComponent<Texture>());
            Skin.transform.GetChild(0).GetComponent<UITexture>().depth = Skin.GetComponent<UISprite>().depth;
            Debug.Log(string.Format("SkinBGI:{0},Skin:{1},i:{2}", Skin.GetComponent<UISprite>().depth, Skin.GetComponentInChildren<UISprite>().depth, i));
        }

    }
    /// <summary>
    /// 스킨의 뎁스 좌클릭시 잠시의 조정
    /// </summary>
    public void SkinDepthLeftAnim()
    {
        GameObject ItemWindow = GameObject.Find("UI Root/Skin/ItemWindow").gameObject;
        for (int i = 0; i < 7; i++)
        {
            GameObject Skin = ItemWindow.transform.Find(string.Format("Skin{0}", i)).gameObject;
            Skin.GetComponent<UISprite>().depth = 6 - (Mathf.Abs(2 - i));
            Skin.GetComponentInChildren<UITexture>().depth = Skin.GetComponent<UISprite>().depth;
            Debug.Log(string.Format("SkinBGI:{0},Skin:{1},i:{2}", Skin.GetComponent<UISprite>().depth, Skin.GetComponentInChildren<UISprite>().depth, i));
        }
    }
    /// <summary>
    /// 스킨의 뎁스 우클릭시 잠시의 조정
    /// </summary>
    public void SkinDepthRightAnim()
    {
        GameObject ItemWindow = GameObject.Find("UI Root/Skin/ItemWindow").gameObject;

        for (int i = 0; i < 7; i++)
        {
            GameObject Skin = ItemWindow.transform.Find(string.Format("Skin{0}", i)).gameObject;
            Skin.GetComponent<UISprite>().depth = 6 - (Mathf.Abs(4 - i));
            Skin.GetComponentInChildren<UITexture>().depth = Skin.GetComponent<UISprite>().depth;
            Debug.Log(string.Format("SkinBGI:{0},Skin:{1},i:{2}", Skin.GetComponent<UISprite>().depth, Skin.GetComponentInChildren<UISprite>().depth, i));
        }
    }
    public void FilterSword()
    {
    }
    public void FilterWand()
    {
    }
    public void FilterAmor()
    {
       
    }
    IEnumerator worstReset() //코루틴으로 15초 간격으로 최저 프레임 리셋해줌.
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            worstFps = 100f;
        }
    }

    float deltaTime = 0.0f;

    GUIStyle style;
    Rect rect;
    float msec;
    float fps;
    float worstFps = 100f;
    string text;

    void OnGUI()//소스로 GUI 표시.
    {

        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;  //초당 프레임 - 1초에

        if (fps < worstFps)  //새로운 최저 fps가 나왔다면 worstFps 바꿔줌.
            worstFps = fps;
        text = msec.ToString("F1") + "ms (" + fps.ToString("F1") + ") //worst : " + worstFps.ToString("F1");
        testlabel.text = text;
    }

    public void PlayClickAnim()
    {
        if (UIButton.current.GetComponent<Animator>() != null)
        {
            Debug.Log(UIButton.current.GetComponent<Animator>().GetFloat("Speed"));
            if (UIButton.current.GetComponent<Animator>().GetFloat("Speed").Equals(1))
            {
                UIButton.current.GetComponent<Animator>().SetFloat("Speed", -1);
                UIButton.current.GetComponent<Animator>().Play("ClickAnim");
            }
            else
            {
                UIButton.current.GetComponent<Animator>().SetFloat("Speed", 1);
                UIButton.current.GetComponent<Animator>().Play("ClickAnim");
            }
        }
        else
        {
            if (UIButton.current.transform.parent.GetComponent<Animator>().GetFloat("Speed").Equals(1))
            {
                UIButton.current.transform.parent.GetComponent<Animator>().SetFloat("Speed", -1);
                UIButton.current.transform.parent.GetComponent<Animator>().Play("ClickAnim");
            }
            else
            {
                UIButton.current.transform.parent.GetComponent<Animator>().SetFloat("Speed", 1);
                UIButton.current.transform.parent.GetComponent<Animator>().Play("ClickAnim");
            }
        }
    }

}