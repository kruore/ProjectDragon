using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RARITY
{
    Normal,
    Rare,
    Unique,
    Legend
}

public enum Item_CLASS
{
    Sword,
    Bow,
    Wand,
    Armor, 
    item
}

public enum Monster_Rarity
{
    Common,
    Named,
    Boss
}

public enum Monster_Region
{
    None,
    Jungle,
    DeepSea,
    HighMountain,
    Prison,
    Castle
}

public enum Monster_Category
{
    None,
    Fly,
    Move,
    Hold,
    HoldandMove
}

public enum Monster_Size
{
    Small,
    Medium,
    Large
}

public enum SEX
{
    None,
    Male,
    Female
}
//TODO: 먼저 얻은 순서용 인트 넣기, 이미지 이름, 아이템 설명

public class Database : MonoSingleton<Database>
{

    //클래스 모음
    #region Data_Class

    [System.Serializable]
    public class Inventory
    { //0,1,2
        public int num; //인벤토리에서의 Index
        public int DB_Num; // 해당 아이템이 있는 DB에서의 Index
        public string name; // 아이템 이름
        public float stat; //공격력, 방어력
        public bool isLock; // 아이템 잠금
        public int itemValue; // 아이템 가치 - 강화젬의 강화 수치 같은 것들
        public RARITY rarity; // 희귀도
        public Item_CLASS item_Class; // 아이템 타입 ;; 소드냐 젬이냐 방어구냐 이런거
        public int upgrade_Level;//아이템 레벨
        public int upgrade_Count;//강화 진행중 정도 - 아이템 경험치
        public string imageName; //이미지 이름
        public int amount; // 갯수
        public int skill_Index; // 아이템이 가진 액티브 스킬의 DB에서의 Index

        public Inventory(int _num, int _DB_Num, string _name, float _stat, bool _isLock, int _itemValue, RARITY _rarity, Item_CLASS _item_Class,
                           int _upgrade_Level, int _upgrade_Count, string _imageName, int _amount, int _skill_Index)
        {
            num = _num;
            DB_Num = _DB_Num;
            name = _name;
            stat = _stat;
            isLock = _isLock;
            itemValue = _itemValue;
            rarity = _rarity;
            item_Class = _item_Class;
            upgrade_Level = _upgrade_Level;
            upgrade_Count = _upgrade_Count;
            imageName = _imageName;
            amount = _amount;
            skill_Index = _skill_Index;
        }
    }

    [System.Serializable]
    public class Weapon
    {
        public int num;
        public string name;
        public float damage;
        public int attack_Count; // 공격 횟수
        public float attack_Range; // 사정 거리
        public string attack_Type; //근거린지 원거린지 범윈지 등
        public float attack_Speed; // 공속
        //public int upgrade_Level;
        //public int upgrade_Gauge;
        public int item_Value;
        public string description;
        public int skill_Index; // 
        public RARITY rarity;
        public Item_CLASS item_Class;
        public string imageName; //이미지 이름


        public Weapon(int _num, string _name, float _damage, int _attack_Count, float _attack_Range, string _attack_Type,
                        float _attack_Speed, int _item_Value, /*int _upgrade_Level, int _upgrade_Gauge,*/ string _description, int _skill_Index, RARITY _rarity, Item_CLASS _item_Class, string _imageName)
        {
            num = _num;
            name = _name;
            damage = _damage;
            attack_Count = _attack_Count;
            attack_Range = _attack_Range;
            attack_Type = _attack_Type;
            attack_Speed = _attack_Speed;
            item_Value = _item_Value;
            //upgrade_Level = _upgrade_Level;
            //upgrade_Gauge = _upgrade_Gauge;
            description = _description;
            skill_Index = _skill_Index;
            rarity = _rarity;
            item_Class = _item_Class;
            imageName = _imageName;
        }
    }

    [System.Serializable]
    public class Armor
    {
        public int num;
        public string name;
        public float hp;
        //public int upgrade_Level; //장비레벨
        //public int upgrade_Gauge;//업그레이드 량
        public int item_Value;
        public string description;
        public RARITY rarity;
        public Item_CLASS item_Class;
        public string imageName;

        public Armor(int _num, string _name, float _hp, int _item_Value,/* int _upgrade_Level, int _upgrade_Gauge,*/ string _description, RARITY _rarity, Item_CLASS _item_Class, string _imageName)
        {
            num = _num;
            name = _name;
            hp = _hp;
            item_Value = _item_Value;
            //upgrade_Level = _upgrade_Level;
            //upgrade_Gauge = _upgrade_Gauge;
            description = _description;
            rarity = _rarity;
            item_Class = _item_Class;
            imageName = _imageName;
        }
    }

