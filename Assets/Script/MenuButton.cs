using UnityEngine;

//���j���[�Ǘ��X�N���v�g
public class MenuButton : MonoBehaviour
{
    //�e�p�l��
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject StatusPanel;
    [SerializeField] GameObject GuidePanel;
    [SerializeField] GameObject ManualPanel;

    //�e�y�[�W��
    public void ShowMenu()
    {
        SoundManager.Instance.PlaySE(21);
        MenuPanel.SetActive(true);
    }
    public void HideMenu()
    {
        SoundManager.Instance.PlaySE(22);
        MenuPanel.SetActive(false);
    }
    public void ShowStatusMenu()
    {
        SoundManager.Instance.PlaySE(21);
        StatusPanel.SetActive(true);
    }
    public void HideStatusMenu()
    {
        SoundManager.Instance.PlaySE(22);
        StatusPanel.SetActive(false);
    }
    public void ShowGuideMenu()
    {
        SoundManager.Instance.PlaySE(21);
        GuidePanel.SetActive(true);
    }
    public void HideGuideMenu()
    {
        SoundManager.Instance.PlaySE(22);
        GuidePanel.SetActive(false);
    }

    public void SHowManualMenu()
    {
        SoundManager.Instance.PlaySE(21);
        ManualPanel.SetActive(true);
    }
    public void HideManualMenu()
    {
        SoundManager.Instance.PlaySE(22);
        ManualPanel.SetActive(false);
    }
}
