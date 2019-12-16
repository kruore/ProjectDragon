
// ==============================================================
// Structure of Database
// 
//  AUTHOR: Kim Dong Ha
// UPDATED: 2019-12-16
// ==============================================================


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

public enum CLASS
{
    검,
    활,
    지팡이,
    갑옷
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

public enum EMBLEM_STATUS
{
    Lock,
    Unlock,
    acheive,
    Activate,
    Equip
}

//TODO: 먼저 얻은 순서용 인트 넣기, 이미지 이름, 아이템 설명

public class Database : MonoSingleton<Database>
{

    //클래스 모음
    #region Data_Class

    [System.Serializable]
    public class Weapon
    {
        public readonly int num;
        public readonly string name;
        public readonly RARITY rarity;
        public readonly CLASS Class;
        public readonly float damage;
        public readonly int attack_Count; // 공격 횟수
        public readonly float attack_Range; // 사정 거리
        public readonly float attack_Speed; // 공속
        public readonly float nuckback; // 공속
        public readonly int item_Value;
        public readonly string description;
        public readonly string imageName; //이미지 이름
        public readonly int skill_Index; // 
        public readonly string optionTableName;

        public Weapon(int _num, string _name, RARITY _rarity, CLASS _Class, float _damage, int _attack_Count, float _attack_Range,
                        float _attack_Speed, float _nuckback, int _item_Value, string _description, string _imageName, int _skill_Index, string _optionTableName)
        {
            num = _num;
            name = _name;
            rarity = _rarity;
            Class = _Class;
            damage = _damage;
            attack_Count = _attack_Count;
            attack_Range = _attack_Range;
            attack_Speed = _attack_Speed;
            nuckback = _nuckback;
            item_Value = _item_Value;
            description = _description;
            imageName = _imageName;
            skill_Index = _skill_Index;
            optionTableName = _optionTableName;
        }
    }

    [System.Serializable]
    public class Armor
    {
        public readonly int num;
        public readonly string name;
        public readonly RARITY rarity;
        public readonly CLASS Class;
        public readonly float hp;
        public readonly int item_Value;
        public readonly string description;
        public readonly string imageName;
        public readonly string optionTableName;

        public Armor(int _num, string _name, RARITY _rarity, CLASS _Class, float _hp, int _item_Value, string _description, string _imageName, string _optionTableName)
        {
            num = _num;
            name = _name;
            rarity = _rarity;
            Class = _Class;
            hp = _hp;
            item_Value = _item_Value;
            description = _description;
            imageName = _imageName;
            optionTableName = _optionTableName;
        }
    }

    [System.Serializable]
    public class Skill
    {
        public readonly int num;
        public readonly string name;
        public readonly float mpCost;
        public readonly int attack_Count; //공격횟수
        public readonly float active_Time; // 실행 속도
        public readonly float coolTime; // 쿨타임
        //attack_Type에 따라 사거리는 어쩌죠?
        public readonly float attack_Range; //사정거리
        public readonly float attack_Power; //데미지
        public readonly string description;
        public readonly string imageName;

        public Skill(int _num, string _name, float _mpCost, int _attack_Count, float _active_Time, float _coolTime, float _attack_Range, float _attack_Power, string _description, string _imageName)
        {
            num = _num;
            name = _name;
            mpCost = _mpCost;
            attack_Count = _attack_Count;
            active_Time = _active_Time;
            coolTime = _coolTime;
            attack_Range = _attack_Range;
            attack_Power = _attack_Power;
            description = _description;
            imageName = _imageName;
        }
    }

    [System.Serializable]
    public class Emblem
    {
        public readonly int num;
        public readonly string name;
        public EMBLEM_STATUS status;
        public readonly string description;
        public readonly string imageName;
        public readonly string methodName;

        public Emblem(int _num, string _name, EMBLEM_STATUS _status, string _description, string _imageName, string _methodName)
        {
            num = _num;
            name = _name;
            status = _status;
            description = _description;
            imageName = _imageName;
            methodName = _methodName;
        }

