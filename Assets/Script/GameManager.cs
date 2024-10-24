using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//�Q�[���}�l�[�W���[
public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // GameManager�I�u�W�F�N�g�����݂��Ȃ��ꍇ�A�V�����쐬����
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    //�J����
    [SerializeField] Camera cam;
    //�_���[�W�e�L�X�g�e
    [SerializeField] Transform textDamageParent;
    //�_���[�W���[�g�e�L�X�g�e
    [SerializeField] Transform textDamageRateParent;
    //�_���[�W�e�L�X�g�I�u�W�F�N�g
    [SerializeField] GameObject prefabTextDamage;
    //�_���[�W���[�g�e�L�X�g�I�u�W�F�N�g
    [SerializeField] GameObject prefabTextDamageRate;
    //�v���C���[
    public PlayerController player;
    //���I�_
    public float leftEnd;
    //�E�I�_
    public float rightEnd;
    //�v���C���[�f�[�^�t�@�C���p�X
    public string playerDataFilePath;
    //�v���C���[�f�[�^
    public string playerDataSave = "PlayerData.json";

    //�^�C�}�[�e�L�X�g
    [SerializeField] Text textTimer;
    //�X�R�A�e�L�X�g
    [SerializeField] Text textScore;
    //HP�X���C�_�[
    [SerializeField] Slider lifeSlider;
    //�o���l�X���C�_�[
    [SerializeField] Slider expSlider;
    //���x���e�L�X�g
    [SerializeField] Text lvText;
    //�`���[�W���[�^�[
    [SerializeField] ChargeMeterController chargeMeterController;
    //�|�[�Y�{�^��
    [SerializeField] Button poseButton;
    //�|�[�Y�p�l��
    [SerializeField] GameObject posePanel;
    //�Q�[���^�C�}�[
    public float gameTimer;
    //�O�̕b
    public float oldSeconds;
    //�|�[�Y���
    public bool isPosing;

    //�u���b�N�����p�X�|�i�[
    [SerializeField] BlockSpawnerController blockSpawner;
    //�G�l�~�[�����p�X�|�i�[
    [SerializeField] EnemySpawnerController enemySpawner;

    //�{�[���R���g���[���[
    [SerializeField] BallController ballController;

    //�Q�[���I�[�o�[
    [SerializeField] GameOverPanel gameOverPanel;

    //exp���Z�̑��x
    [SerializeField] private float expAddDuration = 2f;

    //�G�I�u�W�F�N�g�̐e
    public GameObject parentEnemy;

    //�X�R�A
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
        //�Œ�Ńv���C���[�쐬(�����L�����N�^�[�I����������Ȃ�v�ύX)
        int playerId = 100;
        player = PlayerSettings.Instance.CreatePlayer(playerId, this, ballController);
        player.Data.HP = player.Data.MaxHP;
    }
    void Start()
    {
        //�J�����̏����ݒ�
        float cameraSize = cam.orthographicSize;

        //��ʏc����(9:16)
        float aspect = (float)Screen.width/ (float)Screen.height;
        float cameraHeight = cameraSize * 2;
        float cameraWidth = cameraHeight * aspect;

        leftEnd = cam.transform.position.x - cameraWidth / 2;
        //�E��UI��ǉ������̂ŕύX
        //rightEnd = cam.transform.position.x + cameraWidth / 2;

        //�t�@�C���p�X�擾
        playerDataFilePath = Application.dataPath + "/" + playerDataSave;

        // �����ݒ�
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

        //�|�[�Y
        if (Input.GetKeyDown(KeyCode.Escape) && !isPosing)
        {
            OnPose();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPosing)
        {
            ClosePose();
        }
    }
    //�_���[�W�\��
    public void DispDamage(GameObject target, float damage)
    {
        GameObject obj = Instantiate(prefabTextDamage, textDamageParent);
        obj.GetComponent<TextDamageController>().Init(target, damage);
    }
    //�l���_���[�W���[�g�\��
    public void DispDamageRate(GameObject target, float damageRate)
    {
        GameObject obj = Instantiate(prefabTextDamageRate, textDamageParent);
        obj.GetComponent<TextDamageRateController>().Init(target, damageRate);
    }
    // �Q�[���^�C�}�[
    void updateGameTimer()
    {
        gameTimer += Time.deltaTime;

        // �O��ƕb���������Ȃ珈�������Ȃ�
        int seconds = (int)gameTimer % 60;
        if (seconds == oldSeconds) return;

        textTimer.text = Utils.GetTextTimer(gameTimer);
        oldSeconds = seconds;
    }
    // �Q�[���ĊJ/��~
    void setEnabled(bool enabled = true)
    {
        this.enabled = enabled;
        Time.timeScale = (enabled) ? 1 : 0;
        player.SetEnabled(enabled);
    }
    // �^�C�g����
    public void LoadSceneTitle()
    {
        DOTween.KillAll();
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("TitleScene");
    }
    // �Q�[���I�[�o�[�p�l����\��
    public void DispPanelGameOver()
    {
        StartCoroutine(SoundManager.Instance.StopAndSwitchSE(20));
        // �p�l���\��
        gameOverPanel.DispPanel();
        // �Q�[�����f
        setEnabled(false);
    }
    //�Q�[���X�R�A
    void updateGameScore()
    {
        textScore.text = "Score:" + score;
    }
    //���C�t
    void updateLife()
    {
        lifeSlider.value = player.Data.HP;
    }
    //�Q�[���I�[�o�[����
    void gameOver()
    {
        // �Q�[���I�[�o�[
        if (0 >= player.Data.HP)
        {
            ballController.ballSet = false;
            // ����ł��Ȃ��悤�ɂ���
            player.SetEnabled(false);

            EnemyClear();

            DispPanelGameOver();
        }
    }
    //�o���l�����ݒ�
    public void setUpExp()
    {
        expSlider.maxValue = player.Data.NeedExp;
        expSlider.value = player.Data.Exp;
    }
    //�o���l�ǉ�
    public void updateExp()
    {
        StartCoroutine(AddExp());
        //�L�����N�^�[�ǉ��̏ꍇ�͕ύX
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

        //�X���C�_�[����
        while (elapsed < expAddDuration)
        {
            elapsed += Time.deltaTime;
            expSlider.value = Mathf.Lerp(startValue, targetValue, elapsed / expAddDuration); // ���ݒl����ڕW�l�܂�
            yield return null;
        }

        // �X���C�_�[�̍ŏI�l���Z�b�g
        expSlider.value = targetValue;

        // ���x���A�b�v�̏���
        if (previousLv != player.Data.Lv)
        {
            lvText.text = "Lv" + previousLv + "�@���@Lv" + player.Data.Lv;
            SoundManager.Instance.PlaySE(23);

            // ���x���A�b�v���ɃX���C�_�[�����Z�b�g���A�V�����ő�l�Ɨ]�蕪�𔽉f
            expSlider.maxValue = player.Data.NeedExp;
            // �]�蕪���X���C�_�[�ɔ��f
            expSlider.value = player.Data.Exp;
        }
    }
    //���x���A�b�v
    public void LevelUp()
    {
        // �]�蕪�ɑΉ�
        while (player.Data.Exp >= player.Data.NeedExp) 
        {
            // �K�v�o���l�𒴂����]�蕪���v�Z
            int excessExp = player.Data.Exp - player.Data.NeedExp;

            // ���x���A�b�v�̏���
            player.Data.Lv++;
            player.Data.BonusPoint += 3;

            // �X���C�_�[�����Z�b�g
            expSlider.value = 0;

            // �K�v�o���l�̍X�V
            float needExp = (float)player.Data.NeedExp * 1.2f;
            player.Data.NeedExp = (int)needExp;

            // �o���l�����Z�b�g���A�]�蕪�����̃��x���Ɏ����z��
            player.Data.Exp = excessExp;

            // �X���C�_�[�ɗ]�蕪�𔽉f
            expSlider.maxValue = player.Data.NeedExp;
            expSlider.value = excessExp;

            // �f�[�^��ۑ�
            Utils.JsonSave(player.Data, playerDataFilePath);
        }
    }
    //�`���[�W���[�^�[���Z�b�g
    public void ResetCharge()
    {
        player.currentCharge = 0;
    }
    //�G�l�~�[��S�ď���
    public void EnemyClear()
    {
        foreach(Transform child in parentEnemy.transform)
        {
            Destroy(child.gameObject);
        }
    }
    //�|�[�Y���j���[
    public void OnPose()
    {
        isPosing = true;
        posePanel.SetActive(true);
        // �Q�[�������Ԃ��~�߂�
        Time.timeScale = 0f; 
    }
    public void ClosePose()
    {
        isPosing = false;
        posePanel.SetActive(false);
        // �Q�[�������Ԃ�ʏ�ɖ߂�
        Time.timeScale = 1f; 
    }
}
