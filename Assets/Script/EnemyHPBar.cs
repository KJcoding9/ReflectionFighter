using UnityEngine;
using UnityEngine.UI;

//敵のHPバーを管理するスクリプト
public class EnemyHPBar : MonoBehaviour
{
    //HPバー
    [SerializeField] GameObject hpBar;
    //敵
    [SerializeField] GameObject enemy;
    //位置オフセット
    public Vector3 offset;
    //カメラ
    private Camera mainCamera;
    //HPバーレクトトランスフォーム
    private RectTransform hpBarRectTransform;
    //スライダー
    private Slider hpValue;

    // Update is called once per frame
    void Update()
    {
        SetPos();
    }
    //初期化
    public void Init()
    {
        mainCamera = Camera.main;
        hpBar = gameObject.transform.GetChild(0).gameObject;
        enemy = gameObject.transform.parent.gameObject;
        hpBarRectTransform = hpBar.transform.GetComponent<RectTransform>();
        hpValue = hpBar.GetComponent<Slider>();
    }
    //位置指定
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
