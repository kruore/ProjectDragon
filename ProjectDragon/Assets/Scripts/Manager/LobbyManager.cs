using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (item.item_Class.Equals(Item_CLASS.Sword))
        {
            classname = "Worrior";
        }
        else if (item.item_Class.Equals(Item_CLASS.Bow))
        {
            classname = "Archer";
        }
        else if (item.item_Class.Equals(Item_CLASS.Wand))
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
    public void ObjectControl()
    {
        ButtonManager.ObjectlistControl();
    }
}
