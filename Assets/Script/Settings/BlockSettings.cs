using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockSettings", menuName = "ScriptableObjects/BlockSettings")]
//�u���b�N�Z�b�e�B���O
public class BlockSettings : ScriptableObject
{
    //�u���b�N�f�[�^
    public List<BlockData> data;

    static BlockSettings instance;

    public static BlockSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<BlockSettings>(nameof(BlockSettings));
            }

            return instance;
        }
    }
    // ���X�g��ID����f�[�^����������
    public BlockData Get(int id)
    {
        return (BlockData)data.Find(blockData => blockData.Id == id).GetCopy();
    }

    // �u���b�N����
    public BlockController CreateBlock(int id, GameManager gameManager, Vector3 position)
    {
        // �X�e�[�^�X�擾
        BlockData data = Instance.Get(id);
        // �I�u�W�F�N�g
        GameObject obj = Instantiate(data.Prefab, position, Quaternion.identity);

        // �f�[�^�Z�b�g
        BlockController ctrl = obj.GetComponent<BlockController>();
        ctrl.Init(gameManager, data);

        return ctrl;
    }
}
//�ړ��^�C�v
public enum BlockType
{
    //�Œ�
    Fixed,
    //�ړ�
    Move,
}
//�u���b�N�f�[�^
[Serializable]

public class BlockData : BaseData
{
    //�v���n�u
    public GameObject Prefab;
    //�^�C�v
    public BlockType BlockType;
    //�u���b�N�j�󂵂����̃_���[�W���[�g�㏸��
    public float DamageRateBonus;
    //�X�R�A
    public int Score;
}