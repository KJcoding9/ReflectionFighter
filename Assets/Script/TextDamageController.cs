using UnityEngine;
using DG.Tweening;
using TMPro;

//ダメージテキスト管理スクリプト
public class TextDamageController : MonoBehaviour
{
    //消す時間
    float destroyTime = 1;
    GameObject target;

    void Start()
    {
        //膨らんで小さくなって消える
        transform.DOScale(new Vector2(1, 1), destroyTime / 2)
            .SetRelative().
            OnComplete(() =>
            {
                transform.DOScale(new Vector2(0, 0), destroyTime / 2)
                .OnComplete(() => Destroy(gameObject));
            });
    }
    //初期化
    public void Init(GameObject target, float damage)
    {
        this.target = target;
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = "" + (int)damage;

        //プレイヤーのダメージは赤表示
        if (target.GetComponent<PlayerController>())
        {
            text.color = Color.red;
        }

        if (!target) return;
        //位置決定(中心)
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
        transform.position = pos;
    }
}
