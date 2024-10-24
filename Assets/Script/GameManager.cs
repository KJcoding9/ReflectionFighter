using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//ゲームマネージャー
public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // GameManagerオブジェクトが存在しない場合、新しく作成する
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    //カメラ
    [SerializeField] Camera cam;
    //ダメージテキスト親
    [SerializeField] Transform textDamageParent;
    //ダメージレートテキスト親
    [SerializeField] Transform textDamageRateParent;
    //ダメージテキストオブジェクト
    [SerializeField] GameObject prefabTextDamage;
    //ダメージレートテキストオブジェクト
    [SerializeField] GameObject prefabTextDamageRate;
    //プレイヤー
    public PlayerController player;
    //左終点
    public float leftEnd;
    //右終点
    public float rightEnd;
    //プレイヤーデータファイルパス
    public string playerDataFilePath;
    //プレイヤーデータ
    public string playerDataSave = "PlayerData.json";

    //タイマーテキスト
    [SerializeField] Text textTimer;
    //スコアテキスト
    [SerializeField] Text textScore;
    //HPスライダー
    [SerializeField] Slider lifeSlider;
    //経験値スライダー
    [SerializeField] Slider expSlider;
    //レベルテキスト
    [SerializeField] Text lvText;
    //チャージメーター
    [SerializeField] ChargeMeterController chargeMeterController;
    //ポーズボタン
    [SerializeField] Button poseButton;
    //ポーズパネル
    [SerializeField] GameObject posePanel;
    //ゲームタイマー
    public float gameTimer;
    //前の秒
    public float oldSeconds;
    //ポーズ状態
    public bool isPosing;

    //ブロック生成用スポナー
    [SerializeField] BlockSpawnerController blockSpawner;
    //エネミー生成用スポナー
    [SerializeField] EnemySpawnerController enemySpawner;

    //ボールコントローラー
    [SerializeField] BallController ballController;

    //ゲームオーバー
    [SerializeField] GameOverPanel gameOverPanel;

    //exp加算の速度
    [SerializeField] private float expAddDuration = 2f;

    //敵オブジェクトの親
    public GameObject parentEnemy;

    //スコア
    public int score;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
        //固定でプレイヤー作成(もしキャラクター選択する方式なら要変更)
        int playerId = 100;
        player = PlayerSettings.Instance.CreatePlayer(playerId, this, ballController);
        player.Data.HP = player.Data.MaxHP;
    }
    void Start()
    {
        //カメラの初期設定
        float cameraSize = cam.orthographicSize;

        //画面縦横比(9:16)
        float aspect = (float)Screen.width/ (float)Screen.height;
        float cameraHeight = cameraSize * 2;
        float cameraWidth = cameraHeight * aspect;

        leftEnd = cam.transform.position.x - cameraWidth / 2;
        //右はUIを追加したので変更
        //rightEnd = cam.transform.position.x + cameraWidth / 2;

        //ファイルパス取得
        playerDataFilePath = Application.dataPath + "/" + playerDataSave;

        // 初期設定
        oldSeconds = -1;
        score = 0;
        lifeSlider.maxValue = player.Data.HP;
        lvText.text = "Lv" + player.Data.Lv.ToString();
        chargeMeterController.Init(player);
        enemySpawner.Init(this);
        blockSpawner.Init(this);
        ballController.Init(player.gameObject,player.transform.GetChild(0).transform);
        gameOverPanel.Init(this);

        setUpExp();
        setEnabled();

        SoundManager.Instance.PlayBGM(1);
    }
    void Update()
    {
        updateGameTimer();
        updateGameScore();
        updateLife();
        LevelUp();
        gameOver();

        //ポーズ
        if (Input.GetKeyDown(KeyCode.Escape) && !isPosing)
        {
            OnPose();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPosing)
        {
            ClosePose();
        }
    }
    //ダメージ表示
    public void DispDamage(GameObject target, float damage)
    {
        GameObject obj = Instantiate(prefabTextDamage, textDamageParent);
        obj.GetComponent<TextDamageController>().Init(target, damage);
    }
    //獲得ダメージレート表示
    public void DispDamageRate(GameObject target, float damageRate)
    {
        GameObject obj = Instantiate(prefabTextDamageRate, textDamageParent);
        obj.GetComponent<TextDamageRateController>().Init(target, damageRate);
    }
    // ゲームタイマー
    void updateGameTimer()
    {
        gameTimer += Time.deltaTime;

        // 前回と秒数が同じなら処理をしない
        int seconds = (int)gameTimer % 60;
        if (seconds == oldSeconds) return;

        textTimer.text = Utils.GetTextTimer(gameTimer);
        oldSeconds = seconds;
    }
    // ゲーム再開/停止
    void setEnabled(bool enabled = true)
    {
        this.enabled = enabled;
        Time.timeScale = (enabled) ? 1 : 0;
        player.SetEnabled(enabled);
    }
    // タイトルへ
    public void LoadSceneTitle()
    {
        DOTween.KillAll();
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("TitleScene");
    }
    // ゲームオーバーパネルを表示
    public void DispPanelGameOver()
    {
        StartCoroutine(SoundManager.Instance.StopAndSwitchSE(20));
        // パネル表示
        gameOverPanel.DispPanel();
        // ゲーム中断
        setEnabled(false);
    }
    //ゲームスコア
    void updateGameScore()
    {
        textScore.text = "Score:" + score;
    }
    //ライフ
    void updateLife()
    {
        lifeSlider.value = player.Data.HP;
    }
    //ゲームオーバー処理
    void gameOver()
    {
        // ゲームオーバー
        if (0 >= player.Data.HP)
        {
            ballController.ballSet = false;
            // 操作できないようにする
            player.SetEnabled(false);

            EnemyClear();

            DispPanelGameOver();
        }
    }
    //経験値初期設定
    public void setUpExp()
    {
        expSlider.maxValue = player.Data.NeedExp;
        expSlider.value = player.Data.Exp;
    }
    //経験値追加
    public void updateExp()
    {
        StartCoroutine(AddExp());
        //キャラクター追加の場合は変更
        Utils.JsonSave(player.Data, playerDataFilePath);
        WaveManager.Instance.backToMapButton.SetActive(true);
    }
    private IEnumerator AddExp()
    {
        int previousLv = player.Data.Lv;
        int startValue = (int)expSlider.value;
        int targetValue = startValue + StageManager.Instance.sumExp;

        float elapsed = 0f;
        player.Data.Exp += StageManager.Instance.sumExp;

        //スライダー処理
        while (elapsed < expAddDuration)
        {
            elapsed += Time.deltaTime;
            expSlider.value = Mathf.Lerp(startValue, targetValue, elapsed / expAddDuration); // 現在値から目標値まで
            yield return null;
        }

        // スライダーの最終値をセット
        expSlider.value = targetValue;

        // レベルアップの処理
        if (previousLv != player.Data.Lv)
        {
            lvText.text = "Lv" + previousLv + "　→　Lv" + player.Data.Lv;
            SoundManager.Instance.PlaySE(23);

            // レベルアップ時にスライダーをリセットし、新しい最大値と余剰分を反映
            expSlider.maxValue = player.Data.NeedExp;
            // 余剰分をスライダーに反映
            expSlider.value = player.Data.Exp;
        }
    }
    //レベルアップ
    public void LevelUp()
    {
        // 余剰分に対応
        while (player.Data.Exp >= player.Data.NeedExp) 
        {
            // 必要経験値を超えた余剰分を計算
            int excessExp = player.Data.Exp - player.Data.NeedExp;

            // レベルアップの処理
            player.Data.Lv++;
            player.Data.BonusPoint += 3;

            // スライダーをリセット
            expSlider.value = 0;

            // 必要経験値の更新
            float needExp = (float)player.Data.NeedExp * 1.2f;
            player.Data.NeedExp = (int)needExp;

            // 経験値をリセットし、余剰分を次のレベルに持ち越し
            player.Data.Exp = excessExp;

            // スライダーに余剰分を反映
            expSlider.maxValue = player.Data.NeedExp;
            expSlider.value = excessExp;

            // データを保存
            Utils.JsonSave(player.Data, playerDataFilePath);
        }
    }
    //チャージメーターリセット
    public void ResetCharge()
    {
        player.currentCharge = 0;
    }
    //エネミーを全て消す
    public void EnemyClear()
    {
        foreach(Transform child in parentEnemy.transform)
        {
            Destroy(child.gameObject);
        }
    }
    //ポーズメニュー
    public void OnPose()
    {
        isPosing = true;
        posePanel.SetActive(true);
        // ゲーム内時間を止める
        Time.timeScale = 0f; 
    }
    public void ClosePose()
    {
        isPosing = false;
        posePanel.SetActive(false);
        // ゲーム内時間を通常に戻す
        Time.timeScale = 1f; 
    }
}
