using UnityEngine;
using UnityEngine.UI;

//�G��HP�o�[���Ǘ�����X�N���v�g
public class EnemyHPBar : MonoBehaviour
{
    //HP�o�[
    [SerializeField] GameObject hpBar;
    //�G
    [SerializeField] GameObject enemy;
    //�ʒu�I�t�Z�b�g
    public Vector3 offset;
    //�J����
    private Camera mainCamera;
    //HP�o�[���N�g�g�����X�t�H�[��
    private RectTransform hpBarRectTransform;
    //�X���C�_�[
    private Slider hpValue;

    // Update is called once per frame
    void Update()
    {
        SetPos();
    }
    //������
    public void Init()
    {
        mainCamera = Camera.main;
        hpBar = gameObject.transform.GetChild(0).gameObject;
        enemy = gameObject.transform.parent.gameObject;
        hpBarRectTransform = hpBar.transform.GetComponent<RectTransform>();
        hpValue = hpBar.GetComponent<Slider>();
    }
    //�ʒu�w��
    void SetPos()
    {
        offset = new Vector3(0f, 0.5f, 0f);
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemy.transform.position + offset);
        hpBarRectTransform.position = screenPosition;
    }
    public void UpDateMaxHPValue(int value)
    {
        hpValue.maxValue = value;
    }
    public void UpdateHPBarValue(int value)
    {
        hpValue.value = value;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
