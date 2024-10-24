using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "StageSettings", menuName = "ScriptableObjects/StageSettings")]
//ステージセッティング
public class StageSettings : ScriptableObject
{
    //ステージデータ
    public List<StageData> datas;
    //ステージフラグデータ
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

    //リストのIDからデータを検索する
    public StageData Get(int id)
    {
        return datas.Find(data => data.Id == id).GetCopy();
    }
    //フラグデータ取得
    public StageFlagData GetFlag()
    {
        return flagDatas.GetCopy();
    }
}
//ステージデータ
[Serializable]
public class StageData
{
    //タイトル
    public string Title;
    //データID
    public int Id;
    // 説明文
    [TextArea] public string Description;
    //ステージに最大いくつのウェーブを設定するか
    public int WaveCount;
    public List<int> EnemiesPerWave;
    //ボスがいるか
    public bool IsBoss;
    //エクストラステージ
    public bool IsExtra;

    public StageData GetCopy()
    {
        return (StageData)MemberwiseClone();
    }
}

//ステージフラグデータ
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