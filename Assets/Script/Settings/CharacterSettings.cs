using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableObjects/CharacterSettings")]
//�L�����N�^�[�Z�b�e�B���O
public class CharacterSettings : ScriptableObject
{
    // �L�����N�^�[�f�[�^
    public List<CharacterData> datas;

    public static CharacterSettings instance;

    public static CharacterSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<CharacterSettings>(nameof(CharacterSettings));
            }

            return instance;
        }
    }

    //���X�g��ID����f�[�^����������
    public CharacterData Get(int id)
    {
        return (CharacterData)datas.Find(data => data.Id == id).GetCopy();
    }

    //�G����
    public EnemyController CreateEnemy(int id, GameManager gameManager, Vector3 position)
    {
        //�X�e�[�^�X�擾
        CharacterData data = Instance.Get(id);
        //�I�u�W�F�N�g
        GameObject obj = Instantiate(data.Prefab, position, Quaternion.identity);

        //�f�[�^�Z�b�g
        EnemyController ctrl = obj.GetComponent<EnemyController>();
        ctrl.Init(gameManager, data);

        return ctrl;
    }
}
//�G�^�C�v
public enum Type
{
    Stay,
    Move
}
//�L�����N�^�[�f�[�^
[Serializable]
public class CharacterData : BaseData
{
    //�^�C�v
    public Type EnemyType;
    //�v���n�u
    public GameObject Prefab;

    //�A�r���e�BID
    public List<int> DefaultAbilityIds;
    //�g�p�\�A�r���e�BID
    public List<int> UsableAbilityIds;
    //�U����
    public float Attack;
    //�h���
    public float Defense;
    //�ړ��Ԋu
    public float MoveInterval;
    //�ړ�����
    public float MoveDistance;
    //�X�R�A
    public int Score;

    //�����|���ꂽ�ꍇ�ɓ���ł���o���l
    public int DropExp;
    //�A�r���e�B��������
    public float AbilityTime;
    //�h���b�v�A�C�e��
    public GameObject DropItem;
}