using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

//ステージマネージャー
public class StageManager : MonoBehaviour
{
    //ステージデータ
    [HideInInspector]public StageData stageData;
    //ウェーブテキスト
    public WaveText waveText;
    //ステージID
    public int stageId;
    //ステージで獲得した経験値
    public int sumExp; 
    private static StageManager _instance;

    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // シーン内に存在する StageManager を探す
                if (GameObject.Find("StageManager"))
                {
                    _instance = GameObject.Find("StageManager").GetComponent<StageManager>();
                }
            }
            return _instance;
        }
    }
    //ステージ読み込み
    void LoadStage(int id)
    {
        stageData = StageSettings.Instance.Get(id);
    }
    //ステージクリア
    public void ClearStage(Text clearText)
    {
        clearText.DOFade(1f, 1f).From(0f).SetEase(Ease.OutQuad);
    }
    //マップに戻る
    public void ReturnMap(string mapName)
    {
        Utils.GoScene(mapName);
    }
    //ステージIDセット
    public void SetStageId(int id)
    {
        stageId = id;
    }
    // シーンがロードされたときに実行するメソッドを登録
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // シーンロードイベントの解除
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // シーンがロードされたときに呼ばれるメソッド
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ここで他のオブジェクトの設定を行う
        _instance = GameObject.Find("StageManager").GetComponent<StageManager>();

        LoadStage(stageId);
        waveText = GameObject.Find("WaveText").GetComponent<WaveText>();
        waveText.Init();
    }
}
