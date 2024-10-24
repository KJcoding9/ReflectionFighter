using UnityEngine;
using UnityEngine.UI;

//ボス(プレハブ巨大化)のアビリティカウントテキストがずれるので専用で追加
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
    //初期化
    void Init()
    {
        mainCamera = Camera.main;
        abilityCountTextObj = gameObject.transform.GetChild(0).gameObject;
        enemy = gameObject.transform.parent.gameObject;
        abilityCountTextRect = abilityCountTextObj.transform.GetComponent<RectTransform>();
    }
    //テキスト位置指定
    void SetPos()
    {
        offset = new Vector3(0.5f, -0.5f, 0f);

        // スクリーン座標を取得（スケールを考慮）
        Vector3 adjustedOffset = enemy.transform.localScale.x * offset;
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, enemy.transform.position + adjustedOffset);

        // UI の位置を調整
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
