using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//ウェーブテキスト管理スクリプト
public class WaveText : MonoBehaviour
{
    //テキスト
    private Text text;

    //初期化
    public void Init()
    {
        text = GetComponent<Text>();

        text.text = "Wave " + (WaveManager.Instance.CurrentWave + 1) + "/" + StageManager.Instance.stageData.WaveCount;
        text.text = "Wave " + WaveManager.Instance.CurrentWave+"/" + StageManager.Instance.stageData.WaveCount;
        gameObject.SetActive(true);
        ShowWaveText();
    }
    //テキスト表示
    public void ShowWaveText()
    {
        text.text = "Wave " + (WaveManager.Instance.CurrentWave + 1) + "/" + StageManager.Instance.stageData.WaveCount;
        text.DOFade(1f, 1f).From(0f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // フェードイン後にフェードアウトを開始する
            text.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // フェードアウト後の処理
                Debug.Log("Fade out complete!");
            });
        });
    }
}
