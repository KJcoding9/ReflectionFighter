using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//�Q�[���I�[�o�[�p�l���Ǘ��X�N���v�g
public class GameOverPanel : MonoBehaviour
{
    // �I���{�^��
    [SerializeField] Button buttonDone;
    //�Q�[���}�l�[�W���[
    GameManager gameManager;

    // ������
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    //�p�l���\��
    public void DispPanel()
    {
        // ���Ԃɕ\��
        Sequence seq = DOTween.Sequence();
        float dispTime = 1.5f;
        // ���̃p�l��
        Image panelGameOver = gameObject.GetComponent<Image>();

        Utils.SetAlpha(panelGameOver, 0);
        seq.Append(panelGameOver.DOFade(1, dispTime));

        // ����{�^���Ǝq�I�u�W�F�N�g
        Utils.SetAlpha(buttonDone, 0);

        // �\�����I������烊�X�i�[��o�^
        seq.Append(buttonDone.image.DOFade(1, dispTime)
            .OnComplete(() =>
            {
                buttonDone.onClick.AddListener(gameManager.LoadSceneTitle);
                // �{�^����I����Ԃɂ���
                buttonDone.Select();
            }));

        foreach (var item in buttonDone.GetComponentsInChildren<Graphic>())
        {
            seq.Join(item.DOFade(1, dispTime));
        }

        // �Đ�
        seq.Play().SetUpdate(true);

        // �S�ʂɕ\��
        panelGameOver.transform.SetAsLastSibling();
        transform.SetAsLastSibling();

        // �p�l���\��
        panelGameOver.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
}
