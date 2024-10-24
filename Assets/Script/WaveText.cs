using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//�E�F�[�u�e�L�X�g�Ǘ��X�N���v�g
public class WaveText : MonoBehaviour
{
    //�e�L�X�g
    private Text text;

    //������
    public void Init()
    {
        text = GetComponent<Text>();

        text.text = "Wave " + (WaveManager.Instance.CurrentWave + 1) + "/" + StageManager.Instance.stageData.WaveCount;
        text.text = "Wave " + WaveManager.Instance.CurrentWave+"/" + StageManager.Instance.stageData.WaveCount;
        gameObject.SetActive(true);
        ShowWaveText();
    }
    //�e�L�X�g�\��
    public void ShowWaveText()
    {
        text.text = "Wave " + (WaveManager.Instance.CurrentWave + 1) + "/" + StageManager.Instance.stageData.WaveCount;
        text.DOFade(1f, 1f).From(0f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // �t�F�[�h�C����Ƀt�F�[�h�A�E�g���J�n����
            text.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // �t�F�[�h�A�E�g��̏���
                Debug.Log("Fade out complete!");
            });
        });
    }
}
