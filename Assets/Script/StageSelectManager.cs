using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Collections;

//�X�e�[�W�I���Ǘ��X�N���v�g
public class StageSelectManager : MonoBehaviour
{
    //�X�e�[�WID
    int stageSetId;
    //�f�[�^�p�X
    string stageFlagFilePath;
    string playerDataFilePath;
    //�f�[�^��
    string playerDataSave = "PlayerData.json";
    string stageFlagSave = "StageFlagData.json";
    //�X�e�[�W�I���{�^��
    [SerializeField] private Button[] _stageButton;
    //�X�e�[�W�t���O�f�[�^
    public StageFlagData stageFlagData;
    //�v���C���[�f�[�^
    public PlayerData playerData;

    [SerializeField] Text mapcutIn;

    private static StageSelectManager _instance;

    public static StageSelectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // �V�[�����ɑ��݂��� StageManager ��T��
                if (GameObject.Find("StageSelectManager"))
                {
                    _instance = GameObject.Find("StageSelectManager").GetComponent<StageSelectManager>();
                }
            }

            return _instance;
        }
    }

    //������
    private void Awake()
    {
        //�t�@�C���p�X�擾
        stageFlagFilePath = Application.dataPath + "/" + stageFlagSave;
        playerDataFilePath = Application.dataPath + "/" + playerDataSave;
        SoundManager.Instance.PlayBGM(0);
    }
    //�t�@�C���ǂݍ���
    void Start()
    {
        // �����t�@�C�����Ȃ�������t�@�C�����쐬����
        if (!File.Exists(playerDataFilePath))
        {
            //�L�����N�^�[�ǉ��̏ꍇ�͕ύX
            playerData = PlayerSettings.Instance.Get(100);
            Utils.JsonSave(playerData, playerDataFilePath);
            Debug.Log("StageDataJson�t�@�C�����Ȃ��̂ō쐬���܂����B");
        }
        else
        {
            //�L�����N�^�[�ǉ��̏ꍇ��(ID����v���Ă�����̂ɉ����ēǂݍ��ޕK�v������)
            playerData = Utils.JsonLoad<PlayerData>((playerDataFilePath));
            //�L�����N�^�[�������ǉ�����Ȃ�ID�̎����̂�ύX����K�v������
            Debug.Log("PlayerJson�t�@�C�������[�h���܂����B");
        }
        // �����t�@�C�����Ȃ�������t�@�C�����쐬����
        if (!File.Exists(stageFlagFilePath))
        {
            stageFlagData = StageSettings.Instance.GetFlag();
            Utils.JsonSave(stageFlagData, stageFlagFilePath);
            Debug.Log("StageDataJson�t�@�C�����Ȃ��̂ō쐬���܂����B");
        }
        else
        {
            stageFlagData = Utils.JsonLoad<StageFlagData>(stageFlagFilePath);
            Debug.Log("StageDataJson�t�@�C�������[�h���܂����B");
        }
    }
    void Update()
    {
        StageFlagCheck();
    }
    //�X�e�[�W�ǂݍ��݃{�^���p
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
    //�X�e�[�W�ݒ�
    public void OnSetStage(int id)
    {
        StageManager.Instance.SetStageId(id);
    }
    //�t���O�`�F�b�N
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
    //�ŏ��Ƀ}�b�v�ɓ������Ƃ��ɃJ�b�g�C��
    public void MapFadeCutIn()
    {

    }

}


