using UnityEngine;
using UnityEngine.UI;

//�X�e�[�^�X�p�l���Ǘ��X�N���v�g
public class StatusPanel : MonoBehaviour
{
    //�e�e�L�X�g
    [SerializeField] Text lvText;
    [SerializeField] Text hpText;
    [SerializeField] Text atkText;
    [SerializeField] Text defText;
    [SerializeField] Text bonusPointText;
    //�e�{�^��
    [SerializeField] Button hpButton;
    [SerializeField] Button atkButton;
    [SerializeField] Button defButton;
    //EXP�X���C�_�[
    [SerializeField] Slider expSlider;

    //�X�e�[�W�Z���N�g�}�l�[�W���[
    [SerializeField] StageSelectManager stageSelectManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetExpSlider();
    }

    // Update is called once per frame
    void Update()
    {
        //�����L�����ǉ�����܂ł����Ȃ�ύX
        StatusUpdate();
    }

    //�X�e�[�^�X�X�V
    public void StatusUpdate()
    {
        lvText.text = stageSelectManager.playerData.Lv.ToString().PadLeft(5);
        hpText.text = stageSelectManager.playerData.MaxHP.ToString().PadLeft(5);
        atkText.text = stageSelectManager.playerData.Attack.ToString().PadLeft(5);
        defText.text = stageSelectManager.playerData.Defense.ToString().PadLeft(5);
        bonusPointText.text = stageSelectManager.playerData.BonusPoint.ToString();
        if(stageSelectManager.playerData.BonusPoint > 0)
        {
            hpButton.gameObject.SetActive(true);
            atkButton.gameObject.SetActive(true);
            defButton.gameObject.SetActive(true);
        }
        else
        {
            hpButton.gameObject.SetActive(false);
            atkButton.gameObject.SetActive(false);
            defButton.gameObject.SetActive(false);
        }
    }
    //�X�e�[�^�X�U�蕪��
    public void StatusUp(string status)
    {
        PlayerSettings.Instance.StatAllocation(status,StageSelectManager.Instance.playerData);
    }
    //EXP�Z�b�g
    public void SetExpSlider()
    {
        expSlider.maxValue = stageSelectManager.playerData.NeedExp;
        expSlider.value = stageSelectManager.playerData.Exp;
    }
}
