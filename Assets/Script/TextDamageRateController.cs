using DG.Tweening;
using TMPro;
using UnityEngine;

//�_���[�W���[�g�e�L�X�g�Ǘ��X�N���v�g
public class TextDamageRateController : MonoBehaviour
{
    float destroyTime = 1;
    GameObject target;

    void Start()
    {
        //�c���ŏ�����
        transform.DOScale(new Vector2(1, 1), destroyTime / 2)
            .SetRelative().
            OnComplete(() => Destroy(gameObject));
    }

    //������
    public void Init(GameObject target, float damageRate)
    {
        this.target = target;
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = "" + (int)damageRate+"%UP";

        if (!target) return;
        //�ʒu����i���S�j
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
        transform.position = pos;
    }
}
