using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableObjects/CharacterSettings")]
//キャラクターセッティング
public class CharacterSettings : ScriptableObject
{
    // キャラクターデータ
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

    //リストのIDからデータを検索する
    public CharacterData Get(int id)
    {
        return (CharacterData)datas.Find(data => data.Id == id).GetCopy();
    }

    //敵生成
    public EnemyController CreateEnemy(int id, GameManager gameManager, Vector3 position)
    {
        //ステータス取得
        CharacterData data = Instance.Get(id);
        //オブジェクト
        GameObject obj = Instantiate(data.Prefab, position, Quaternion.identity);

        //データセット
        EnemyController ctrl = obj.GetComponent<EnemyController>();
        ctrl.Init(gameManager, data);

        return ctrl;
    }
}
//敵タイプ
public enum Type
{
    Stay,
    Move
}
//キャラクターデータ
[Serializable]
public class CharacterData : BaseData
{
    //タイプ
    public Type EnemyType;
    //プレハブ
    public GameObject Prefab;

    //アビリティID
    public List<int> DefaultAbilityIds;
    //使用可能アビリティID
    public List<int> UsableAbilityIds;
    //攻撃力
    public float Attack;
    //防御力
    public float Defense;
    //移動間隔
    public float MoveInterval;
    //移動距離
    public float MoveDistance;
    //スコア
    public int Score;

    //もし倒された場合に入手できる経験値
    public int DropExp;
    //アビリティ発動時間
    public float AbilityTime;
    //ドロップアイテム
    public GameObject DropItem;
}