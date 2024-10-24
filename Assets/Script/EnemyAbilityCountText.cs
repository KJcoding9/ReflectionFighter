using UnityEngine;
using UnityEngine.UI;

//�G�l�~�[�̃A�r���e�B�J�E���g�e�L�X�g��\������X�N���v�g
public class EnemyAbilityCountText : MonoBehaviour
{
    //�e�L�X�g�I�u�W�F�N�g
    [SerializeField] GameObject abilityCountTextObj;
    //�G
    [SerializeField] GameObject enemy;
    //�\���I�t�Z�b�g
    public Vector3 offset;
    //�J����
    private Camera mainCamera;
    //�e�L�X�g�̃��N�g
    private RectTransform abilityCountTextRect;
    //�A�r���e�B�e�L�X�g
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
    //�ʒu�w��
    void SetPos()
    {
        offset = new Vector3(0.5f, -0.5f, 0f);

        // �X�N���[�����W���擾
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, enemy.transform.position + offset);

        // UI �v�f�̈ʒu���X�N���[�����W�ɐݒ�
        abilityCountTextRect.position = screenPosition;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }

    //�l�X�V�p
    public void UpdateAbilityText(string s)
    {
        abilityCountText.text = s;
    }
}
