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
    void Start()
    {
        database = GetComponent<Database>();
        if(database==null)
        {
            gameObject.AddComponent<Database>();
            database = GetComponent<Database>();
        }
        DataPhasing();
        DataBaseConnecting();
        LoadAllTableData();
    }

    #region Database Connecting

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

    void LoadAllTableData()
    {
        Load_Weapon_Table();
        Load_Armor_Table();
        Load_Item_Table();
        Load_ActiveSkill_Table();
        Load_Passive_Table();
        Load_Monster_Table();
        Load_Inventory_Table();
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
            database.playData.currentHp = PlayerPrefs.GetFloat("currentHp");
            database.playData.clearStage = PlayerPrefs.GetInt("clearStage");
            database.playData.mp = PlayerPrefs.GetInt("mp");
            database.playData.sex = (SEX)PlayerPrefs.GetInt("sex");
        }
        else
        {
            database.playData.currentHp = 0.0f;
            database.playData.clearStage = 0;
            database.playData.mp = 0;
            database.playData.sex = 0;
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
            int ItemValue = reader.GetInt32(3);
            RARITY Rarity = (RARITY)(reader.GetInt32(4));
            Item_CLASS item_Class = (Item_CLASS)(reader.GetInt32(5));
            int Upgrade_Level = reader.GetInt32(6);
            int Upgrade_Count = reader.GetInt32(7);
            string ImageName = reader.GetString(8);
            int Amount = reader.GetInt32(9);
            bool IsEquipment = (reader.GetInt32(10) == 1) ? true : false;

            database.playData.inventory.Add(new Database.Inventory(Num, DB_Num, Name, ItemValue, Rarity, item_Class, Upgrade_Level, Upgrade_Count, ImageName, Amount, IsEquipment));
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
            int ItemValue = database.playData.inventory[i].itemValue;
            int Rarity = (int)database.playData.inventory[i].rarity;
            int item_Class = (int)database.playData.inventory[i].item_Class;
            int Upgrade_Level = database.playData.inventory[i].upgrade_Level;
            int Upgrade_Count = database.playData.inventory[i].upgrade_Count;
            string ImageName = database.playData.inventory[i].imageName;
            int Amount = database.playData.inventory[i].amount;
            int IsEquipment = (database.playData.inventory[i].isEquipment == true) ? 1 : 0;

            sqlQuery = "INSERT INTO Inventory(Num, DB_Num, Name, ItemValue, Rarity, item_Class, Upgrade_Level, Upgrade_Count, ImageName, Amount, IsEquipment) " +
                        "values(" + Num + "," + DB_Num + ",'" + Name + "'," + ItemValue + "," + Rarity + "," + item_Class + "," + Upgrade_Level + "," + Upgrade_Count + ",'" + ImageName + "'," + Amount + "," + IsEquipment + ")";
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
            float Chase_Range = reader.GetFloat(7);
            string Description = reader.GetString(8);
            string Skill = reader.GetString(9);
            RARITY Rarity = (RARITY)(reader.GetInt32(10));
            Item_CLASS Item_Class = (Item_CLASS)(reader.GetInt32(11));

            database.weapons.Add(new Database.Weapon(Num, Name, Damage, Attack_Count, Attack_Range, Attack_Type, Attack_Speed, Chase_Range, Description, Skill, Rarity, Item_Class));
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
            string Description = reader.GetString(3);
            RARITY Rarity = (RARITY)(reader.GetInt32(4));
            Item_CLASS item_Class = (Item_CLASS)(reader.GetInt32(5));

            database.armors.Add(new Database.Armor(Num, Name, Hp, Description, Rarity, item_Class));
        }
        reader.Close();
        reader = null;
    }

    void Load_Item_Table()
    {
        string sqlQuery = "SELECT * FROM ItemTable";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int Num = reader.GetInt32(0);
            string Name = reader.GetString(1);
            int Item_Value = reader.GetInt32(2);
            RARITY Rarity = (RARITY)(reader.GetInt32(3));
            Item_CLASS item_Class = (Item_CLASS)(reader.GetInt32(4));
            string Description = reader.GetString(5);

            database.items.Add(new Database.Item(Num, Name, Item_Value, Rarity, item_Class, Description));
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
            int Attack_Count = reader.GetInt32(3);
            float Active_Time = reader.GetFloat(4);
            float CoolDown = reader.GetFloat(5);
            float Attack_Range = reader.GetFloat(6);
            string Attack_Type = reader.GetString(7);
            float Attack_Power = reader.GetFloat(8);

            database.skill.Add(new Database.Skill(Num, Name, Description, Attack_Count, Active_Time, CoolDown, Attack_Range, Attack_Type, Attack_Power));
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
            float Damage = reader.GetFloat(4);
            float Hp = reader.GetFloat(5);
            float Attack_Speed = reader.GetFloat(6);
            float Move_Speed = reader.GetFloat(7);

            database.passive.Add(new Database.Passive(Num, World, Name, Description, Damage, Hp, Attack_Speed, Move_Speed));
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
            int Location = reader.GetInt32(1);
            string Name = reader.GetString(2);
            float Damage = reader.GetFloat(3);
            float Hp = reader.GetFloat(4);
            Monster_Rarity monster_Rarity = (Monster_Rarity)(reader.GetInt32(5));
            float Attack_Range = reader.GetFloat(6);
            string Attack_Type = reader.GetString(7);
            float Attack_Speed = reader.GetFloat(8);
            float Chase_Range = reader.GetFloat(9);
            float Move_Speed = reader.GetFloat(10);
            bool IsPossibleMove = (reader.GetInt32(11) == 1) ? true : false;
            string Description = reader.GetString(12);

            database.monsters.Add(new Database.Monster(Num, Location, Name, Damage, Hp, monster_Rarity, Attack_Range, Attack_Type, Attack_Speed, Chase_Range, Move_Speed, IsPossibleMove, Description));
        }
        reader.Close();
        reader = null;
    }

    #endregion
}
