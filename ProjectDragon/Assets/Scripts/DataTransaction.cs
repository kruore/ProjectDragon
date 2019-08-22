using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;
using Random = UnityEngine.Random;

public class DataTransaction : MonoSingleton<DataTransaction>
{
    //public T Temp_Inventory;
    private Database database;
    public IDbCommand DEB_dbcmd;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.AddComponent<Database>();
        database = GetComponent<Database>();
        DataPhasing();
        DataBaseConnecting();
        LoadAllTableData();
    }

    #region Database Connecting
    /// <summary>
    /// DB에서 접속하고 연결합니다.
    /// </summary>

    void DataPhasing()
    {
        string conn;
        if (Application.platform == RuntimePlatform.Android)
        {
            conn = Application.persistentDataPath + "/DS_Database.sqlite";
            if (!File.Exists(conn))
            {
                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "/StreamingAsset/DS_Database.sqlite");
                loadDB.bytesDownloaded.ToString();
                while (!loadDB.isDone) { }
                File.WriteAllBytes(conn, loadDB.bytes);
            }
        }
    }

    void DataBaseConnecting()
    {
        string conn;
        if (Application.platform == RuntimePlatform.Android)
        {
            conn = "URI=file:" + Application.persistentDataPath + "/DS_Database.sqlite";
        }
        else
        {
            conn = "URI=file:" + Application.dataPath + "/StreamingAsset/DS_Database.sqlite";
        }
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        DEB_dbcmd = dbconn.CreateCommand();
    }

    #endregion

    //모든 테이블의 정보를 로드 합니다.
    void LoadAllTableData()
    {
        LoadPlayerData();
        Load_Weapon_Table();
        //Load_Armor_Table(); //Armor 데이터가 업로드 되면 업데이트
        //Load_Item_Table(); //삭제 위기
        Load_ActiveSkill_Table();
        Load_Passive_Table();
        Load_Monster_Table();
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

    //인벤토리에 아이템을 추가합니다.
    //해당 아이템의 num과 item_Class를 매개변수로 보내주세요
    public void Insert_Inventory_Item(int _item_DBNum, Item_CLASS _item_Class)
    {
        // 아이템 중복되는 것 있으면 amount 컨트롤 해야함
        switch (_item_Class)
        {
            case Item_CLASS.갑옷:
                Database.Armor armor = database.armors[_item_DBNum];
                database.playData.inventory.Add(new Database.Inventory(GetInventoryCount(), armor.num, armor.name, armor.hp, false, armor.item_Value, armor.rarity, Item_CLASS.갑옷, 1, 0, armor.imageName, 1, -1));
                break;
            case Item_CLASS.아이템:
                if(_item_DBNum < 3)
                InsertJem(_item_DBNum);
                break;
            case Item_CLASS.활:
            case Item_CLASS.검:
            case Item_CLASS.지팡이:
                Database.Weapon weapon = database.weapons[_item_DBNum];
                database.playData.inventory.Add(new Database.Inventory(GetInventoryCount(), weapon.num, weapon.name, weapon.damage, false, weapon.item_Value, weapon.rarity, weapon.item_Class, 1, 0, weapon.imageName, 1, weapon.skill_Index));
                break;
        }
    }
    void InsertJem(int _item_DBNum)
    {
        if(_item_DBNum.Equals(0))
        {
            database.playData.inventory[0].amount++;
        }
        else if(_item_DBNum.Equals(1))
        {
            database.playData.inventory[1].amount++;
        }
        else
        {
            database.playData.inventory[2].amount++;
        }
    }
    public void Delete_Inventory_Item(int _item_Inventory_Index)
    {
        if (_item_Inventory_Index > 2)
        {
            database.playData.inventory.RemoveAt(_item_Inventory_Index);
        }

        for(int i = _item_Inventory_Index; i <= GetInventoryCount(); i++)
        {
            database.playData.inventory[i].num--;
        }
    }
    //인벤토리에 현재 몇개의 아이템을 가지고 있는지 반환합니다.
    public int GetInventoryCount()
    {
        return database.playData.inventory.Count;
    }

    //얻은 패시브를 플레이데이터에 추가하기 위한 함수
    public void Add_PassivetoPlayData(int _passive_DBNum)
    {
        database.playData.passive.Add(database.passive[_passive_DBNum]);
    }
    public void Remove_PassivetoPlayData(int _passive_DBNum)
    {
        database.playData.passive.RemoveAt(_passive_DBNum);
    }

    public int Convert_EquipmenttoJam(Database.Inventory _EquipItem)
    {
        int[] jem_Amount = new int[10];

        switch(_EquipItem.rarity)
        {
            case RARITY.노말:
                jem_Amount = new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9};
                break;
            case RARITY.레어:
                jem_Amount = new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13 };
                break;
            case RARITY.유니크:
                jem_Amount = new int[] { 15, 16, 17, 18, 19, 20, 21, 22, 23 };
                break;
            case RARITY.레전드:
                jem_Amount = new int[] { 40, 42, 44, 46, 48, 50, 60, 70, 80 };
                break;
        }

        return jem_Amount[_EquipItem.upgrade_Level - 1];
    }

    //2019.08.19 - 김동하
    //패시브 테이블에서 이미 가지고 있는 패시브를 제외하고
    //몇 장을 뽑고싶은지 지정한 만큼의 패시브를 랜덤으로 뽑습니다.
    //몇 장 뽑을지 지정을 안했을 경우 default로 3장의 카드가 뽑힙니다.
    public List<Database.Passive> Rand_Passive(int _amount = 3)
    {
        List<Database.Passive> result_Passive = new List<Database.Passive>();
        List<Database.Passive> Passive = database.passive;

        //중복 제거
        for(int i = 0; i < database.playData.passive.Count; i++)
        {
            for(int j = 0; j < Passive.Count; j++)
            {
                if(Passive[j].num.Equals(database.playData.passive[i].num))
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

    //플레이어 데이터를 로드합니다.
    public void LoadPlayerData()
    {
        //플레이어 테이블 데이터 로드
        Load_Inventory_Table();

        //플레이어 기본 데이터 로드
        Load_PlayerPrefs_Data();

        Load_Passive_PlayData();
    }

    public void SavePlayerData()
    {
        //플레이어 테이블 데이터 저장
        Save_Inventory_Table();

        //플레이어 기본 데이터 저장
        Save_PlayerPrefs_Data();

        Save_Passive_PlayData();
    }


    #region Database_Load_Player_Data
    //플레이어 데이터 로드 함수

    void Load_PlayerPrefs_Data()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            database.playData.currentHp = PlayerPrefs.GetFloat("currentHp");
            database.playData.clearStage = PlayerPrefs.GetInt("clearStage");
            database.playData.mp = PlayerPrefs.GetInt("mp");
            database.playData.sex = (SEX)PlayerPrefs.GetInt("sex");
            database.playData.equiWeapon_InventoryNum = PlayerPrefs.GetInt("equiWeapon_InventoryNum");
            database.playData.equiArmor_InventoryNum = PlayerPrefs.GetInt("equiArmor_InventoryNum");
        }
        else
        {
            database.playData.currentHp = 0.0f;
            database.playData.clearStage = 0;
            database.playData.mp = 0;
            database.playData.sex = 0;
            database.playData.equiWeapon_InventoryNum = 0;
            database.playData.equiArmor_InventoryNum = 0;
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


    #region Database_Save_Player_Data

    void Save_PlayerPrefs_Data()
    {
        PlayerPrefs.SetInt("save", 1);
        PlayerPrefs.SetFloat("currentHp", database.playData.currentHp);
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

    //
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

    void ResetAll_PlayData()
    {
        database.playData.inventory.RemoveRange(3, database.playData.inventory.Count-3);
        database.playData.passive.Clear();
        database.playData.currentHp = 0.0f;
        database.playData.clearStage = 0;
        database.playData.mp = 0;
        database.playData.sex = 0;
        database.playData.equiWeapon_InventoryNum = 0;  
        database.playData.equiArmor_InventoryNum = 0;
    }

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

    //void Load_Item_Table()
    //{
    //    string sqlQuery = "SELECT * FROM ItemTable";
    //    DEB_dbcmd.CommandText = sqlQuery;
    //    IDataReader reader = DEB_dbcmd.ExecuteReader();
    //    while (reader.Read())
    //    {
    //        int Num = reader.GetInt32(0);
    //        string Name = reader.GetString(1);
    //        int Item_Value = reader.GetInt32(2);
    //        RARITY Rarity = (RARITY)(reader.GetInt32(3));
    //        Item_CLASS item_Class = (Item_CLASS)(reader.GetInt32(4));
    //        string Description = reader.GetString(5);

    //        database.items.Add(new Database.Item(Num, Name, Item_Value, Rarity, item_Class, Description));
    //    }
    //    reader.Close();
    //    reader = null;
    //}

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
