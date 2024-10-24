using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//ステージウェーブ管理スクリプト
public class WaveManager : MonoBehaviour
{
    //ステージマネージャー
    [SerializeField] StageManager stageManager;
    //ウェーブテキスト
    [SerializeField] WaveText waveText;
    //ブロック生成用親
    [SerializeField] GameObject blockParent;
    //背景
    [SerializeField] GameObject backGround;
    //ボールコントローラー
    [SerializeField] BallController ballController;
    //各スポーナー
    [SerializeField] BlockSpawnerController blockSpawnerController;
    [SerializeField] EnemySpawnerController enemySpawnerController;

    //演出
    [SerializeField] GameObject cutInPanel;
    [SerializeField] GameObject redBlinkPanel;
    //ダメージテキスト生成用親
    [SerializeField] GameObject parentDamageText;
    //各テキスト
    [SerializeField] Text bossText;
    [SerializeField] Text damageRateText;
    [SerializeField] Text clearText;
    //exp
    [SerializeField] GameObject expUI;
    //戻るボタン
    public GameObject backToMapButton;

    //ステージフラグデータ
    [SerializeField] StageFlagData stageFlagData;

    //現在のウェーブ
    private int currentWave = 0;
    //ダメージレート
    public float damageRate = 1;
    //プロパティ
    public int CurrentWave
    {
        get { return currentWave; }
    }
    //倒した敵
    private int defeatedEnemy = 0;

    //ステージフラグデータパス
    string stageFlagFilePath;
    string stageFlagSave = "StageFlagData.json";

    private static WaveManager _instance;

    public static WaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("WaveManager").AddComponent<WaveManager>();
            }
            return _instance;
        }
    }
    //初期化(必要に応じてゲームマネージャーに初期化を呼び出してもらう)
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        stageFlagFilePath = Application.dataPath + "/" + stageFlagSave;
    }

    void Start()
    {
        stageFlagData = Utils.JsonLoad<StageFlagData>(stageFlagFilePath);
        StartWave();
    }
    void Update()
    {

        damageRateText.text = Utils.ConvertToPercentage(damageRate);

        
        //ウェーブごとの移行条件確認
        if (stageManager.stageData.EnemiesPerWave != null &&
            stageManager.stageData.EnemiesPerWave.Count > currentWave)
        {
            
            if (defeatedEnemy >= stageManager.stageData.EnemiesPerWave[currentWave] &&
                stageManager.stageData.EnemiesPerWave.Count <= stageManager.stageData.WaveCount)
            {
                // 一旦ボールは止める
                ballController.Rb2d.velocity = Vector2.zero;
                Utils.SetAlpha(ballController.gameObject.GetComponent<SpriteRenderer>().material, 0f);
                ballController.ballSet = true;

                // PanelTextDamageに子要素が何もないことを確認してから次のウェーブへ
                if (parentDamageText.transform.childCount == 0)
                {
                    NextWave();
                }
            }
        }
    }
    //初期化
    public void StartWave()
    {
        currentWave = 0;
        damageRate = 1;
        if (currentWave + 1 == stageManager.stageData.WaveCount &&
            stageManager.stageData.IsBoss)
        {
            //ボス演出
            AdvanceBossWaveEffect();
            StartCoroutine(SoundManager.Instance.SwitchBGM(2));
        }
    }
    //ウェーブクリア時
    public void NextWave()
    {
        //ウェーブ更新
        currentWave++;

        //一回残りのブロックを消す
        for (int i =0; i<blockParent.transform.childCount;i++)
        {
            if (blockParent.transform != null)
            {
                Destroy(blockParent.transform.GetChild(i).gameObject);
            }
        }
        GameManager.Instance.ResetCharge();
        Time.timeScale = 0f;
        //ステージ移行アニメーション処理
        AdvanceWaveEffect();

        //条件分岐→まだWaveが残ってるなら次のWave、終わりならクリア
        if (currentWave >= stageManager.stageData.WaveCount)
        {
            ballController.ballSet = false;
            GameManager.Instance.player.SetEnabled(false);

            stageManager.ClearStage(clearText);
            if (!stageManager.stageData.IsExtra)
            {
                if (stageFlagData.IsClear[stageManager.stageId] == false)
                {
                    stageFlagData.stageUnlock++;
                }
            }
            if (!stageManager.stageData.IsExtra)
            {
                stageFlagData.IsClear[stageManager.stageId] = true;
            }
            string fileName = "StageFlagData.json";
            string filePath = Application.dataPath + "/" + fileName;
            GameManager.Instance.player.Data.HP = GameManager.Instance.player.Data.MaxHP;
            Utils.JsonSave(stageFlagData,filePath);

            SoundManager.Instance.StopBGM();
            expUI.SetActive(true);
            SoundManager.Instance.PlaySE(8);
            GameManager.Instance.updateExp();
        }
        else
        {
            if(currentWave + 1 == stageManager.stageData.WaveCount &&
                stageManager.stageData.IsBoss)
            {
                //ボス演出
                AdvanceBossWaveEffect();
            }
            blockSpawnerController.SpecifiedSpawn(currentWave);
            enemySpawnerController.DesignationSpawn(currentWave);
            waveText.ShowWaveText();
        }
        defeatedEnemy = 0;
        //ステージ移行アニメーション処理
        Time.timeScale = 1f;
    }
    //敵を倒した
    public void DestroyEnemy()
    {
        defeatedEnemy++;
    }
    //ウェーブが進む演出(必要に応じてエフェクトマネージャーに統合)
    public void AdvanceWaveEffect()
    {
        SoundManager.Instance.PlaySE(12);
        backGround.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 2f)
            .OnComplete(() => backGround.transform.localScale = new Vector3(2.1f, 2.1f, 1f));
        SpriteRenderer bgSpriteRenderer = backGround.GetComponent<SpriteRenderer>();
        bgSpriteRenderer.DOFade(0, 2f)
            .OnComplete(() => bgSpriteRenderer.DOFade(1, 0f));
    }
    //ウェーブが進む演出BOSS(必要に応じてエフェクトマネージャーに統合)
    public void AdvanceBossWaveEffect()
    {
        SoundManager.Instance.PlaySE(11);
        SoundManager.Instance.StopBGM();
        Image redBlinkPanelSpriteRederer = redBlinkPanel.GetComponent<Image>();
        Image cutInPanelSpriteRenderer = cutInPanel.GetComponent<Image>();

        Utils.FlashScreen(redBlinkPanelSpriteRederer,0.6f, 1.0f, 3);

        cutInPanelSpriteRenderer.DOFade(0.7f, 1f)
        .OnComplete(() =>
        {
            bossText.DOFade(1f, 1f)
                .OnComplete(() =>
                {
                    bossText.DOFade(0f, 0.5f)
                        .OnComplete(() => cutInPanelSpriteRenderer.DOFade(0f, 0.5f));
                });
        });
        SoundManager.Instance.PlayBGM(2);
    }
}
