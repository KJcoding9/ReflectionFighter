using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//ベースアビリティ生成器スクリプト
public class BaseAbilityActivator : MonoBehaviour
{
    //発動物
    [SerializeField] GameObject AbilityPrefab;

    //敵のアビリティタイマー
    [SerializeField] Text abilityCountText;

    //アビリティデータ
    public AbilityActivatorData Data;

    //稼働タイマー
    public float TotalTimer;

    //発動タイマー
    protected float spawnTimer;

    //プロパティ
    public float SpawnTimer { get { return spawnTimer; } set { spawnTimer = value; } }

    //発動したアビリティのリスト
    protected List<BaseAbility> abilities;

    //敵生成装置
    protected EnemySpawnerController enemySpawner;

    protected Transform parentTransform;

    //ブロックタイプの生成用親オブジェクト
    protected GameObject blockParent;
    //ボールタイプの生成用親オブジェクト
    protected GameObject ballParent;




    public void Start()
    {
        spawnTimer = Data.SpawnTimerMax;

        parentTransform = transform.parent;
    }
    //初期化(敵用)
    public void Init(EnemySpawnerController enemySpawner,AbilityActivatorData data)
    {
        //変数初期化
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
        //変数初期化
        abilities = new List<BaseAbility>();
        this.Data = data;

        if (Data.WhoUseAbility == 1)
        {
            Transform parentEnemyTransform = gameObject.transform.parent;
            Transform canvasTransform = parentEnemyTransform.Find("AbCountCanvas");
            abilityCountText = canvasTransform.GetChild(0).GetComponent<Text>();
        }
    }

    //アビリティ発動
    protected BaseAbility CreateAbility(Vector3 position,Transform parent = null)
    {
        //生成
        GameObject obj = Instantiate(AbilityPrefab, position, AbilityPrefab.transform.rotation, parent);
        //共通データセット
        BaseAbility ability = obj.GetComponent<BaseAbility>();
        //データ初期化
        ability.Init(this);
        Debug.Log(this);
        //アビリティのリストへ追加
        abilities.Add(ability);

        return ability;
    }

    //タイマー消化チェック
    protected bool IsSpawnTimerNotElapsed()
    {
        //タイマー消化
        spawnTimer -= Time.deltaTime;
        if(Data.WhoUseAbility == 1)
        {
            abilityCountText.text = ((int)spawnTimer).ToString();
        }
        if (0 < spawnTimer) return true;

        return false;
    }
}
