using UnityEngine;
using UnityEngine.UI;

//�{�X(�v���n�u���剻)�̃A�r���e�B�J�E���g�e�L�X�g�������̂Ő�p�Œǉ�
public class BossEnemyAbilityCountText : MonoBehaviour
{
    [SerializeField] GameObject abilityCountTextObj;
    [SerializeField] GameObject enemy;

    public Vector3 offset;
    private Camera mainCamera;
    private RectTransform abilityCountTextRect;
    private Text abilityCountText;

    void Start()
    {
        Init();
    }

    void Update()
    {
        SetPos();
    }
    //������
    void Init()
    {
        mainCamera = Camera.main;
        abilityCountTextObj = gameObject.transform.GetChild(0).gameObject;
        enemy = gameObject.transform.parent.gameObject;
        abilityCountTextRect = abilityCountTextObj.transform.GetComponent<RectTransform>();
    }
    //�e�L�X�g�ʒu�w��
    void SetPos()
    {
        offset = new Vector3(0.5f, -0.5f, 0f);

        // �X�N���[�����W���擾�i�X�P�[�����l���j
        Vector3 adjustedOffset = enemy.transform.localScale.x * offset;
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, enemy.transform.position + adjustedOffset);

        // UI �̈ʒu�𒲐�
        abilityCountTextRect.position = screenPosition;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }

    public void UpdateAbilityText(string s)
    {
        abilityCountText.text = s; 
    }
}
