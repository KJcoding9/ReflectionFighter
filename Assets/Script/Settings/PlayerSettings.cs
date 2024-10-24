using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;


[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]

//プレイヤーセッティング
public class PlayerSettings : ScriptableObject
{
    string playerDataSave = "PlayerData.json";
    string playerDataFilePath;

    // キャラクターデータ
    public List<PlayerData> datas;

    public static PlayerSettings instance;

    public static PlayerSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<PlayerSettings>(nameof(PlayerSettings));
            }

            return instance;
        }
    }

    //リストのIDからデータを検索する
    public PlayerData Get(int id)
    {
        return (PlayerData)datas.Find(data => data.Id == id).GetCopy();
    }

    //プレイヤー生成
    public PlayerController CreatePlayer(int id, GameManager gameManager, BallController ballController)
    {

        playerDataFilePath = Application.dataPath + "/" + playerDataSave;
        PlayerData data;


        // もしファイルがあったらそこから読み込み(キャラ２人以上なら要変更)
        //キャラクター追加の場合は(IDが一致しているものに応じて読み込む必要がある)
        if (File.Exists(playerDataFilePath))
        {
            data = Utils.JsonLoad<PlayerData>((playerDataFilePath));
            Debug.Log("jsonから読み取り");
        }
        else
        {
            //ステータス取得
            data = Instance.Get(id);
            Debug.Log("settingsから読み取り");
        }

        Debug.Log("Prefab Path: " + data.prefabPath);
        Debug.Log("Prefab: " + data.Prefab);

        //プレハブをパスからロード
        data.LoadPrefab();
        //オブジェクト作成
        GameObject obj = Instantiate(data.Prefab, new Vector3(-3f, -5f, 0f), Quaternion.identity);
        //データセット
        PlayerController ctrl = obj.GetComponent<PlayerController>();
        Debug.Log(ctrl);
        ctrl.Init(gameManager, data, ballController);

        return ctrl;
    }
    //ステータス振り分け
    public void StatAllocation(string type,PlayerData data)
    {
        SoundManager.Instance.PlaySE(23);
        playerDataFilePath = Application.dataPath + "/" + playerDataSave;
        data.BonusPoint--;
        switch (type)
        {
            case "HP":
                data.MaxHP += 3;
                break;
            case "ATK":
                data.Attack++;
                break;
            case "DEF":
                data.Defense++;
                break;
            default:
                break;
        }
        Utils.JsonSave<PlayerData>(data, playerDataFilePath);
    }
}
//プレイヤーデータ
[Serializable]
public class PlayerData : CharacterData
{
    //攻撃判定有効時間
    public float AttackActivationTime;
    //チャージ時間
    public float ChargeRate;
    //チャージ可能量
    public float MaxCharge;
    //ボール速度減衰率
    public float DecayRate;
    //攻撃時のクールタイム
    public float AttackCoolTime;
    //レベル
    public int Lv;
    //経験値
    public int Exp;
    //必要経験値
    public int NeedExp;
    //ボーナスポイント
    public int BonusPoint;

    // プレハブのパスを保存
    public string prefabPath;

    // プレハブを動的にロードするメソッド
    public void LoadPrefab()
    {
        if (!string.IsNullOrEmpty(prefabPath))
        {
            Prefab = Resources.Load<GameObject>(prefabPath);
        }
    }
}
