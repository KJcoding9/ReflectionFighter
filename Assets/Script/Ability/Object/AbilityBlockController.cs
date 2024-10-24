using UnityEngine;

//�A�r���e�B�u���b�N�I�u�W�F�N�g�Ǘ��X�N���v�g
public class AbilityBlockController : BaseAbility
{
    enum State
    {
        Alive,
        Dead
    }
    State state;

    private void Start()
    {
        SoundManager.Instance.PlaySE(19);
        VFXManager.Instance.PlayEffect(this.gameObject.transform, 3);
    }
    //�_���[�W����
    public float Damage(float attack)
    {

        // ��A�N�e�B�u
        if (State.Alive != state) return 0;

        data.HP -= (int)attack;

        // ����
        if (0 > data.HP)
        {
            GameManager.Instance.score += data.Score;
            VFXManager.Instance.SpawnAndPlayEffect(this.gameObject.transform, 1);
            setDead();
        }
        return attack;
    }
    //���ŏ���
    void setDead()
    {
        if (State.Alive != state) return;

        // �����������~
        rb2d.simulated = false;

        SoundManager.Instance.PlaySE(3);
        //�G�t�F�N�g���Ăяo����Ȃ��̂ŏ����x��������
        Destroy(gameObject);

        state = State.Dead;
    }
}