    #region 이제 곧 죽을 것
    //[System.Serializable]
    //public class Item
    //{ 
    //    public int num;
    //    public string name;
    //    public int item_Value;// 강화시 게이지 수치 값, 공격력비슷, 아이템 가치
    //    //public int amount; // 몇개 가지고 있을까요
    //    public RARITY rarity;
    //    public Item_CLASS item_CLASS;
    //    public string description;

    //    public Item(int _num, string _name, int _item_Value, /*int _amount,*/ RARITY _rarity, Item_CLASS _item_Class, string _description)
    //    {
    //        num = _num;
    //        name = _name;
    //        item_Value = _item_Value;
    //        //amount = _amount;
    //        rarity = _rarity;
    //        item_CLASS = _item_Class;
    //        description = _description;
    //    }
    //}
    #endregion

    [System.Serializable]
    public class Skill
    {
        public int num;
        public string name;
        public string description;
        public float mpCost;
        public int attack_Count; //공격횟수
        public float active_Time; // 실행 속도
        public float coolDown; // 쿨타임
        //attack_Type에 따라 사거리는 어쩌죠?
        public float attack_Range; //사정거리
        public string attack_Type;
        public float attack_Power; //데미지
        public string imageName;

        public Skill(int _num, string _name, string _description, float _mpCost, int _attack_Count, float _active_Time, float _coolDown, float _attack_Range, string _attack_Type, float _attack_Power, string _imageName)
        {
            num = _num;
            name = _name;
            description = _description;
            mpCost = _mpCost;
            attack_Count = _attack_Count;
            active_Time = _active_Time;
            coolDown = _coolDown;
            attack_Range = _attack_Range;
            attack_Type = _attack_Type;
            attack_Power = _attack_Power;
            imageName = _imageName;
        }
    }

    [System.Serializable]
    public class Passive
    {
        public int num;
        public int world;
        public string name;
        public string description;
        public string imageName;

        //효과 변수
        public float damage;
        public float hp;
        public float attack_Speed;
        public float move_Speed;

        public Passive(int _num, int _world, string _name, string _description, string _imageName,
                        float _damage, float _hp, float _attack_Speed, float _move_Speed)
        {
            num = _num;
            world = _world;
            name = _name;
            description = _description;
            imageName = _imageName;

            //효과
            damage = _damage;
            hp = _hp;
            attack_Speed = _attack_Speed;
            move_Speed = _move_Speed;
        }
    }

    [System.Serializable]
    public class Monster
    {
        public int num;
        public Monster_Region region; // 몬스터 출현 지역.. 맵마다 몬스터 나오는 것은 맵 정보에 넣기
        public string name;
        public float damage;
        public float hp;
        public Monster_Rarity monster_Rarity;
        public Monster_Size size;
        public float attack_Range;
        public string attack_Type; //원거린지 근거린지 설명용
        public float attack_Speed;
        public float chase_Range; // 인식 거리
        public float move_Speed; // 이동 속도
        public Monster_Category category;
        public string description;
        public string imageName;
        public string dropItem; // 몬스터가 드랍하는 아이템 월드로 묶을지 함 생각해봐야함 아직 추가 안했음

        public Monster(int _num, Monster_Region _region, string _name, float _damage, float _hp, Monster_Rarity _monster_Rarity, Monster_Size _size,
                         float _attack_Range, string _attack_Type, float _attack_Speed, float _chase_Range, float _move_Speed, Monster_Category _category, string _description, string _imageName)
        {
            num = _num;
            region = _region;
            name = _name;
            damage = _damage;
            hp = _hp;
            monster_Rarity = _monster_Rarity;
            size = _size;
            attack_Range = _attack_Range;
            attack_Type = _attack_Type;
            attack_Speed = _attack_Speed;
            chase_Range = _chase_Range;
            move_Speed = _move_Speed;
            category = _category;
            description = _description;
            imageName = _imageName;
        }
    }

    //플레이 데이터 집합소
    [System.Serializable]
    public class PlayData
    {
        public int itemCount = 0;
        public List<Inventory> inventory = new List<Inventory>();
        public List<Passive> passive = new List<Passive>();
        public float currentHp;
        public int clearStage;
        public int mp; //money power, 돈의 힘
        public SEX sex;

        //장착중인 장비 데이터 따로 추가
        public int equiWeapon_InventoryNum;
        public int equiArmor_InventoryNum;
    }

    #endregion


    //변수 모음
    #region Data_Variable

    //Tables - Just Read

    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armors = new List<Armor>();
    //public List<Item> items = new List<Item>();
    public List<Monster> monsters = new List<Monster>();
    public List<Skill> skill = new List<Skill>();
    public List<Passive> passive = new List<Passive>();

    //Player Game Data Instace
    public PlayData playData = new PlayData();

    #endregion
}
