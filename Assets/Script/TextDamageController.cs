using UnityEngine;
using DG.Tweening;
using TMPro;

//�_���[�W�e�L�X�g�Ǘ��X�N���v�g
public class TextDamageController : MonoBehaviour
{
    //��������
    float destroyTime = 1;
    GameObject target;

    void Start()
    {
        //�c���ŏ������Ȃ��ď�����
        transform.DOScale(new Vector2(1, 1), destroyTime / 2)
            .SetRelative().
            OnComplete(() =>
            {
                transform.DOScale(new Vector2(0, 0), destroyTime / 2)
                .OnComplete(() => Destroy(gameObject));
            });
    }
    //������
    public void Init(GameObject target, float damage)
    {
        this.target = target;
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = "" + (int)damage;

        //�v���C���[�̃_���[�W�͐ԕ\��
        if (target.GetComponent<PlayerController>())
        {
            text.color = Color.red;
        }

        if (!target) return;
        //�ʒu����(���S)
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
        transform.position = pos;
    }
}
