using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//�x�[�X�A�r���e�B������X�N���v�g
public class BaseAbilityActivator : MonoBehaviour
{
    //������
    [SerializeField] GameObject AbilityPrefab;

    //�G�̃A�r���e�B�^�C�}�[
    [SerializeField] Text abilityCountText;

    //�A�r���e�B�f�[�^
    public AbilityActivatorData Data;

    //�ғ��^�C�}�[
    public float TotalTimer;

    //�����^�C�}�[
    protected float spawnTimer;

    //�v���p�e�B
    public float SpawnTimer { get { return spawnTimer; } set { spawnTimer = value; } }

    //���������A�r���e�B�̃��X�g
    protected List<BaseAbility> abilities;

    //�G�������u
    protected EnemySpawnerController enemySpawner;

    protected Transform parentTransform;

    //�u���b�N�^�C�v�̐����p�e�I�u�W�F�N�g
    protected GameObject blockParent;
    //�{�[���^�C�v�̐����p�e�I�u�W�F�N�g
    protected GameObject ballParent;




    public void Start()
    {
        spawnTimer = Data.SpawnTimerMax;

        parentTransform = transform.parent;
    }
    //������(�G�p)
    public void Init(EnemySpawnerController enemySpawner,AbilityActivatorData data)
    {
        //�ϐ�������
        abilities = new List<BaseAbility>();
        this.enemySpawner = enemySpawner;
        this.Data = data;
        if (Data.WhoUseAbility == 1)
        {
            Transform parentEnemyTransform = gameObject.transform.parent;
            Transform canvasTransform = parentEnemyTransform.Find("AbCountCanvas");
            abilityCountText = canvasTransform.GetChild(0).GetComponent<Text>();
        }
    }

    public void Init(AbilityActivatorData data)
    {
        //�ϐ�������
        abilities = new List<BaseAbility>();
        this.Data = data;

        if (Data.WhoUseAbility == 1)
        {
            Transform parentEnemyTransform = gameObject.transform.parent;
            Transform canvasTransform = parentEnemyTransform.Find("AbCountCanvas");
            abilityCountText = canvasTransform.GetChild(0).GetComponent<Text>();
        }
    }

    //�A�r���e�B����
    protected BaseAbility CreateAbility(Vector3 position,Transform parent = null)
    {
        //����
        GameObject obj = Instantiate(AbilityPrefab, position, AbilityPrefab.transform.rotation, parent);
        //���ʃf�[�^�Z�b�g
        BaseAbility ability = obj.GetComponent<BaseAbility>();
        //�f�[�^������
        ability.Init(this);
        Debug.Log(this);
        //�A�r���e�B�̃��X�g�֒ǉ�
        abilities.Add(ability);

        return ability;
    }

    //�^�C�}�[�����`�F�b�N
    protected bool IsSpawnTimerNotElapsed()
    {
        //�^�C�}�[����
        spawnTimer -= Time.deltaTime;
        if(Data.WhoUseAbility == 1)
        {
            abilityCountText.text = ((int)spawnTimer).ToString();
        }
        if (0 < spawnTimer) return true;

        return false;
    }
}
