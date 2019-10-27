using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RARITY
{
    노말,
    레어,
    유니크,
    레전드
}

public enum Item_CLASS
{
    검,
    활,
    지팡이,
    갑옷, 
    아이템
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
    HoldandFly
}

public enum Monster_Size
{
    S,
    M,
    L
}

public enum SEX
{
    None,
    Male,
    Female
}

public enum PASSIVE_TYPE
{
    None,
    AP,
    RD,
    HP,
    AM,
    Luck,
    AD,
    CT,
    AS,
    DR,
    AH,
    EM
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

        public Inventory(Database.Weapon weapon)
        {
            num = Database.Inst.GetInventoryCount();
            DB_Num = weapon.num;
            name = weapon.name;
            stat = weapon.damage;
            isLock = false;
            itemValue = weapon.item_Value;
            rarity = weapon.rarity;
            item_Class = weapon.item_Class;
            upgrade_Level = 1;
            upgrade_Count = 0;
            imageName = weapon.imageName;
            amount = 1;
            skill_Index = weapon.skill_Index;
        }

        public Inventory(Database.Armor armor)
        {
            num = Database.Inst.GetInventoryCount();
            DB_Num = armor.num;
            name = armor.name;
            stat = armor.hp;
            isLock = false;
            itemValue = armor.item_Value;
            rarity = armor.rarity;
            item_Class = armor.item_Class;
            upgrade_Level = 1;
            upgrade_Count = 0;
            imageName = armor.imageName;
            amount = 1;
            skill_Index = -1;
        }
    }

    [System.Serializable]
    public class Weapon
    {
        public readonly int num;
        public readonly string name;
        public readonly float damage;
        public readonly int attack_Count; // 공격 횟수
        public readonly float attack_Range; // 사정 거리
        public readonly string attack_Type; //근거린지 원거린지 범윈지 등
        public readonly float attack_Speed; // 공속
        public readonly int item_Value;
        public readonly string description;
        public readonly int skill_Index; // 
        public readonly RARITY rarity;
        public readonly Item_CLASS item_Class;
        public readonly string imageName; //이미지 이름


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
        public readonly int num;
        public readonly string name;
        public readonly float hp;
        public readonly int item_Value;
        public readonly string description;
        public readonly RARITY rarity;
        public readonly Item_CLASS item_Class;
        public readonly string imageName;

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

    [System.Serializable]
    public class Skill
    {
        public readonly int num;
        public readonly string name;
        public readonly string description;
        public readonly float mpCost;
        public readonly int attack_Count; //공격횟수
        public readonly float active_Time; // 실행 속도
        public readonly float coolDown; // 쿨타임
        //attack_Type에 따라 사거리는 어쩌죠?
        public readonly float attack_Range; //사정거리
        public readonly string attack_Type;
        public readonly float attack_Power; //데미지
        public readonly string imageName;

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
        public readonly int num;
        public readonly int world;
        public readonly string name;
        public readonly string description;
        public readonly string imageName;

        //효과 변수
        public readonly PASSIVE_TYPE passiveType;
        public readonly int[] statPerLV;

        public Passive(int _num, int _world, string _name, string _description, string _imageName,
                        PASSIVE_TYPE _passiveType, int[] _statPerLV)
        {
            num = _num;
            world = _world;
            name = _name;
            description = _description;
            imageName = _imageName;

            //효과
            passiveType = _passiveType;
            statPerLV = _statPerLV;
        }

        public Passive(Database.Passive src)
        {
            num = src.num;
            world = src.world;
            name = src.name;
            description = src.description;
            imageName = src.imageName;

            //효과
            passiveType = src.passiveType;
            statPerLV = src.statPerLV;
        }
    }

    [System.Serializable]
    public class Monster
    {
        public readonly int num;
        public readonly Monster_Region region; // 몬스터 출현 지역.. 맵마다 몬스터 나오는 것은 맵 정보에 넣기
        public readonly string name;
        public readonly float damage;
        public readonly float hp;
        public readonly Monster_Rarity monster_Rarity;
        public readonly Monster_Size size;
        public readonly float attack_Range;
        public readonly string attack_Type; //원거린지 근거린지 설명용
        public readonly float attack_Speed;
        public readonly float chase_Range; // 인식 거리
        public readonly float move_Speed; // 이동 속도
        public readonly Monster_Category category;
        public readonly string description;
        public readonly string imageName;
        public readonly string dropItem; // 몬스터가 드랍하는 아이템 월드로 묶을지 함 생각해봐야함 아직 추가 안했음

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
        public List<Inventory> inventory = new List<Inventory>();
        public List<Passive> passive = new List<Passive>();
        public float currentHp;
        public float damage;
        public float moveSpeed;
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
    public int[,] statPerLevel = new int[16, 9];

    //Player Game Data Instace
    public PlayData playData = new PlayData();

    #endregion

    #region Function
    //인벤토리에 현재 몇개의 아이템을 가지고 있는지 반환합니다.
    public int GetInventoryCount()
    {
        return playData.inventory.Count;
    }
    #endregion
}
