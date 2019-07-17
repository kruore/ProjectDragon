using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager inst;
    bool isnight;
    public GameObject[] Equipobject= new GameObject[2];
    GameObject particle;
    public GameObject ObjectPool,Fireobject;
    public GameObject Filterobject, Arrangementobject;
    public AudioClip fire;
    public UISpriteAnimation anim;
    private void Awake()
    {
        inst = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(isnight)
        {
            SoundManager.Inst.Ds_BgmPlayer(fire);
            Fireobject.SetActive(true);
        }
        else
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        //터치시 파티클생성
        if (Input.GetMouseButtonDown(0))
        {
            if(particle!=null)
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
}
