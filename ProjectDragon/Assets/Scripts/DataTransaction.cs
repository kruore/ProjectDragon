using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.Linq;
using System.IO;
using System.Data;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DataTransaction : MonoSingleton<DataTransaction>
{
    //public Text text;
    //public T Temp_Inventory;
    private Database database;
    public IDbCommand DEB_dbcmd;

    private void Awake()
    {
        StartCoroutine(DataPhasing());
        gameObject.AddComponent<Database>();
        database = GetComponent<Database>();
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
        //text.text = "쓋";
        dbconn.Open();
        //text.text = "open";
        DEB_dbcmd = dbconn.CreateCommand();
    }

    #endregion

    //모든 테이블의 정보를 로드 합니다.
    IEnumerator LoadAllTableData()
    {
        LoadPlayerData();
        Load_Weapon_Table();
        Load_Armor_Table();
        Load_Item_StatPerLevel_Table();
        Load_ActiveSkill_Table();
        Load_Passive_Table();
        Load_Monster_Table();

        yield return null;
        StopCoroutine(LoadAllTableData());
    }

    //플레이어 데이터를 로드합니다.
    public void LoadPlayerData()
    {
        //플레이어 테이블 데이터 로드
        Load_Inventory_Table();

        //플레이어 기본 데이터 로드
        Load_PlayerPrefs_Data();

        //플레이어의 패시브 데이터를 로드
        Load_Passive_PlayData();
    }

    public void SavePlayerData()
    {
        //플레이어 테이블 데이터 저장
        Save_Inventory_Table();

        //플레이어 기본 데이터 저장
        Save_PlayerPrefs_Data();

        //플레이어의 패시브를 저장
        Save_Passive_PlayData();
        Debug.Log("Save Player Data Complete");
    }

    //구글 연결 함수
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
    public void EndGame_Get_Item(List<Database.Weapon> _weapon_List, List<Database.Armor> _armor_List, int[] jem_amounts, int _mp = 0)
    {
        List<Database.Inventory> inventories = new List<Database.Inventory>();

        inventories = Convert_InventoryList_fromItem(_weapon_List, _armor_List);

        //인벤토리에 아이템 삽입
        Insert_Inventory_Item(inventories);

        //잼 넣기
        for (int i = 0; i < 3; i++)
        {
            if (!jem_amounts[i].Equals(0))
            {
                Change_InventoryJem(i, jem_amounts[i]);
            }
        }
        Change_ManaPower(_mp);
    }

    //
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

    //인벤토리에 아이템을 추가합니다.
    //해당 아이템의 num과 item_Class를 매개변수로 보내주세
    public void Insert_Inventory_Item(List<Database.Inventory> _inventories)
    {
        // 아이템 중복되는 것 있으면 amount 컨트롤 해야함
        database.playData.inventory.AddRange(_inventories);
    }

    /// <summary>
    /// 리얼 mp를 _amount만큼 증가시키거나 감소시킵니다.
    /// </summary>
    /// <param name="_amount"></param>
    public void Change_ManaPower(int _amount)
    {
        database.playData.mp += _amount;
    }
     
    /// <summary>
    /// 인벤토리의 잼의 갯수를 _amount만큼 증가시키거나 감소시킵니다. jem은 인벤토리에서 0~2에 있습니다.
    /// </summary>
    /// <param name="_gem_DBNum"></param>
    /// <param name="_amount"></param>
    public void Change_InventoryJem(int _gem_DBNum, int _amount)
    {
        if(_gem_DBNum > 2)
        {
            Debug.LogError("Datatransaction::Change_InventoryJem(), out of index, Jem index are from 0 to 2.");
            return;
        }
        database.playData.inventory[_gem_DBNum].amount += _amount;
    }

    /// <summary>
    /// 인벤토리에서 아이템을 삭제합니다.
    /// </summary>
    /// <param name="_item_Inventory_Num"></param>
    public void Delete_Inventory_Item(int _item_Inventory_Num)
    {
        if (_item_Inventory_Num > 2)
        {
            database.playData.inventory.RemoveAt(_item_Inventory_Num);

            //테이블상에서 삭제되는 아이템보다 num가 높았던 데이터의 inventory의 num을 조정합니다.
            for (int i = _item_Inventory_Num; i < database.GetInventoryCount(); i++)
            {
                database.playData.inventory[i].num--;
            }
        }
        else
        {
            Debug.Log("인벤토리에서 2이하의 인덱스는 item_jem의 영역입니다.");
            return;
        }

        //삭제된 아이템의 inventory의 num이 현재 장착중인 아이템의 inventory의 num보다 작으면 현재 장착중인 아이템의 inventory의 num 줄입니다.
        if (database.playData.equiWeapon_InventoryNum > _item_Inventory_Num)
        {
            database.playData.equiWeapon_InventoryNum--;
        }
        //삭제된 아이템의 inventory의 num이 현재 장착중인 아이템의 inventory의 nu보다 작으면 현재 장착중인 아이템의 inventory의 num 줄입니다.
        if (database.playData.equiArmor_InventoryNum > _item_Inventory_Num)
        {
            database.playData.equiWeapon_InventoryNum--;
        }

    }

    /// <summary>
    /// 플레이데이터에 패시브를 추가합니다.
    /// </summary>
    /// <param name="_passive_DBNum"></param>
    public void Add_PassivetoPlayData(Database.Passive _passive)
    {
        database.playData.passive.Add(_passive);
    }

    /// <summary>
    /// 패시브 삭제용 함수 - 쓰일지는 잘 모르겠다.
    /// </summary>
    /// <param name="_passive_DBNum"></param>
    public void Remove_PassivetoPlayData(int _equipPassive_Num)
    {
        database.playData.passive.RemoveAt(_equipPassive_Num);
    }

    /// <summary>
    /// 현재 장착중인 아이템의 스킬을 반환합니다.
    /// </summary>
    /// <returns></returns>
    public Database.Skill CurrentSkill()
    {
        return database.skill[database.playData.inventory[database.playData.equiWeapon_InventoryNum].skill_Index];
    }

    /// <summary>
    /// 무기를 해단 무기의 스킬로 변환하여 반환합니다.
    /// </summary>
    /// <param name="_EquipItem"></param>
    /// <returns></returns>
    public Database.Skill Convert_ItemtoSkill(Database.Inventory _EquipItem)
    {
        if(_EquipItem.item_Class.Equals(Item_CLASS.갑옷) || _EquipItem.item_Class.Equals(Item_CLASS.아이템))
        {
            Debug.LogError("DataTransaction::Convert_ItemtoSkill(), Please give me an item with skill");
            return null;
        }

        return database.skill[_EquipItem.skill_Index];
    }

    /// <summary> 
    /// 장비 아이템을 분해할 시 잼이 얼마나 나올지를 반환합니다.
    /// </summary>
    /// <param name="_EquipItem"></param>
    /// <returns></returns>
    public int Convert_EquipmenttoJam(Database.Inventory _EquipItem)
    {
        int[] jem_Amount = new int[10];

        switch (_EquipItem.rarity)
        {
            case RARITY.노말:
                jem_Amount = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 20 };
                break;
            case RARITY.레어:
                jem_Amount = new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 30 };
                break;
            case RARITY.유니크:
                jem_Amount = new int[] { 15, 16, 17, 18, 19, 20, 21, 22, 23, 50 };
                break;
            case RARITY.레전드:
                jem_Amount = new int[] { 40, 42, 44, 46, 48, 50, 60, 70, 80, 100 };
                break;
        }

        return jem_Amount[_EquipItem.upgrade_Level - 1];
    }

    /// <summary>
    /// 아이템 강화시 다음 레벨까지 필요한 경험치를 반환합니다.
    /// </summary>
    /// <param name="_EquipItem"></param>
    /// <returns></returns>
    public int Convert_ItemtoUpgradeCount(Database.Inventory _EquipItem)
    {
        if (_EquipItem.upgrade_Level.Equals(10))
        {
            Debug.Log("_EquipItem_Level is 10, cannot Updgrade");
            return -1;
        }

        int[] upgradeCount = new int[9];
        int level = _EquipItem.upgrade_Level - 1;

        switch (_EquipItem.rarity)
        {
            case RARITY.노말:
                upgradeCount = new int[] { 1, 2, 3, 5, 6, 7, 9, 10, 10 };
                break;
            case RARITY.레어:
                upgradeCount = new int[] { 6, 8, 10, 14, 16, 18, 20, 25, 30 };
                break;
            case RARITY.유니크:
                upgradeCount = new int[] { 10, 14, 18, 20, 25, 30, 35, 40, 50 };
                break;
            case RARITY.레전드:
                upgradeCount = new int[] { 30, 40, 50, 60, 70, 80, 90, 95, 100 };
                break;
        }

        return upgradeCount[level];
    }

    /// <summary>
    /// 아이템의 추가 능력치를 반환합니다.
    /// </summary>
    /// <param name="_EquipItem"></param>
    /// <returns></returns>
    public int Convert_ItemLeveltoStat(Database.Inventory _EquipItem)
    {
        if (_EquipItem.upgrade_Level.Equals(1))
        {
            return 0;
        }

        int num = 0;
        int level = _EquipItem.upgrade_Level - 2;

        //아이템 종류에 따른 index 위치 변화
        switch (_EquipItem.item_Class)
        {
            //0
            case Item_CLASS.검:
                num = 0;
                break;
            //4
            case Item_CLASS.활:
                num = 4;
                break;
            //8
            case Item_CLASS.지팡이:
                num = 8;
                break;
            //12
            case Item_CLASS.갑옷:
                num = 12;
                break;
        }

        //레어리티에 따른 index값 변화
        switch (_EquipItem.rarity)
        {
            case RARITY.노말:
                break;
            case RARITY.레어:
                num += 1;
                break;
            case RARITY.유니크:
                num += 2;
                break;
            case RARITY.레전드:
                num += 3;
                break;
        }

        return database.statPerLevel[num, level];
    }


    /// <summary>
    /// 패시브 테이블에서 이미 가지고 있는 패시브를 제외하고
    /// 몇 장을 뽑고싶은지 지정한 만큼의 패시브를 랜덤으로 뽑습니다.
    /// 몇 장 뽑을지 지정을 안했을 경우 default로 3장의 카드가 뽑힙니다.
    /// </summary>
    /// <param name="_amount"></param>
    /// <returns></returns>
    public List<Database.Passive> Rand_Passive(int _amount = 3)
    {
        //뽑으려는 패시브의 수가 0보다 작으면 에러와 null을 반환
        if(_amount <= 0)
        {
            Debug.LogError("DataTransaction::Rand_Passive(), Please give number more than 0 where param _amount");
            return null;
        }

        List<Database.Passive> result_Passive = new List<Database.Passive>();
        List<Database.Passive> Passive = new List<Database.Passive>(database.passive);

        //중복 제거
        for (int i = 0; i < database.playData.passive.Count; i++)
        {
            for (int j = 0; j < Passive.Count; j++)
            {
                if (Passive[j].num.Equals(database.playData.passive[i].num))
                {
                    Passive.RemoveAt(j);
                    break;
                }
            }
        }

        //중복없는 패시브 3개 랜덤 뽑기
        int count = Passive.Count - 1;
        for (int i = 0; i < _amount; i++)
        {
            int index = Random.Range(i, count);

            Database.Passive temp = Passive[index];
            Passive[index] = Passive[i];
            Passive[i] = temp;
            result_Passive.Add(temp);
        }

        return result_Passive;
    }


    #endregion

    //테스트 완료
    #region Database_Load_Player_Data
    //플레이어 데이터 로드 함수

    void Load_PlayerPrefs_Data()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            database.playData.currentHp = PlayerPrefs.GetFloat("currentHp");
            database.playData.damage = PlayerPrefs.GetFloat("damage");
            database.playData.moveSpeed = PlayerPrefs.GetFloat("moveSpeed");
            database.playData.clearStage = PlayerPrefs.GetInt("clearStage");
            database.playData.mp = PlayerPrefs.GetInt("mp");
            database.playData.sex = (SEX)PlayerPrefs.GetInt("sex");
            database.playData.equiWeapon_InventoryNum = PlayerPrefs.GetInt("equiWeapon_InventoryNum");
            database.playData.equiArmor_InventoryNum = PlayerPrefs.GetInt("equiArmor_InventoryNum");
        }
        else
        {
            database.playData.currentHp = 100.0f;
            database.playData.damage = 10.0f;
            database.playData.moveSpeed = 1.0f;
            database.playData.clearStage = 0;
            database.playData.mp = 0;
            database.playData.sex = 0;
            database.playData.equiWeapon_InventoryNum = -1;
            database.playData.equiArmor_InventoryNum = -1;
        }
    }

    void Load_Inventory_Table()
    {
        string sqlQuery = "SELECT * FROM Inventory";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int Num = reader.GetInt32(0);
            int DB_Num = reader.GetInt32(1);
            string Name = reader.GetString(2);
            float Stat = reader.GetFloat(3);
            bool IsLock = (reader.GetInt32(4) == 1) ? true : false;
            int ItemValue = reader.GetInt32(5);
            RARITY Rarity = (RARITY)(reader.GetInt32(6));
            Item_CLASS item_Class = (Item_CLASS)(reader.GetInt32(7));
            int Upgrade_Level = reader.GetInt32(8);
            int Upgrade_Count = reader.GetInt32(9);
            string ImageName = reader.GetString(10);
            int Amount = reader.GetInt32(11);
            int skill_Index = reader.GetInt32(12);

            database.playData.inventory.Add(new Database.Inventory(Num, DB_Num, Name, Stat, IsLock, ItemValue, Rarity, item_Class, Upgrade_Level, Upgrade_Count, ImageName, Amount, skill_Index));
        }
        reader.Close();
        reader = null;
    }


    void Load_Passive_PlayData()
    {
        string sqlQuery = "SELECT * FROM PassivePlayData";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int Num = reader.GetInt32(0);
            string Name = reader.GetString(1);
            int World = reader.GetInt32(2);
            string Description = reader.GetString(3);
            string ImageName = reader.GetString(4);
            PASSIVE_TYPE PassiveType = (PASSIVE_TYPE)reader.GetInt32(5);
            int[] StatPerLV = { reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), };
            database.playData.passive.Add(new Database.Passive(Num, World, Name, Description, ImageName, PassiveType, StatPerLV));
        }
        reader.Close();
        reader = null;
    }
    #endregion

    //테스트 완료
    #region Database_Save_Player_Data

    void Save_PlayerPrefs_Data()
    {
        PlayerPrefs.SetInt("save", 1);
        PlayerPrefs.SetFloat("currentHp", database.playData.currentHp);
        PlayerPrefs.SetFloat("damage", database.playData.damage);
        PlayerPrefs.SetFloat("moveSpeed", database.playData.moveSpeed);
        PlayerPrefs.SetInt("clearStage", database.playData.clearStage);
        PlayerPrefs.SetInt("mp", database.playData.mp);
        PlayerPrefs.SetInt("sex", (int)database.playData.sex);
        PlayerPrefs.SetInt("equiWeapon_InventoryNum", database.playData.equiWeapon_InventoryNum);
        PlayerPrefs.SetInt("equiArmor_InventoryNum", database.playData.equiArmor_InventoryNum);
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
            float Stat = database.playData.inventory[i].stat;
            int IsLock = (database.playData.inventory[i].isLock == true) ? 1 : 0;
            int ItemValue = database.playData.inventory[i].itemValue;
            int Rarity = (int)database.playData.inventory[i].rarity;
            int item_Class = (int)database.playData.inventory[i].item_Class;
            int Upgrade_Level = database.playData.inventory[i].upgrade_Level;
            int Upgrade_Count = database.playData.inventory[i].upgrade_Count;
            string ImageName = database.playData.inventory[i].imageName;
            int Amount = database.playData.inventory[i].amount;
            int Skill_Index = database.playData.inventory[i].skill_Index;

            sqlQuery = "INSERT INTO Inventory(Num, DB_Num, Name, Stat, IsLock, ItemValue, Rarity, item_Class, Upgrade_Level, Upgrade_Count, ImageName, Amount, Skill_Index) " +
                        "values(" + Num + "," + DB_Num + ",'" + Name + "'," + Stat + "," + IsLock + "," + ItemValue + "," + Rarity + "," + item_Class + "," + Upgrade_Level + "," + Upgrade_Count + ",'" + ImageName + "'," + Amount + "," + Skill_Index + ")";
            DEB_dbcmd.CommandText = sqlQuery;
            DEB_dbcmd.ExecuteNonQuery();
        }
    }


    void Save_Passive_PlayData()
    {
        //Reset Table
        string sqlQuery = "DELETE FROM PassivePlayData";
        DataTransaction.Inst.DEB_dbcmd.CommandText = sqlQuery;
        DataTransaction.Inst.DEB_dbcmd.ExecuteNonQuery();

        List<Database.Passive> passive = database.playData.passive;
        //Insert Data into Table
        for (int i = 0; i < passive.Count; i++)
        {
            int Num = passive[i].num;
            string Name = passive[i].name;
            int World = passive[i].world;
            string Description = passive[i].description;
            string ImageName = passive[i].imageName;
            int PassiveType = (int)passive[i].passiveType;
            int E1 = passive[i].statPerLV[0];
            int E2 = passive[i].statPerLV[1];
            int E3 = passive[i].statPerLV[2];
            int E4 = passive[i].statPerLV[3];

            sqlQuery = "INSERT INTO PassivePlayData(Num, Name, World, Description, ImageName, PassiveType, E1, E2, E3, E4) " +
                        "values(" + Num + ",'" + Name + "'," + World + ",'" + Description + "','" + ImageName + "'," + PassiveType + "," + E1 + "," + E2 + "," + E3 + "," + E4 + ")";
            DataTransaction.Inst.DEB_dbcmd.CommandText = sqlQuery;
            DataTransaction.Inst.DEB_dbcmd.ExecuteNonQuery();
        }
    }

    #endregion

    //테스트 완료
    public void ResetAll_PlayData()
    {
        database.playData.inventory.RemoveRange(3, database.playData.inventory.Count - 3);
        database.playData.passive.Clear();
        database.playData.currentHp = 100.0f;
        database.playData.damage = 10.0f;
        database.playData.moveSpeed = 1.0f;
        database.playData.clearStage = 0;
        database.playData.mp = 0;
        database.playData.sex = 0;
        database.playData.equiWeapon_InventoryNum = -1;
        database.playData.equiArmor_InventoryNum = -1;
        //세이브
        SavePlayerData();
    }

    //테스트 완료
    #region Database_Load_Method
    //데이터베이스에서 테이블들을 가져오는 함수들
    //readonly

    void Load_Weapon_Table()
    {
        string sqlQuery = "SELECT * FROM WeaponTable";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int Num = reader.GetInt32(0);
            string Name = reader.GetString(1);
            float Damage = reader.GetFloat(2);
            int Attack_Count = reader.GetInt32(3);
            float Attack_Range = reader.GetFloat(4);
            string Attack_Type = reader.GetString(5);
            float Attack_Speed = reader.GetFloat(6);
            int Item_Value = reader.GetInt32(7);
            string Description = reader.GetString(8);
            int Skill_Index = reader.GetInt32(9);
            RARITY Rarity = (RARITY)(reader.GetInt32(10));
            Item_CLASS Item_Class = (Item_CLASS)(reader.GetInt32(11));
            string ImageName = reader.GetString(12);

            database.weapons.Add(new Database.Weapon(Num, Name, Damage, Attack_Count, Attack_Range, Attack_Type, Attack_Speed, Item_Value, Description, Skill_Index, Rarity, Item_Class, ImageName));
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
            int Num = reader.GetInt32(0);
            string Name = reader.GetString(1);
            float Hp = reader.GetFloat(2);
            int Item_Value = reader.GetInt32(3);
            string Description = reader.GetString(4);
            RARITY Rarity = (RARITY)(reader.GetInt32(5));
            Item_CLASS item_Class = (Item_CLASS)(reader.GetInt32(6));
            string ImageName = reader.GetString(7);

            database.armors.Add(new Database.Armor(Num, Name, Hp, Item_Value, Description, Rarity, item_Class, ImageName));
        }
        reader.Close();
        reader = null;
    }

    void Load_Item_StatPerLevel_Table()
    {
        string sqlQuery = "SELECT * FROM ItemStat";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int Num = reader.GetInt32(0);
            for (int i = 0; i < 9; i++)
            {
                database.statPerLevel[Num, i] = reader.GetInt32(i + 1);
            }
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
            int Num = reader.GetInt32(0);
            string Name = reader.GetString(1);
            string Description = reader.GetString(2);
            float MpCost = reader.GetFloat(3);
            int Attack_Count = reader.GetInt32(4);
            float Active_Time = reader.GetFloat(5);
            float CoolDown = reader.GetFloat(6);
            float Attack_Range = reader.GetFloat(7);
            string Attack_Type = reader.GetString(8);
            float Attack_Power = reader.GetFloat(9);
            string ImageName = reader.GetString(10);

            database.skill.Add(new Database.Skill(Num, Name, Description, MpCost, Attack_Count, Active_Time, CoolDown, Attack_Range, Attack_Type, Attack_Power, ImageName));
        }
        reader.Close();
        reader = null;
    }

    void Load_Passive_Table()
    {
        string sqlQuery = "SELECT * FROM Passive";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int Num = reader.GetInt32(0);
            string Name = reader.GetString(1);
            int World = reader.GetInt32(2);
            string Description = reader.GetString(3);
            string ImageName = reader.GetString(4);
            PASSIVE_TYPE PassiveType = (PASSIVE_TYPE)reader.GetInt32(5);
            int[] StatPerLV = { reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), };
            database.passive.Add(new Database.Passive(Num, World, Name, Description, ImageName, PassiveType, StatPerLV));
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
            int Num = reader.GetInt32(0);
            Monster_Region Region = (Monster_Region)reader.GetInt32(1);
            string Name = reader.GetString(2);
            float Damage = reader.GetFloat(3);
            float Hp = reader.GetFloat(4);
            Monster_Rarity monster_Rarity = (Monster_Rarity)(reader.GetInt32(5));
            Monster_Size Size = (Monster_Size)(reader.GetInt32(6));
            float Attack_Range = reader.GetFloat(7);
            string Attack_Type = reader.GetString(8);
            float Attack_Speed = reader.GetFloat(9);
            float Chase_Range = reader.GetFloat(10);
            float Move_Speed = reader.GetFloat(11);
            Monster_Category Category = (Monster_Category)reader.GetInt32(12);
            string Description = reader.GetString(13);
            string ImageName = reader.GetString(14);

            database.monsters.Add(new Database.Monster(Num, Region, Name, Damage, Hp, monster_Rarity, Size, Attack_Range, Attack_Type, Attack_Speed, Chase_Range, Move_Speed, Category, Description, ImageName));
        }
        reader.Close();
        reader = null;
    }

    #endregion
}
