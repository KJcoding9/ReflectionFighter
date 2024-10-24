using UnityEngine;

//�x�[�X�A�r���e�B�X�N���v�g
public class BaseAbility : MonoBehaviour
{
    //�e�������u
    protected BaseAbilityActivator activator;
    //�A�r���e�B�f�[�^
    protected AbilityActivatorData data;
    //��������
    protected Rigidbody2D rb2d;

    //������
    public void Init(BaseAbilityActivator activator)
    {
        //�e�������u
        this.activator = activator;
        //�A�r���e�B�f�[�^�Z�b�g
        this.data = (AbilityActivatorData)activator.Data.GetCopy();
        //��������
        this.rb2d = GetComponent<Rigidbody2D>();
    }
}
