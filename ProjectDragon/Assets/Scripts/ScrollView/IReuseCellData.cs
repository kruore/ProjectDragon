
public interface IReuseCellData
{
    int Index
    {
        get;
        set;
    }
    int DB_Num
    {
        get;
        set;
    }
    string name
    {
        get;
        set;
    }
    int itemValue
    {
        get;
        set;
    } // 아이템 가치 - 강화젬의 강화 수치 같은 것들
    RARITY rarity
    {
        get;
        set;
    } // 희귀도
    Item_CLASS item_Class
    {
        get;
        set;
    } // 아이템 타입
    int upgrade_Level
    {
        get;
        set;
    }//아이템 레벨
    int upgrade_Count
    {
        get;
        set;
    }//강화 진행중 정도 - 아이템 경험치
    string imageName
    {
        get;
        set;
    } //이미지 이름
    int amount
    {
        get;
        set;
    } // 갯수
    bool isEquipment
    {
        get;
        set;
    } // 장착중인가?

}
