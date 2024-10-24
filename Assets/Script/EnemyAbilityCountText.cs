using UnityEngine;
using UnityEngine.UI;

//エネミーのアビリティカウントテキストを表示するスクリプト
public class EnemyAbilityCountText : MonoBehaviour
{
    //テキストオブジェクト
    [SerializeField] GameObject abilityCountTextObj;
    //敵
    [SerializeField] GameObject enemy;
    //表示オフセット
    public Vector3 offset;
    //カメラ
    private Camera mainCamera;
    //テキストのレクト
    private RectTransform abilityCountTextRect;
    //アビリティテキスト
    private Text abilityCountText;

    void Start()
    {
        Init();
    }
    void Update()
    {
        SetPos();
    }
    //初期化
    void Init()
    {
        mainCamera = Camera.main;
        abilityCountTextObj = gameObject.transform.GetChild(0).gameObject;
        enemy = gameObject.transform.parent.gameObject;
        abilityCountTextRect = abilityCountTextObj.transform.GetComponent<RectTransform>();
    }
    //位置指定
    void SetPos()
    {
        offset = new Vector3(0.5f, -0.5f, 0f);

        // スクリーン座標を取得
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, enemy.transform.position + offset);

        // UI 要素の位置をスクリーン座標に設定
        abilityCountTextRect.position = screenPosition;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }

    //値更新用
    public void UpdateAbilityText(string s)
    {
        abilityCountText.text = s;
    }
}
