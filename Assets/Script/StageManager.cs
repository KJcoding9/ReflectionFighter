using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

//�X�e�[�W�}�l�[�W���[
public class StageManager : MonoBehaviour
{
    //�X�e�[�W�f�[�^
    [HideInInspector]public StageData stageData;
    //�E�F�[�u�e�L�X�g
    public WaveText waveText;
    //�X�e�[�WID
    public int stageId;
    //�X�e�[�W�Ŋl�������o���l
    public int sumExp; 
    private static StageManager _instance;

    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // �V�[�����ɑ��݂��� StageManager ��T��
                if (GameObject.Find("StageManager"))
                {
                    _instance = GameObject.Find("StageManager").GetComponent<StageManager>();
                }
            }
            return _instance;
        }
    }
    //�X�e�[�W�ǂݍ���
    void LoadStage(int id)
    {
        stageData = StageSettings.Instance.Get(id);
    }
    //�X�e�[�W�N���A
    public void ClearStage(Text clearText)
    {
        clearText.DOFade(1f, 1f).From(0f).SetEase(Ease.OutQuad);
    }
    //�}�b�v�ɖ߂�
    public void ReturnMap(string mapName)
    {
        Utils.GoScene(mapName);
    }
    //�X�e�[�WID�Z�b�g
    public void SetStageId(int id)
    {
        stageId = id;
    }
    // �V�[�������[�h���ꂽ�Ƃ��Ɏ��s���郁�\�b�h��o�^
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // �V�[�����[�h�C�x���g�̉���
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // �V�[�������[�h���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �����ő��̃I�u�W�F�N�g�̐ݒ���s��
        _instance = GameObject.Find("StageManager").GetComponent<StageManager>();

        LoadStage(stageId);
        waveText = GameObject.Find("WaveText").GetComponent<WaveText>();
        waveText.Init();
    }
}