        public Emblem(Database.Emblem src)
        {
            num = src.num;
            name = src.name;
            status = src.status;
            description = src.description;
            imageName = src.imageName;
            methodName = src.methodName;
        }
    }

    [System.Serializable]
    public class Monster
    {
        public readonly int num;
        public readonly string name;
        public readonly Monster_Rarity monster_Rarity;
        public readonly float damage;
        public readonly float hp;
        public readonly float attack_Range;
        public readonly float move_Speed; // 이동 속도
        public readonly string description;
        public readonly string imageName;

        public Monster(int _num, string _name, Monster_Rarity _monster_Rarity, float _damage, float _hp,
                         float _attack_Range, float _move_Speed,  string _description, string _imageName)
        {
            num = _num;
            name = _name;
            monster_Rarity = _monster_Rarity;
            damage = _damage;
            hp = _hp;
            attack_Range = _attack_Range;
            move_Speed = _move_Speed;
            description = _description;
            imageName = _imageName;
        }
    }

    [System.Serializable]
    public class OptionTable
    {
        public readonly int num;
        public readonly float parameter;
        public readonly string description;
        public readonly string methodName;

        public OptionTable(int _num, float _parameter, string _description, string _methodName)
        {
            num = _num;
            parameter = _parameter;
            description = _description;
            methodName = _methodName;
        }
    }

    [System.Serializable]
    public class Inventory
    { //0,1,2
        public int num; //인벤토리에서의 Index
        public int DB_Num; // 해당 아이템이 있는 DB에서의 Index
        public string name; // 아이템 이름
        public RARITY rarity; // 희귀도
        public CLASS Class; // 아이템 타입 ;; 소드냐 젬이냐 방어구냐 이런거
        public int itemValue; // 아이템 가치 - 강화젬의 강화 수치 같은 것들
        public string imageName; //이미지 이름
        public int skill_Index; // 아이템이 가진 액티브 스킬의 DB에서의 Index
        public int option_Index;

        public Inventory(int _num, int _DB_Num, string _name, RARITY _rarity, CLASS _Class, int _itemValue,
                           string _imageName, int _skill_Index, int _option_Index)
        {
            num = _num;
            DB_Num = _DB_Num;
            name = _name;
            rarity = _rarity;
            Class = _Class;
            itemValue = _itemValue;
            imageName = _imageName;
            skill_Index = _skill_Index;
            option_Index = _option_Index;
        }

        public Inventory(Database.Weapon weapon)
        {
            num = Database.Inst.GetInventoryCount();
            DB_Num = weapon.num;
            name = weapon.name;
            rarity = weapon.rarity;
            Class = weapon.Class;
            itemValue = weapon.item_Value;
            imageName = weapon.imageName;
            skill_Index = weapon.skill_Index;
            option_Index = 0;
        }

        public Inventory(Database.Armor armor)
        {
            num = Database.Inst.GetInventoryCount();
            DB_Num = armor.num;
            name = armor.name;
            rarity = armor.rarity;
            Class = armor.Class;
            itemValue = armor.item_Value;
            imageName = armor.imageName;
            skill_Index = -1;
            option_Index = 0;
        }
    }

    //플레이 데이터 집합소
    [System.Serializable]
    public class PlayData
    {
        public List<Inventory> inventory = new List<Inventory>();
        public List<Emblem> emblem = new List<Emblem>();
        public float currentHp;
        public readonly float baseHp = 20.0f;
        public float hp;
        public float damage;
        public float moveSpeed;
        public float attackSpeed;
        public float attackRange;
        public float nuckBack;
        public int currentStage;
        public int mp; //money power, 돈의 힘
        public SEX sex;

        //장비 강화 패시브에 의해 변경되는 값들
        public bool resist_Fire;
        public bool resist_Water;
        public bool resist_Poison;
        public bool resist_Electric;
        public bool attackType_Fire;
        public bool attackType_Water;
        public bool attackType_Poison;
        public bool attackType_Electric;

        public float damage_Reduction;

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
    public List<Monster> monsters = new List<Monster>();
    public List<Skill> skill = new List<Skill>();

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
