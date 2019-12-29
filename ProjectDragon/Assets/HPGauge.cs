using UnityEngine;

public class HPGauge : MonoBehaviour
{
    //Player Find, and HP Changed
    public Player Current_player;
    public float HP;
    public int record_HPBar;

    //Use this UISprite from How to visually looking by HPGauge. 
    public UISprite First_Block_HPGauge;
    public UISprite Second_Block_HPGauge;
    public UISprite Third_Block_HPGauge;
    public UILabel record_HP;

    public Vector3 Original_Position = new Vector3(-88.1f,39f,0f);
    public Vector3 Minimal_Transform_Position = new Vector3(151.5f, 39f,0f);

    //if Vector3 equal Maximum_Transform_Position then changed sprite in this place.
    public string MaxPos_sprite_Name = "HP_04";
    //if Vector3 equal Minimal_Transform_Position then changed sprite goint to this place.
    public string MinPos_sprite_Name = "HP_03";

   
    // Start is called before the first frame update
    void Start()
    {

        Current_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        First_Block_HPGauge = GameObject.Find("UI Root/HPBar/HP_01").GetComponent<UISprite>();
        Second_Block_HPGauge = GameObject.Find("UI Root/HPBar/HP_02").GetComponent<UISprite>();
        Third_Block_HPGauge = GameObject.Find("UI Root/HPBar/HP_03").GetComponent<UISprite>();
        Third_Block_HPGauge.transform.localPosition = Minimal_Transform_Position;
        record_HPBar = Current_player.HP;
        record_HP.text = HP.ToString();
    }

    public void Player_HP_Changed(float playerHP,float playerMaxHP)
    {
       HP = playerHP / playerMaxHP;
       Second_Block_HPGauge.fillAmount =HP;
       if(Second_Block_HPGauge.fillAmount==1)
        {
            Third_Block_HPGauge.spriteName = MaxPos_sprite_Name;
        }
        else
        {
            Third_Block_HPGauge.spriteName = MinPos_sprite_Name;
        }
        Third_Block_HPGauge.transform.localPosition = new Vector3( Normalized_Two_Position(Original_Position, Minimal_Transform_Position),Third_Block_HPGauge.transform.localPosition.y,Third_Block_HPGauge.transform.localPosition.z);
        record_HP.text = record_HPBar.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    float Normalized_Two_Position(Vector3 a, Vector3 b)
    {
        record_HPBar = Current_player.HP;
        Debug.Log("HP : "+ HP);
        Vector3 c = Vector3.Lerp(a,b,HP);
        return c.x;
    }
}
