using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

public class DataTransaction : MonoSingleton<DataTransaction>
{
    //public T Temp_Inventory;
    private Database database;
    private IDbCommand DEB_dbcmd;

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
                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "/StreamingAssets/DS_Database.sqlite");
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
            conn = "URI=file:" + Application.dataPath + "/StreamingAssets/DS_Database.sqlite";
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
        Load_Armor_Table(); 
        //Load_Item_Table();
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

    //인벤토리에 아이템 넣기 - 나중에
    public void Insert_Inventory(int _item_Index, Item_CLASS _item_Class)
    { 
        // 아이템 중복되는 것 있으면 amount 컨트롤 해야함
        switch (_item_Class)
        {
            case Item_CLASS.갑옷:
                Database.Armor armor = database.armors[_item_Index];
                database.playData.inventory.Add(new Database.Inventory(database.playData.itemCount, armor.num, armor.name, armor.hp, false, armor.item_Value, armor.rarity, Item_CLASS.갑옷, 1, 0, armor.imageName, 1, -1));
                break;
            case Item_CLASS.아이템:
                //atabase.Item item = database.items[_item_Index];
                break;
            default:
                //무기
                Database.Weapon weapon = database.weapons[_item_Index];   
                break;
        }
        database.playData.itemCount++;
    }


    //플레이어 데이터를 로드합니다.
    public void LoadPlayerData()
    {
        //플레이어 테이블 데이터 로드
        Load_Inventory_Table();

        //플레이어 기본 데이터 로드
        Load_PlayerPrefs_Data();
    }

    public void SavePlayerData()
    {
        //플레이어 테이블 데이터 저장
        Save_Inventory_Table();

        //플레이어 기본 데이터 저장
        Save_PlayerPrefs_Data();
    }


    #region Database_Load_Player_Data
    //플레이어 데이터 로드 함수

    void Load_PlayerPrefs_Data()
    { 
        if (PlayerPrefs.HasKey("save"))
        {
            database.playData.itemCount = PlayerPrefs.GetInt("itemCount");
            database.playData.currentHp = PlayerPrefs.GetFloat("currentHp");
            database.playData.clearStage = PlayerPrefs.GetInt("clearStage");
            database.playData.mp = PlayerPrefs.GetInt("mp");
            database.playData.sex = (SEX)PlayerPrefs.GetInt("sex");
            database.playData.equiWeapon_InventoryNum = PlayerPrefs.GetInt("equiWeapon_InventoryNum");
            database.playData.equiArmor_InventoryNum = PlayerPrefs.GetInt("equiArmor_InventoryNum");
        }
        else
        {
            database.playData.itemCount = 0;
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

    #endregion


    #region Database_Save_Player_Data

    void Save_PlayerPrefs_Data()
    {
        PlayerPrefs.SetInt("save", 1);
        PlayerPrefs.SetInt("itemCount", database.playData.itemCount);
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

    #endregion


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
            float Damage = reader.GetFloat(5);
            float Hp = reader.GetFloat(6);
            float Attack_Speed = reader.GetFloat(7);
            float Move_Speed = reader.GetFloat(8);

            database.passive.Add(new Database.Passive(Num, World, Name, Description, ImageName, Damage, Hp, Attack_Speed, Move_Speed));
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
