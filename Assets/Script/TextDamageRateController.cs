using DG.Tweening;
using TMPro;
using UnityEngine;

//ダメージレートテキスト管理スクリプト
public class TextDamageRateController : MonoBehaviour
{
    float destroyTime = 1;
    GameObject target;

    void Start()
    {
        //膨らんで消える
        transform.DOScale(new Vector2(1, 1), destroyTime / 2)
            .SetRelative().
            OnComplete(() => Destroy(gameObject));
    }

    //初期化
    public void Init(GameObject target, float damageRate)
    {
        this.target = target;
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = "" + (int)damageRate+"%UP";

        if (!target) return;
        //位置決定（中心）
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
        transform.position = pos;
    }
}
