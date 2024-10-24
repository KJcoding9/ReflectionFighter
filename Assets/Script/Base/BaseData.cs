using UnityEngine;


public enum DataType
{
    HP,
    MaxHP,
}
//ベースデータ
public class BaseData
{
    //タイトル
    public string Title;
    //データID
    public int Id;
    // 説明文
    [TextArea] public string Description;
    //体力
    public int HP;
    //体力最大
    public int MaxHP;

    //インデクサー
    public float this[DataType key]
    {
        get
        {
            if (key == DataType.HP) return HP;
            else if (key == DataType.MaxHP) return MaxHP;
            else return 0;
        }
        set
        {
            if (key == DataType.HP) HP = (int)value;
            else if (key == DataType.MaxHP) MaxHP = (int)value;
        }
    }
    public BaseData GetCopy()
    {
        return (BaseData)MemberwiseClone();
    }
}


