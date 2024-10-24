using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//ゲームオーバーパネル管理スクリプト
public class GameOverPanel : MonoBehaviour
{
    // 終了ボタン
    [SerializeField] Button buttonDone;
    //ゲームマネージャー
    GameManager gameManager;

    // 初期化
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    //パネル表示
    public void DispPanel()
    {
        // 順番に表示
        Sequence seq = DOTween.Sequence();
        float dispTime = 1.5f;
        // このパネル
        Image panelGameOver = gameObject.GetComponent<Image>();

        Utils.SetAlpha(panelGameOver, 0);
        seq.Append(panelGameOver.DOFade(1, dispTime));

        // 閉じるボタンと子オブジェクト
        Utils.SetAlpha(buttonDone, 0);

        // 表示し終わったらリスナーを登録
        seq.Append(buttonDone.image.DOFade(1, dispTime)
            .OnComplete(() =>
            {
                buttonDone.onClick.AddListener(gameManager.LoadSceneTitle);
                // ボタンを選択状態にする
                buttonDone.Select();
            }));

        foreach (var item in buttonDone.GetComponentsInChildren<Graphic>())
        {
            seq.Join(item.DOFade(1, dispTime));
        }

        // 再生
        seq.Play().SetUpdate(true);

        // 全面に表示
        panelGameOver.transform.SetAsLastSibling();
        transform.SetAsLastSibling();

        // パネル表示
        panelGameOver.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
}
