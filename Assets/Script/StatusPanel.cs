using UnityEngine;
using UnityEngine.UI;

//ステータスパネル管理スクリプト
public class StatusPanel : MonoBehaviour
{
    //各テキスト
    [SerializeField] Text lvText;
    [SerializeField] Text hpText;
    [SerializeField] Text atkText;
    [SerializeField] Text defText;
    [SerializeField] Text bonusPointText;
    //各ボタン
    [SerializeField] Button hpButton;
    [SerializeField] Button atkButton;
    [SerializeField] Button defButton;
    //EXPスライダー
    [SerializeField] Slider expSlider;

    //ステージセレクトマネージャー
    [SerializeField] StageSelectManager stageSelectManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetExpSlider();
    }

    // Update is called once per frame
    void Update()
    {
        //もしキャラ追加するまでいくなら変更
        StatusUpdate();
    }

    //ステータス更新
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
    //ステータス振り分け
    public void StatusUp(string status)
    {
        PlayerSettings.Instance.StatAllocation(status,StageSelectManager.Instance.playerData);
    }
    //EXPセット
    public void SetExpSlider()
    {
        expSlider.maxValue = stageSelectManager.playerData.NeedExp;
        expSlider.value = stageSelectManager.playerData.Exp;
    }
}
