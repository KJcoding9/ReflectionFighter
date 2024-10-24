using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//�X�e�[�W�E�F�[�u�Ǘ��X�N���v�g
public class WaveManager : MonoBehaviour
{
    //�X�e�[�W�}�l�[�W���[
    [SerializeField] StageManager stageManager;
    //�E�F�[�u�e�L�X�g
    [SerializeField] WaveText waveText;
    //�u���b�N�����p�e
    [SerializeField] GameObject blockParent;
    //�w�i
    [SerializeField] GameObject backGround;
    //�{�[���R���g���[���[
    [SerializeField] BallController ballController;
    //�e�X�|�[�i�[
    [SerializeField] BlockSpawnerController blockSpawnerController;
    [SerializeField] EnemySpawnerController enemySpawnerController;

    //���o
    [SerializeField] GameObject cutInPanel;
    [SerializeField] GameObject redBlinkPanel;
    //�_���[�W�e�L�X�g�����p�e
    [SerializeField] GameObject parentDamageText;
    //�e�e�L�X�g
    [SerializeField] Text bossText;
    [SerializeField] Text damageRateText;
    [SerializeField] Text clearText;
    //exp
    [SerializeField] GameObject expUI;
    //�߂�{�^��
    public GameObject backToMapButton;

    //�X�e�[�W�t���O�f�[�^
    [SerializeField] StageFlagData stageFlagData;

    //���݂̃E�F�[�u
    private int currentWave = 0;
    //�_���[�W���[�g
    public float damageRate = 1;
    //�v���p�e�B
    public int CurrentWave
    {
        get { return currentWave; }
    }
    //�|�����G
    private int defeatedEnemy = 0;

    //�X�e�[�W�t���O�f�[�^�p�X
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
    //������(�K�v�ɉ����ăQ�[���}�l�[�W���[�ɏ��������Ăяo���Ă��炤)
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

        
        //�E�F�[�u���Ƃ̈ڍs�����m�F
        if (stageManager.stageData.EnemiesPerWave != null &&
            stageManager.stageData.EnemiesPerWave.Count > currentWave)
        {
            
            if (defeatedEnemy >= stageManager.stageData.EnemiesPerWave[currentWave] &&
                stageManager.stageData.EnemiesPerWave.Count <= stageManager.stageData.WaveCount)
            {
                // ��U�{�[���͎~�߂�
                ballController.Rb2d.velocity = Vector2.zero;
                Utils.SetAlpha(ballController.gameObject.GetComponent<SpriteRenderer>().material, 0f);
                ballController.ballSet = true;

                // PanelTextDamage�Ɏq�v�f�������Ȃ����Ƃ��m�F���Ă��玟�̃E�F�[�u��
                if (parentDamageText.transform.childCount == 0)
                {
                    NextWave();
                }
            }
        }
    }
    //������
    public void StartWave()
    {
        currentWave = 0;
        damageRate = 1;
        if (currentWave + 1 == stageManager.stageData.WaveCount &&
            stageManager.stageData.IsBoss)
        {
            //�{�X���o
            AdvanceBossWaveEffect();
            StartCoroutine(SoundManager.Instance.SwitchBGM(2));
        }
    }
    //�E�F�[�u�N���A��
    public void NextWave()
    {
        //�E�F�[�u�X�V
        currentWave++;

        //���c��̃u���b�N������
        for (int i =0; i<blockParent.transform.childCount;i++)
        {
            if (blockParent.transform != null)
            {
                Destroy(blockParent.transform.GetChild(i).gameObject);
            }
        }
        GameManager.Instance.ResetCharge();
        Time.timeScale = 0f;
        //�X�e�[�W�ڍs�A�j���[�V��������
        AdvanceWaveEffect();

        //�������򁨂܂�Wave���c���Ă�Ȃ玟��Wave�A�I���Ȃ�N���A
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
                //�{�X���o
                AdvanceBossWaveEffect();
            }
            blockSpawnerController.SpecifiedSpawn(currentWave);
            enemySpawnerController.DesignationSpawn(currentWave);
            waveText.ShowWaveText();
        }
        defeatedEnemy = 0;
        //�X�e�[�W�ڍs�A�j���[�V��������
        Time.timeScale = 1f;
    }
    //�G��|����
    public void DestroyEnemy()
    {
        defeatedEnemy++;
    }
    //�E�F�[�u���i�މ��o(�K�v�ɉ����ăG�t�F�N�g�}�l�[�W���[�ɓ���)
    public void AdvanceWaveEffect()
    {
        SoundManager.Instance.PlaySE(12);
        backGround.transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 2f)
            .OnComplete(() => backGround.transform.localScale = new Vector3(2.1f, 2.1f, 1f));
        SpriteRenderer bgSpriteRenderer = backGround.GetComponent<SpriteRenderer>();
        bgSpriteRenderer.DOFade(0, 2f)
            .OnComplete(() => bgSpriteRenderer.DOFade(1, 0f));
    }
    //�E�F�[�u���i�މ��oBOSS(�K�v�ɉ����ăG�t�F�N�g�}�l�[�W���[�ɓ���)
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
