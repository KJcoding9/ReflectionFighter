using UnityEngine;
using UnityEngine.UI;

//チャージメーターを管理するスクリプト
public class ChargeMeterController : MonoBehaviour
{
    //チャージメータースライダー
    Slider slider;
    //イメージ
    Image fillImage;
    //前の色
    Color previoursColor;
    //プレイヤーコントローラー
    [SerializeField] PlayerController playerController;


    void Update()
    {
        slider.value = playerController.currentCharge;

        ChangeSliderColor();
    }

    //初期化
    public void Init(PlayerController player)
    {
        slider = gameObject.GetComponent<Slider>();

        playerController = player;

        slider.maxValue = playerController.Data.MaxCharge;

        if (fillImage == null)
        {
            fillImage = slider.fillRect.GetComponent<Image>();
        }
        previoursColor = fillImage.color;
    }

    //チャージメーターMAXの時に色を変える
    public void ChangeSliderColor()
    {
        if(fillImage != null)
        {
            if (slider.value == slider.maxValue)
            {
                fillImage.color = new Color(1f, 0.647f, 0f);
            }
            else
            {
                fillImage.color = previoursColor;
            }
        }
    }
}
