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
    Jem
}

public enum Monster_Rarity
{
    Common,
    Named,
    Boss
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
    {
        public int num;
        public int DB_Num;
        public string name;
        public int itemValue; // 아이템 가치 - 강화젬의 강화 수치 같은 것들
        public RARITY rarity; // 희귀도
        public Item_CLASS item_Class; // 아이템 타입
        public int upgrade_Level;//아이템 레벨
        public int upgrade_Count;//강화 진행중 정도 - 아이템 경험치
        public string imageName; //이미지 이름
        public int amount; // 갯수
        public bool isEquipment; // 장착중인가?
        public Inventory(int _num, int _DB_Num, string _name, int _itemValue, RARITY _rarity, Item_CLASS _item_Class,
                           int _upgrade_Level, int _upgrade_Count, string _imageName, int _amount, bool _isEquipment)
        {
            num = _num;
            DB_Num = _DB_Num;
            name = _name;
            itemValue = _itemValue;
            rarity = _rarity;
            item_Class = _item_Class;
            upgrade_Level = _upgrade_Level;
            upgrade_Count = _upgrade_Count;
            imageName = _imageName;
            amount = _amount;
            isEquipment = _isEquipment;
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
        public float chase_Range; // 인식 범위? 사정거리... 흠 모르겠군
        //public int upgrade_Level;
        //public int upgrade_Gauge;
        public string description;
        public string skill;
        public RARITY rarity;
        public Item_CLASS item_Class;


        public Weapon(int _num, string _name, float _damage, int _attack_Count, float _attack_Range, string _attack_Type,
                        float _attack_Speed, float _chase_Range, /*int _upgrade_Level, int _upgrade_Gauge,*/ string _description, string _skill, RARITY _rarity, Item_CLASS _item_Class)
        {
            num = _num;
            name = _name;
            damage = _damage;
            attack_Count = _attack_Count;
            attack_Range = _attack_Range;
            attack_Type = _attack_Type;
            attack_Speed = _attack_Speed;
            chase_Range = _chase_Range;
            //upgrade_Level = _upgrade_Level;
            //upgrade_Gauge = _upgrade_Gauge;
            description = _description;
            skill = _skill;
            rarity = _rarity;
            item_Class = _item_Class;
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
        public string description;
        public RARITY rarity;
        public Item_CLASS item_Class;

        public Armor(int _num, string _name, float _hp,/* int _upgrade_Level, int _upgrade_Gauge,*/ string _description, RARITY _rarity, Item_CLASS _item_Class)
        {
            num = _num;
            name = _name;
            hp = _hp;
            //upgrade_Level = _upgrade_Level;
            //upgrade_Gauge = _upgrade_Gauge;
            description = _description;
            rarity = _rarity;
            item_Class = _item_Class;
        }
    }

    [System.Serializable]
    public class Item
    {
        public int num;
        public string name;
        public int item_Value;// 강화시 게이지 수치 값, 공격력비슷
        //public int amount; // 몇개 가지고 있을까요
        public RARITY rarity;
        public Item_CLASS item_CLASS;
        public string description;

        public Item(int _num, string _name, int _item_Value, /*int _amount,*/ RARITY _rarity, Item_CLASS _item_Class, string _description)
        {
            num = _num;
            name = _name;
            item_Value = _item_Value;
            //amount = _amount;
            rarity = _rarity;
            item_CLASS = _item_Class;
            description = _description;
        }
    }

    [System.Serializable]
    public class Skill
    {
        public int num;
        public string name;
        public string description;
        public int attack_Count; //공격횟수
        public float active_Time; // 실행 속도
        public float coolDown; // 쿨타임
        //attack_Type에 따라 사거리는 어쩌죠?
        public float attack_Range; //사정거리
        public string attack_Type;
        public float attack_Power; //데미지

        public Skill(int _num, string _name, string _description, int _attack_Count, float _active_Time, float _coolDown, float _attack_Range, string _attack_Type, float _attack_Power)
        {
            num = _num;
            name = _name;
            description = _description;
            attack_Count = _attack_Count;
            active_Time = _active_Time;
            coolDown = _coolDown;
            attack_Range = _attack_Range;
            attack_Type = _attack_Type;
            attack_Power = _attack_Power;
        }
    }

    [System.Serializable]
    public class Passive
    {
        public int num;
        public int world;
        public string name;
        public string description;

        //효과 변수
        public float damage;
        public float hp;
        public float attack_Speed;
        public float move_Speed;

        public Passive(int _num, int _world, string _name, string _description,
                        float _damage, float _hp, float _attack_Speed, float _move_Speed)
        {
            num = _num;
            world = _world;
            name = _name;
            description = _description;

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
        public int location; // 1-1 스테이지면 101이런식의 데이터가 들어갈 것
        public string name;
        public float damage;
        public float hp;
        public Monster_Rarity monster_Rarity;
        public float attack_Range;
        public string attack_Type; //원거린지 근거린지 설명용
        public float attack_Speed;
        public float chase_Range; // 인식 거리
        public float move_Speed; // 이동 속도
        public bool isPossibleMove; //0은 false로, 1은 true로 치환
        public string description;

        public Monster(int _num, int _location, string _name, float _damage, float _hp, Monster_Rarity _monster_Rarity,
                         float _attack_Range, string _attack_Type, float _attack_Speed, float _chase_Range, float _move_Speed, bool _isPossibleMove, string _description)
        {
            num = _num;
            location = _location;
            name = _name;
            damage = _damage;
            hp = _hp;
            monster_Rarity = _monster_Rarity;
            attack_Range = _attack_Range;
            attack_Type = _attack_Type;
            attack_Speed = _attack_Speed;
            chase_Range = _chase_Range;
            move_Speed = _move_Speed;
            isPossibleMove = _isPossibleMove;
            description = _description;
        }
    }

    //플레이 데이터 집합소
    [System.Serializable]
    public class PlayData
    {
        public List<Inventory> inventory = new List<Inventory>();
        public List<Passive> passive = new List<Passive>();
        public float currentHp;
        public int clearStage;
        public int mp; //money power, 
        public SEX sex;
    }

    #endregion


    //변수 모음
    #region Data_Variable

    //Tables - Just Read

    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armors = new List<Armor>();
    public List<Item> items = new List<Item>();
    public List<Monster> monsters = new List<Monster>();
    public List<Skill> skill = new List<Skill>();
    public List<Passive> passive = new List<Passive>();

    //Player Game Data Instace
    public PlayData playData = new PlayData();

    #endregion
}
