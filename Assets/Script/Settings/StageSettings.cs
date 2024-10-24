using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "StageSettings", menuName = "ScriptableObjects/StageSettings")]
//�X�e�[�W�Z�b�e�B���O
public class StageSettings : ScriptableObject
{
    //�X�e�[�W�f�[�^
    public List<StageData> datas;
    //�X�e�[�W�t���O�f�[�^
    public StageFlagData flagDatas;

    public static StageSettings instance;
    public static StageSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<StageSettings>(nameof(StageSettings));
            }

            return instance;
        }
    }

    //���X�g��ID����f�[�^����������
    public StageData Get(int id)
    {
        return datas.Find(data => data.Id == id).GetCopy();
    }
    //�t���O�f�[�^�擾
    public StageFlagData GetFlag()
    {
        return flagDatas.GetCopy();
    }
}
//�X�e�[�W�f�[�^
[Serializable]
public class StageData
{
    //�^�C�g��
    public string Title;
    //�f�[�^ID
    public int Id;
    // ������
    [TextArea] public string Description;
    //�X�e�[�W�ɍő傢���̃E�F�[�u��ݒ肷�邩
    public int WaveCount;
    public List<int> EnemiesPerWave;
    //�{�X�����邩
    public bool IsBoss;
    //�G�N�X�g���X�e�[�W
    public bool IsExtra;

    public StageData GetCopy()
    {
        return (StageData)MemberwiseClone();
    }
}

//�X�e�[�W�t���O�f�[�^
[Serializable]
public class StageFlagData
{
    public int stageUnlock;
    public List<bool> IsClear;

    public StageFlagData GetCopy()
    {
        return (StageFlagData)MemberwiseClone();
    }
}