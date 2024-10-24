using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySettings", menuName = "ScriptableObjects/AbilitySettings")]
//�A�r���e�B�Z�b�e�B���O
public class AbilitySettings : ScriptableObject
{
    //�f�[�^
    public List<AbilityActivatorData> datas;

    static AbilitySettings instance;

    public static AbilitySettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<AbilitySettings>(nameof(AbilitySettings));
            }
            return instance;
        }
    }

    //���X�g��ID����f�[�^����������
    public AbilityActivatorData Get(int id)
    {
        return (AbilityActivatorData)datas.Find(data => data.Id == id).GetCopy();
    }

    //�A�r���e�B����(�G)
    public BaseAbilityActivator CreateAbilityActivator(int id, EnemySpawnerController enemySpawner, Transform parent = null)
    {
        //�f�[�^�擾
        AbilityActivatorData data = Instance.Get(id);
        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(data.ActivatePrefab, parent);
        //�f�[�^�Z�b�g
        BaseAbilityActivator activator = obj.GetComponent<BaseAbilityActivator>();
        activator.Init(enemySpawner, data);

        return activator;
    }

    public BaseAbilityActivator CreateAbilityActivator(int id,Transform parent = null)
    {
        //�f�[�^�擾
        AbilityActivatorData data = Instance.Get(id);
        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(data.ActivatePrefab, parent);
        //�f�[�^�Z�b�g
        BaseAbilityActivator activator = obj.GetComponent<BaseAbilityActivator>();

        activator.Init(data);

        return activator;
    }

}

public enum AbilityType
{
    Block,
    Falling
}
//�A�r���e�B�������u
[System.Serializable]
public class AbilityActivatorData : BaseData
{
    //�A�r���e�B�̃^�C�v
    public AbilityType type;
    //�������u�̃v���n�u
    public GameObject ActivatePrefab;
    //�A�r���e�B�̃A�C�R��
    public Sprite Icon;
    //��x�ɔ������鐔
    public float ActivateCount;
    //�A�r���e�B�̃X�R�A
    public int Score;
    //�����^�C�}�[
    public float SpawnTimerMin;
    public float SpawnTimerMax;
    //���ˑ��x
    public float ShotSpeed;
    //�A�r���e�B�̍U����
    public float Attack;
    //�A�r���e�B�v�Z���ŏ��l
    public float CalcMin;
    //�A�r���e�B�v�Z���ő�l
    public float CalcMax;
    //�A�r���e�B�����ʒu
    public Vector3 ActivatePosition;

    //�g�p�҂��v���C���[���G�� �v���C���[�̏ꍇ�@:�@�O�@�@�G�̏ꍇ�@�F�P�@
    public int WhoUseAbility;

    //TODO�@�A�r���e�B���g�p����L�����N�^�[���L�^���邩�͖���
    //public int UseAbilityChara;

    //�������Ԏ擾
    public float GetRandomSpawnTimer()
    {
        return Random.Range(SpawnTimerMin, SpawnTimerMax);
    }
}
