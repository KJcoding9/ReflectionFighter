using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockSettings", menuName = "ScriptableObjects/BlockSettings")]
//ブロックセッティング
public class BlockSettings : ScriptableObject
{
    //ブロックデータ
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
    // リストのIDからデータを検索する
    public BlockData Get(int id)
    {
        return (BlockData)data.Find(blockData => blockData.Id == id).GetCopy();
    }

    // ブロック生成
    public BlockController CreateBlock(int id, GameManager gameManager, Vector3 position)
    {
        // ステータス取得
        BlockData data = Instance.Get(id);
        // オブジェクト
        GameObject obj = Instantiate(data.Prefab, position, Quaternion.identity);

        // データセット
        BlockController ctrl = obj.GetComponent<BlockController>();
        ctrl.Init(gameManager, data);

        return ctrl;
    }
}
//移動タイプ
public enum BlockType
{
    //固定
    Fixed,
    //移動
    Move,
}
//ブロックデータ
[Serializable]

public class BlockData : BaseData
{
    //プレハブ
    public GameObject Prefab;
    //タイプ
    public BlockType BlockType;
    //ブロック破壊した時のダメージレート上昇量
    public float DamageRateBonus;
    //スコア
    public int Score;
}