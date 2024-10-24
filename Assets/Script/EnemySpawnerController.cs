using System;
using UnityEngine;
using System.Collections.Generic;

//スポーンタイプ(現在ランダムで使用する予定なし)
public enum SpawnType
{
    Random,
    Designate
}
//インスペクター用
[Serializable]
public class EnemySpawnData
{
    //説明用
    public string Title;

    //出現タイプ
    public SpawnType SpawnType;

    //生成座標(ランダムじゃない場合)
    public List<Vector3> SpawnPosition;

    //生成数
    public int SpawnCountMax;

    //生成する敵ID
    public List<int> EnemyIds;
}
[Serializable]
public class EnemySpawnDataCollection
{
    //スポーンデータ
    public List<EnemySpawnData> EnemySpawnDatas;
}
//エネミースポーナーを管理するスクリプト
public class EnemySpawnerController : MonoBehaviour
{
    //敵データ
   [SerializeField] List<EnemySpawnData> enemySpawnDatas;

    //生成した敵
    List<EnemyController> enemies;

    //ゲームマネージャー
    GameManager gameManager;

    //現在のデータ位置
    //int spawnDataIndex;

    //初期化
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;

        //生成した敵を保存
        enemies = new List<EnemyController>();
        //spawnDataIndex = -1;

        //指定なので0固定
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
