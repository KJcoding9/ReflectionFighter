using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilitySettings", menuName = "ScriptableObjects/AbilitySettings")]
//アビリティセッティング
public class AbilitySettings : ScriptableObject
{
    //データ
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

    //リストのIDからデータを検索する
    public AbilityActivatorData Get(int id)
    {
        return (AbilityActivatorData)datas.Find(data => data.Id == id).GetCopy();
    }

    //アビリティ生成(敵)
    public BaseAbilityActivator CreateAbilityActivator(int id, EnemySpawnerController enemySpawner, Transform parent = null)
    {
        //データ取得
        AbilityActivatorData data = Instance.Get(id);
        //オブジェクト作成
        GameObject obj = Instantiate(data.ActivatePrefab, parent);
        //データセット
        BaseAbilityActivator activator = obj.GetComponent<BaseAbilityActivator>();
        activator.Init(enemySpawner, data);

        return activator;
    }

    public BaseAbilityActivator CreateAbilityActivator(int id,Transform parent = null)
    {
        //データ取得
        AbilityActivatorData data = Instance.Get(id);
        //オブジェクト作成
        GameObject obj = Instantiate(data.ActivatePrefab, parent);
        //データセット
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
//アビリティ発動装置
[System.Serializable]
public class AbilityActivatorData : BaseData
{
    //アビリティのタイプ
    public AbilityType type;
    //発動装置のプレハブ
    public GameObject ActivatePrefab;
    //アビリティのアイコン
    public Sprite Icon;
    //一度に発動する数
    public float ActivateCount;
    //アビリティのスコア
    public int Score;
    //発動タイマー
    public float SpawnTimerMin;
    public float SpawnTimerMax;
    //発射速度
    public float ShotSpeed;
    //アビリティの攻撃力
    public float Attack;
    //アビリティ計算式最小値
    public float CalcMin;
    //アビリティ計算式最大値
    public float CalcMax;
    //アビリティ生成位置
    public Vector3 ActivatePosition;

    //使用者がプレイヤーか敵か プレイヤーの場合　:　０　　敵の場合　：１　
    public int WhoUseAbility;

    //TODO　アビリティを使用するキャラクターを記録するかは未定
    //public int UseAbilityChara;

    //生成時間取得
    public float GetRandomSpawnTimer()
    {
        return Random.Range(SpawnTimerMin, SpawnTimerMax);
    }
}
