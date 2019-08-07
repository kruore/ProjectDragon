using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum LobbyState { }
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
    public GameObject CurrentWeapone, CurrentArmor, CurrentActive,equipCharactor;
    #endregion
    private void Awake()
    {
        inst = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        Database.Inst.playData.equiWeapon_InventoryNum = 0;
        Database.Inst.playData.equiArmor_InventoryNum = 5;
        if (isnight)
        {
            SoundManager.Inst.Ds_BgmPlayer(fire);
            fireobject.SetActive(true);
        }
        else
        {
        }
        string classname = "null";
        Debug.Log(Database.Inst.playData.inventory.Count);
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        if (item.item_Class.Equals(Item_CLASS.검))
        {
            classname = "Worrior";
            if(Database.Inst.playData.sex.Equals(SEX.Male))
            {
                playerimg.transform.Find("Weapone").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapone").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                playerimg.transform.Find("Weapone").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapone").transform.localPosition = new Vector3(1, -0.7f, 0);
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
        Debug.Log(playerclass);

        playerimg.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        equipCharactor.GetComponent<UITexture>().mainTexture= Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        CurrentWeapone.transform.Find("SwordIcon").GetComponent<UISprite>().spriteName=item.imageName;
        CurrentWeapone.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = item.stat.ToString();
        Debug.Log(playerclass + "_Weapone");
        playerimg.transform.Find("Weapone").GetComponent<UISprite>().atlas = Resources.Load(playerclass+"_Weapone", typeof(NGUIAtlas)) as NGUIAtlas;
        playerimg.transform.Find("Weapone").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].imageName;
        equipCharactor.transform.Find("Weapone").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapone", typeof(NGUIAtlas)) as NGUIAtlas;
        equipCharactor.transform.Find("Weapone").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].imageName;
        
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
        ButtonManager.TouchBackButton();
    }
    public void ObjectControl()
    {
        ButtonManager.ObjectlistControl();
    }
    public void ChangeEquip()
    {
        //Database.Inst.playData.
    }
}
