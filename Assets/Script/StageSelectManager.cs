using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Collections;

//ステージ選択管理スクリプト
public class StageSelectManager : MonoBehaviour
{
    //ステージID
    int stageSetId;
    //データパス
    string stageFlagFilePath;
    string playerDataFilePath;
    //データ名
    string playerDataSave = "PlayerData.json";
    string stageFlagSave = "StageFlagData.json";
    //ステージ選択ボタン
    [SerializeField] private Button[] _stageButton;
    //ステージフラグデータ
    public StageFlagData stageFlagData;
    //プレイヤーデータ
    public PlayerData playerData;

    [SerializeField] Text mapcutIn;

    private static StageSelectManager _instance;

    public static StageSelectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // シーン内に存在する StageManager を探す
                if (GameObject.Find("StageSelectManager"))
                {
                    _instance = GameObject.Find("StageSelectManager").GetComponent<StageSelectManager>();
                }
            }

            return _instance;
        }
    }

    //初期化
    private void Awake()
    {
        //ファイルパス取得
        stageFlagFilePath = Application.dataPath + "/" + stageFlagSave;
        playerDataFilePath = Application.dataPath + "/" + playerDataSave;
        SoundManager.Instance.PlayBGM(0);
    }
    //ファイル読み込み
    void Start()
    {
        // もしファイルがなかったらファイルを作成する
        if (!File.Exists(playerDataFilePath))
        {
            //キャラクター追加の場合は変更
            playerData = PlayerSettings.Instance.Get(100);
            Utils.JsonSave(playerData, playerDataFilePath);
            Debug.Log("StageDataJsonファイルがないので作成しました。");
        }
        else
        {
            //キャラクター追加の場合は(IDが一致しているものに応じて読み込む必要がある)
            playerData = Utils.JsonLoad<PlayerData>((playerDataFilePath));
            //キャラクターをもし追加するならIDの式自体を変更する必要がある
            Debug.Log("PlayerJsonファイルをロードしました。");
        }
        // もしファイルがなかったらファイルを作成する
        if (!File.Exists(stageFlagFilePath))
        {
            stageFlagData = StageSettings.Instance.GetFlag();
            Utils.JsonSave(stageFlagData, stageFlagFilePath);
            Debug.Log("StageDataJsonファイルがないので作成しました。");
        }
        else
        {
            stageFlagData = Utils.JsonLoad<StageFlagData>(stageFlagFilePath);
            Debug.Log("StageDataJsonファイルをロードしました。");
        }
    }
    void Update()
    {
        StageFlagCheck();
    }
    //ステージ読み込みボタン用
    public void StageSelect(int stage)
    {
        SceneManager.LoadScene("Stage1-" + stage);
    }
    public void LoadNextScene(string sceneName)
    {
        SoundManager.Instance.PlaySE(15);
        StartCoroutine(LoadSceneAsync("Stage1-" + sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        OnSetStage(stageSetId);
    }

    public void OnSetStageId(int id)
    {
        stageSetId = id;
    }
    //ステージ設定
    public void OnSetStage(int id)
    {
        StageManager.Instance.SetStageId(id);
    }
    //フラグチェック
    public void StageFlagCheck()
    {
        _stageButton[0].gameObject.SetActive(true);

        for (int i = 1; i <= stageFlagData.IsClear.Count-1; i++)
        {
            if (stageFlagData.IsClear[i-1] == true)
            {
                _stageButton[i].gameObject.SetActive(true);
            }
            else
            {
                _stageButton[i].gameObject.SetActive(false);
            }
        }
    }
    //最初にマップに入ったときにカットイン
    public void MapFadeCutIn()
    {

    }

}


