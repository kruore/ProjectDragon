
// ==============================================================
// DataTransaction
// Connect between Database and other Classes.
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Data;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DataTransaction : MonoSingleton<DataTransaction>
{
    private Database database;
    public IDbCommand DEB_dbcmd;

    private void Awake()
    {
        StartCoroutine(DataPhasing());
        database = Database.Inst;
        DataBaseConnecting();
        StartCoroutine(LoadAllTableData());
    }

    #region Database Connecting

    //현재 파일 중에 DB파일이 없다면 생성
    IEnumerator DataPhasing()
    {
        string conn;
        if (Application.platform.Equals(RuntimePlatform.Android))
        {
            conn = Application.persistentDataPath + "/DS_Database.sqlite";
            if (!File.Exists(conn))
            {
                WWW loadDB = new WWW("jar: file://" + Application.dataPath + "!/assets/" + "DS_Database.sqlite");
                loadDB.bytesDownloaded.ToString();
                while (!loadDB.isDone) { }
                File.WriteAllBytes(conn, loadDB.bytes);
            }
        }
        yield return null;
    }

    //DB에 연결합니다.
    void DataBaseConnecting()
    {
        string conn;
        if (Application.platform == RuntimePlatform.Android)
        {
            conn = "URI=file:" + Application.persistentDataPath + "/DS_Database.sqlite";
        }
        else
        {
            conn = "URI=file:" + Application.dataPath + "/StreamingAssets/DS_Database.sqlite";
        }
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        DEB_dbcmd = dbconn.CreateCommand();
    }

    #endregion

    //모든 테이블의 정보를 로드 합니다.
    IEnumerator LoadAllTableData()
    {
        Load_Weapon_Table();
        Load_Armor_Table();
        Load_ActiveSkill_Table();
        Load_Monster_Table();
        LoadPlayerData();

        yield return null;
        StopCoroutine(LoadAllTableData());
    }

    //플레이어 데이터를 로드합니다.
    public void LoadPlayerData()
    {
        //플레이어 테이블 데이터 로드
        Load_Inventory_Table();
        //플레이어의 패시브 데이터를 로드
        Load_Emblem_PlayData();
        //플레이어 기본 데이터 로드
        Load_PlayerPrefs_Data();
    }

    public void SavePlayerData()
    {
        //플레이어 테이블 데이터 저장
        Save_Inventory_Table();

        //플레이어 기본 데이터 저장
        Save_PlayerPrefs_Data();

        //플레이어의 패시브를 저장
        Save_Emblem_PlayData();
        Debug.Log("Save Player Data Complete");
    }

    //구글 데이터베이스 연결 함수
    #region Google_Connecting_Method

    public void PlayDataSavedOnGoogle()
    {

    }

    public void PlayDataLoadedOnGoogle()
    {

    }

    public void PlayDataResetAndSaveOnGoogle()
    {

    }

    #endregion

    #region 아이템 드랍 - 만들어야 함

    #endregion

    #region 편의성 함수 모음


    //배틀 - 스테이지 끝나고 맵상의 모든 아이템을 인벤토리에 세팅하기 위한 함수 입니다.
    public void EndGame_Get_Item(List<Database.Weapon> _weapon_List, List<Database.Armor> _armor_List, int _mp = 0)
    {
        List<Database.Inventory> inventories = new List<Database.Inventory>();

        inventories = Convert_InventoryList_fromItem(_weapon_List, _armor_List);

        //인벤토리에 아이템 삽입
        Insert_Inventory_Item(inventories);

        Mp += _mp;
    }

    /// <summary>
    /// return weapons and armor with a single inventory.
    /// </summary>
    /// <param name="weapons">weapons table</param>
    /// <param name="armors">armors table</param>
    /// <returns></returns>
    public List<Database.Inventory> Convert_InventoryList_fromItem(List<Database.Weapon> weapons, List<Database.Armor> armors)
    {
        List<Database.Inventory> inventories = new List<Database.Inventory>();

        if (!weapons.Count.Equals(0))
        {
            foreach (Database.Weapon obj in weapons)
            {
                inventories.Add(new Database.Inventory(obj));
            }
        }
        if (!armors.Count.Equals(0))
        {
            foreach (Database.Armor obj in armors)
            {
                inventories.Add(new Database.Inventory(obj));
            }
        }
        return inventories;
    }

    /// <summary>
    /// 무기를 해당 무기의 스킬로 변환하여 반환합니다.
    /// </summary>
    /// <param name="_EquipItem">인벤토리에 있는 장비 아이템</param>
    /// <returns></returns>
    public Database.Skill Convert_ItemtoSkill(Database.Inventory _EquipItem)
    {
        if (_EquipItem.Class.Equals(CLASS.갑옷))
        {
            Debug.LogError("DataTransaction::Convert_ItemtoSkill(), Please give me an item with skill");
            return null;
        }

        return database.skill[_EquipItem.skill_Index];
    }

    /// <summary>
    /// add items to inventory
    /// </summary>
    /// <param name="_inventories">items</param>
    public void Insert_Inventory_Item(List<Database.Inventory> _inventories)
    {
        // 아이템 중복되는 것 있으면 amount 컨트롤 해야함
        database.playData.inventory.AddRange(_inventories);
    }

    #endregion

    /// <summary>
    /// 인벤토리에서 아이템을 삭제합니다.
    /// </summary>
    /// <param name="_item_Inventory_Num"></param>
    public void Delete_Inventory_Item(int _item_Inventory_Num)
    {
        if (database.GetInventoryCount() <= _item_Inventory_Num) return;

        database.playData.inventory.RemoveAt(_item_Inventory_Num);

        for (int i = _item_Inventory_Num; i < database.GetInventoryCount(); i++)
        {
            database.playData.inventory[i].num--;
        }
    }



    #region player data method & property
    //플레이어의 데이터를 연결해주는 property 들

    public Database.PlayData PlayData
    {
        get { return database.playData; }
    }

    public float MaxHp
    {
        get { return database.playData.hp + CurrentEquipArmor.hp; }
    }
    public float BaseHp
    {
        get { return database.playData.baseHp; }
    }
    public float CurrentHp
    {
        get {
            return database.playData.currentHp;
        }
        set
        {
            float temp = value;

            if (temp <= 0) temp = 0.0f;
            else if (MaxHp <= temp) temp = MaxHp;

            database.playData.currentHp = temp;
        }
    }
    public float CurrentDamage
    {
        get { return database.playData.damage; }
    }
    public float MoveSpeed
    {
        get { return database.playData.moveSpeed; }
    }
    public float AttackSpeed
    {
        get { return database.playData.attackSpeed; }
    }
    public float AttackRange
    {
        get { return database.playData.attackRange; }
    }
    public float NuckBack
    {
        get { return database.playData.nuckBack; }
    }
    public int CurrentStage
    {
        get { return database.playData.currentStage; }
        set
        {
            int temp = value;
            if (temp < 0) temp = 0;
            //if(< temp)
            database.playData.currentStage = value;
        }
    }
    public int Mp
    {
        get { return database.playData.mp; }
        set
        {
            int temp = value;
            if (temp < 0) temp = 0;
            else if (9999999 < temp) temp = 9999999;

            database.playData.mp = value;
        }
    }
    public SEX Sex
    {
        get { return database.playData.sex; }
        set
        {
            database.playData.sex = value;
        }
    }
    //플레이어 현재 스킬
    public Database.Skill CurrentSkill
    {
        get { return database.skill[PlayerEquipWeapon.skill_Index]; }
    }
    //현재 장착 무기
    public Database.Weapon CurrentEquipWeapon
    {
        get { return database.weapons[PlayerEquipWeapon.DB_Num]; }
    }
    //현재 장착 방어구
    public Database.Armor CurrentEquipArmor
    {
        get { return database.armors[PlayerEquipArmor.DB_Num]; }
    }
    //무기 장착 해제
    public Database.Inventory PlayerEquipWeapon
    {
        get
        {
            return database.playData.inventory[database.playData.equiWeapon_InventoryNum];
        }
        set
        {
            if (!value.Class.Equals(CLASS.갑옷))
            {
                Database.Weapon weapon = database.weapons[value.DB_Num];
                database.playData.equiWeapon_InventoryNum = value.num;
                database.playData.damage = weapon.damage;
                database.playData.attackSpeed = weapon.attack_Speed;
                database.playData.attackRange = weapon.attack_Range;
                database.playData.nuckBack = weapon.nuckback;

                if (!value.option_Index.Equals(0))
                {
                    Database.OptionTable option = LoadOptionData(weapon.optionTableName, value.option_Index);
                    ApplyOption(option);
                }
            }
        }
    }
    //방어구 장착 해제
    public Database.Inventory PlayerEquipArmor
    {
        get
        {
            return database.playData.inventory[database.playData.equiArmor_InventoryNum];
        }
        set
        {
            if (value.Class.Equals(CLASS.갑옷))
            {
                Database.Armor armor = database.armors[value.DB_Num];
                database.playData.equiArmor_InventoryNum = value.num;

                if (!value.option_Index.Equals(0))
                {
                    Database.OptionTable option = LoadOptionData(armor.optionTableName, value.option_Index);
                    ApplyOption(option);
                }
            }
        }
    }

    /// <summary>
    /// 엠블럼 기능 구조~~
    /// </summary>
    /// <param name="_optionTableName"></param>
    /// <param name="_num"></param>
    /// <returns></returns>
    //옵션 데이터 로드
    private Database.OptionTable LoadOptionData(string _optionTableName, int _num)
    {
        string sqlQuery = "SELECT * FROM " + _optionTableName + " WHERE Num = " + _num;
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        reader.Read();

        int Num = reader.GetInt32(0);
        float Parameter = reader.GetFloat(1);
        string Description = reader.GetString(2);
        string MethodName = reader.GetString(3);

        reader.Close();
        reader = null;
        return new Database.OptionTable(Num, Parameter, Description, MethodName);
    }
    //옵션 적용
    private void ApplyOption(Database.OptionTable option)
    {
        Type type = this.GetType();
        MethodInfo method = type.GetMethod(option.methodName);
        if (method != null)
        {
            method.Invoke(this, new object[] { option.parameter });
        }
    }

    //옵션 함수 모음
    private void IncreaseDamage(float param)
    {
        database.playData.damage += param;
    }
    private void IncreaseHp(float param)
    {
        //최대 체력과 현재 체력 다 올라감
        database.playData.hp += param;
        CurrentHp += param;
    }
    #endregion


    /// <summary>
    /// 플레이어가 죽었거나 포기했을때 부르면 데이터가 초기화 됩니다.
    /// </summary>
    public void InitializePlayData()
    {
        InitialPlayData();
        SavePlayerData();
    }

    #region 공사중

    /// <summary>
    /// initialize player data
    /// </summary>
    private void InitialPlayData()
    {
        ResetInventory();
        ResetEmblem();
        database.playData.equiWeapon_InventoryNum = 0;
        database.playData.equiArmor_InventoryNum = 1;

        database.playData.hp = BaseHp;
        database.playData.moveSpeed = 1.0f;
        database.playData.currentStage = 0;
        database.playData.mp = 1000;
        InitializePlayerStat();

        database.playData.sex = SEX.Female;
        database.playData.resist_Fire = false;
        database.playData.resist_Water = false;
        database.playData.resist_Poison = false;
        database.playData.resist_Electric = false;
        database.playData.attackType_Fire = false;
        database.playData.attackType_Water = false;
        database.playData.attackType_Poison = false;
        database.playData.attackType_Electric = false;
    }

    //아이템에 의한 능력치 조정
    private void InitializePlayerStat()
    {
        Database.Inventory item = database.playData.inventory[0];
        Database.Weapon weapon = database.weapons[item.DB_Num];
        database.playData.damage = weapon.damage;
        database.playData.attackSpeed = weapon.attack_Speed;
        database.playData.attackRange = weapon.attack_Range;
        database.playData.nuckBack = weapon.nuckback;

        database.playData.currentHp = MaxHp;
    }

    //죽었을때 인벤토리 초기화
    private void ResetInventory()
    {
        database.playData.inventory.RemoveRange(0, database.playData.inventory.Count);
        database.playData.inventory.Add(new Database.Inventory(database.weapons[0]));
        database.playData.inventory.Add(new Database.Inventory(database.armors[0]));
    }

    /// <summary>
    /// reset emblem table and player data, if unlocked emblemes are retained
    /// </summary>
    /// 
    private void ResetEmblem()
    {
        List<Database.Emblem> emblem = database.playData.emblem;
        //Insert Data into Table
        for (int i = 0; i < emblem.Count; i++)
        {
            int Status = (int)emblem[i].status;

            if (Status > 1)
            {
                database.playData.emblem[i].status = EMBLEM_STATUS.Unlock;
                Status = 1;
            }
        }
    }
    #endregion

    //테스트 완료
    #region Database_Load_Player_Data
    //플레이어 데이터 로드 함수

    //플레이어 프리팹 로드
    void Load_PlayerPrefs_Data()
    {
        if (PlayerPrefs.HasKey("save1"))
        {
            database.playData.currentHp = PlayerPrefs.GetFloat("currentHp");
            database.playData.hp = PlayerPrefs.GetFloat("hp");
            database.playData.damage = PlayerPrefs.GetFloat("damage");
            database.playData.moveSpeed = PlayerPrefs.GetFloat("moveSpeed");
            database.playData.attackSpeed = PlayerPrefs.GetFloat("attackSpeed");
            database.playData.attackRange = PlayerPrefs.GetFloat("attackRange");
            database.playData.nuckBack = PlayerPrefs.GetFloat("nuckBack");
            database.playData.currentStage = PlayerPrefs.GetInt("currentStage");
            database.playData.mp = PlayerPrefs.GetInt("mp");
            database.playData.sex = (SEX)PlayerPrefs.GetInt("sex");
            database.playData.equiWeapon_InventoryNum = PlayerPrefs.GetInt("equiWeapon_InventoryNum");
            database.playData.equiArmor_InventoryNum = PlayerPrefs.GetInt("equiArmor_InventoryNum");
            database.playData.resist_Fire = PlayerPrefs.GetInt("resist_Fire").Equals(1) ? true : false;
            database.playData.resist_Water = PlayerPrefs.GetInt("resist_Water").Equals(1) ? true : false;
            database.playData.resist_Poison = PlayerPrefs.GetInt("resist_Poison").Equals(1) ? true : false;
            database.playData.resist_Electric = PlayerPrefs.GetInt("resist_Electric").Equals(1) ? true : false;
            database.playData.attackType_Fire = PlayerPrefs.GetInt("attackType_Fire").Equals(1) ? true : false;
            database.playData.attackType_Water = PlayerPrefs.GetInt("attackType_Water").Equals(1) ? true : false;
            database.playData.attackType_Poison = PlayerPrefs.GetInt("attackType_Poison").Equals(1) ? true : false;
            database.playData.attackType_Electric = PlayerPrefs.GetInt("attackType_Electric").Equals(1) ? true : false;
        }
        else
        {
            InitialPlayData();
        }
    }

    //인벤토리 테이블 로드
    void Load_Inventory_Table()
    {
        string sqlQuery = "SELECT * FROM Inventory";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            int DB_Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            RARITY Rarity = (RARITY)(reader.GetInt32(count++));
            CLASS Class = (CLASS)(reader.GetInt32(count++));
            int ItemValue = reader.GetInt32(count++);
            string ImageName = reader.GetString(count++);
            int skill_Index = reader.GetInt32(count++);
            int option_Index = reader.GetInt32(count++);

            database.playData.inventory.Add(new Database.Inventory(Num, DB_Num, Name, Rarity, Class, ItemValue, ImageName, skill_Index, option_Index));
        }
        reader.Close();
        reader = null;
    }

    //엠블럼 테이블 로드
    void Load_Emblem_PlayData()
    {
        string sqlQuery = "SELECT * FROM Emblem";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            EMBLEM_STATUS Status = (EMBLEM_STATUS)reader.GetInt32(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);
            string MethodName = reader.GetString(count++);

            database.playData.emblem.Add(new Database.Emblem(Num, Name, Status, Description, ImageName, MethodName));
        }
        reader.Close();
        reader = null;
    }
    #endregion

    //테스트 완료
    #region Database_Save_Player_Data

    void Save_PlayerPrefs_Data()
    {
        Database.PlayData playData = database.playData;

        PlayerPrefs.SetInt("save", 1);
        PlayerPrefs.SetFloat("currentHp", playData.currentHp);
        PlayerPrefs.SetFloat("hp", playData.hp);
        PlayerPrefs.SetFloat("damage", playData.damage);
        PlayerPrefs.SetFloat("moveSpeed", playData.moveSpeed);
        PlayerPrefs.SetFloat("attackSpeed", playData.attackSpeed);
        PlayerPrefs.SetFloat("attackRange", playData.attackRange);
        PlayerPrefs.SetFloat("nuckBack", playData.nuckBack);
        PlayerPrefs.SetInt("currentStage", playData.currentStage);
        PlayerPrefs.SetInt("mp", playData.mp);
        PlayerPrefs.SetInt("sex", (int)playData.sex);
        PlayerPrefs.SetInt("resist_Fire", playData.resist_Fire ? 1 : 0);
        PlayerPrefs.SetInt("resist_Water", playData.resist_Water ? 1 : 0);
        PlayerPrefs.SetInt("resist_Poison", playData.resist_Poison ? 1 : 0);
        PlayerPrefs.SetInt("resist_Electric", playData.resist_Electric ? 1 : 0);
        PlayerPrefs.SetInt("attackType_Fire", playData.attackType_Fire ? 1 : 0);
        PlayerPrefs.SetInt("attackType_Water", playData.attackType_Water ? 1 : 0);
        PlayerPrefs.SetInt("attackType_Poison", playData.attackType_Poison ? 1 : 0);
        PlayerPrefs.SetInt("attackType_Electric", playData.attackType_Electric ? 1 : 0);
        PlayerPrefs.SetInt("equiWeapon_InventoryNum", playData.equiWeapon_InventoryNum);
        PlayerPrefs.SetInt("equiArmor_InventoryNum", playData.equiArmor_InventoryNum);
        PlayerPrefs.Save();
    }

    void Save_Inventory_Table()
    {
        //Reset Table
        string sqlQuery = "DELETE FROM Inventory";
        DEB_dbcmd.CommandText = sqlQuery;
        DEB_dbcmd.ExecuteNonQuery();

        //Insert Data into Table
        for (int i = 0; i < database.playData.inventory.Count; i++)
        {
            int Num = database.playData.inventory[i].num;
            int DB_Num = database.playData.inventory[i].DB_Num;
            string Name = database.playData.inventory[i].name;
            int Rarity = (int)database.playData.inventory[i].rarity;
            int Class = (int)database.playData.inventory[i].Class;
            int ItemValue = database.playData.inventory[i].itemValue;
            string ImageName = database.playData.inventory[i].imageName;
            int Skill_Index = database.playData.inventory[i].skill_Index;
            int OptionIndex = database.playData.inventory[i].option_Index;

            sqlQuery = "INSERT INTO Inventory(Num, DB_Num, Name, Rarity, Class, ItemValue, ImageName, Skill_Index, OptionIndex) " +
                        "values(" + Num + "," + DB_Num + ",'" + Name + "'," + Rarity + "," + Class + "," + ItemValue + ",'" + ImageName + "'," + Skill_Index + "," + OptionIndex + ")";
            DEB_dbcmd.CommandText = sqlQuery;
            DEB_dbcmd.ExecuteNonQuery();
        }
    }

    // 쿼리 에러 뜸
    void Save_Emblem_PlayData()
    {
        List<Database.Emblem> emblem = database.playData.emblem;
        //Insert Data into Table
        for (int i = 0; i < emblem.Count; i++)
        {
            int Status = (int)emblem[i].status;

            string sqlQuery = "UPDATE Emblem" +
                              " SET Status = " + Status +
                              " WHERE Num = " + i;
            DEB_dbcmd.CommandText = sqlQuery;
            DEB_dbcmd.ExecuteNonQuery();
        }
    }

    #endregion

    //테스트 완료
    #region Database_Load_Method
    //쿼리문으로 그때그떄 찾아서 데이터 들고오는 쉐끼
    public void LoadWeaponData()
    {
        //if (database.GetInventoryCount() <= _num) return null;

        string sqlQuery = "SELECT * FROM WeaponTable WHERE Num = 1";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();

        reader.Read();
        int count = 0;
        int Num = reader.GetInt32(count++);
        string Name = reader.GetString(count++);
        RARITY Rarity = (RARITY)(reader.GetInt32(count++));
        CLASS Class = (CLASS)(reader.GetInt32(count++));
        float Damage = reader.GetFloat(count++);
        int Attack_Count = reader.GetInt32(count++);
        float Attack_Range = reader.GetFloat(count++);
        float Attack_Speed = reader.GetFloat(count++);
        float Nuckback = reader.GetFloat(count++);
        int Item_Value = reader.GetInt32(count++);
        string Description = reader.GetString(count++);
        string ImageName = reader.GetString(count++);
        int Skill_Index = reader.GetInt32(count++);
        string OptionTableName = reader.GetString(count++);

        Debug.Log(new Database.Weapon(Num, Name, Rarity, Class, Damage, Attack_Count, Attack_Range, Attack_Speed, Nuckback, Item_Value, Description, ImageName, Skill_Index, OptionTableName));
        //return new Database.Weapon(Num, Name, Rarity, Class, Damage, Attack_Count, Attack_Range, Attack_Speed, Nuckback, Item_Value, Description, ImageName, Skill_Index, OptionTableName));
        reader.Close();
        reader = null;
    }
    //데이터베이스에서 테이블들을 가져오는 함수들
    //readonly

    void Load_Weapon_Table()
    {
        string sqlQuery = "SELECT * FROM WeaponTable";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            RARITY Rarity = (RARITY)(reader.GetInt32(count++));
            CLASS Class = (CLASS)(reader.GetInt32(count++));
            float Damage = reader.GetFloat(count++);
            int Attack_Count = reader.GetInt32(count++);
            float Attack_Range = reader.GetFloat(count++);
            float Attack_Speed = reader.GetFloat(count++);
            float Nuckback = reader.GetFloat(count++);
            int Item_Value = reader.GetInt32(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);
            int Skill_Index = reader.GetInt32(count++);
            string OptionTableName = reader.GetString(count++);

            database.weapons.Add(new Database.Weapon(Num, Name, Rarity, Class, Damage, Attack_Count, Attack_Range, Attack_Speed, Nuckback, Item_Value, Description, ImageName, Skill_Index, OptionTableName));
        }
        reader.Close();
        reader = null;
    }

    void Load_Armor_Table()
    {
        string sqlQuery = "SELECT * FROM ArmorTable";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            RARITY Rarity = (RARITY)(reader.GetInt32(count++));
            CLASS Class = (CLASS)(reader.GetInt32(count++));
            float Hp = reader.GetFloat(count++);
            int Item_Value = reader.GetInt32(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);
            string OptionTableName = reader.GetString(count++);

            database.armors.Add(new Database.Armor(Num, Name, Rarity, Class, Hp, Item_Value, Description, ImageName, OptionTableName));
        }
        reader.Close();
        reader = null;
    }

    void Load_ActiveSkill_Table()
    {
        string sqlQuery = "SELECT * FROM ActiveSkill";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            float MpCost = reader.GetFloat(count++);
            int Attack_Count = reader.GetInt32(count++);
            float Active_Time = reader.GetFloat(count++);
            float CoolDown = reader.GetFloat(count++);
            float Attack_Range = reader.GetFloat(count++);
            float Attack_Power = reader.GetFloat(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);

            database.skill.Add(new Database.Skill(Num, Name, MpCost, Attack_Count, Active_Time, CoolDown, Attack_Range, Attack_Power, Description, ImageName));
        }
        reader.Close();
        reader = null;
    }

    void Load_Monster_Table()
    {
        string sqlQuery = "SELECT * FROM MonsterTable";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            Monster_Rarity monster_Rarity = (Monster_Rarity)(reader.GetInt32(count++));
            float Damage = reader.GetFloat(count++);
            float Hp = reader.GetFloat(count++);
            float Attack_Range = reader.GetFloat(count++);
            float Move_Speed = reader.GetFloat(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);

            database.monsters.Add(new Database.Monster(Num, Name, monster_Rarity, Damage, Hp, Attack_Range, Move_Speed, Description, ImageName));
        }
        reader.Close();
        reader = null;
    }

    #endregion
}