using UnityEngine;


public enum DataType
{
    HP,
    MaxHP,
}
//�x�[�X�f�[�^
public class BaseData
{
    //�^�C�g��
    public string Title;
    //�f�[�^ID
    public int Id;
    // ������
    [TextArea] public string Description;
    //�̗�
    public int HP;
    //�̗͍ő�
    public int MaxHP;

    //�C���f�N�T�[
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


