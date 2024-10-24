using System;
using UnityEngine;
using System.Collections.Generic;

//�X�|�[���^�C�v(���݃����_���Ŏg�p����\��Ȃ�)
public enum SpawnType
{
    Random,
    Designate
}
//�C���X�y�N�^�[�p
[Serializable]
public class EnemySpawnData
{
    //�����p
    public string Title;

    //�o���^�C�v
    public SpawnType SpawnType;

    //�������W(�����_������Ȃ��ꍇ)
    public List<Vector3> SpawnPosition;

    //������
    public int SpawnCountMax;

    //��������GID
    public List<int> EnemyIds;
}
[Serializable]
public class EnemySpawnDataCollection
{
    //�X�|�[���f�[�^
    public List<EnemySpawnData> EnemySpawnDatas;
}
//�G�l�~�[�X�|�[�i�[���Ǘ�����X�N���v�g
public class EnemySpawnerController : MonoBehaviour
{
    //�G�f�[�^
   [SerializeField] List<EnemySpawnData> enemySpawnDatas;

    //���������G
    List<EnemyController> enemies;

    //�Q�[���}�l�[�W���[
    GameManager gameManager;

    //���݂̃f�[�^�ʒu
    //int spawnDataIndex;

    //������
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;

        //���������G��ۑ�
        enemies = new List<EnemyController>();
        //spawnDataIndex = -1;

        //�w��Ȃ̂�0�Œ�
        if (SpawnType.Designate == enemySpawnDatas[0].SpawnType)
        {
            DesignationSpawn(0);
        }
    }

    public void DesignationSpawn(int listNum)
    {
        for(int i=0; i < enemySpawnDatas[listNum].SpawnCountMax; i++)
        {
            int id = enemySpawnDatas[listNum].EnemyIds[i];
            EnemyController enemy = CharacterSettings.Instance.CreateEnemy(id, gameManager, enemySpawnDatas[listNum].SpawnPosition[i]);
            if (gameManager.parentEnemy != null)
            {
                enemy.transform.SetParent(gameManager.parentEnemy.transform);
            }
            enemies.Add(enemy);
        }
    }
}
