using UnityEngine;
using UnityEngine.UI;

//�`���[�W���[�^�[���Ǘ�����X�N���v�g
public class ChargeMeterController : MonoBehaviour
{
    //�`���[�W���[�^�[�X���C�_�[
    Slider slider;
    //�C���[�W
    Image fillImage;
    //�O�̐F
    Color previoursColor;
    //�v���C���[�R���g���[���[
    [SerializeField] PlayerController playerController;


    void Update()
    {
        slider.value = playerController.currentCharge;

        ChangeSliderColor();
    }

    //������
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

    //�`���[�W���[�^�[MAX�̎��ɐF��ς���
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
