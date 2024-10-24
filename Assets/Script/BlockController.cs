using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

//�u���b�N�Ǘ��X�N���v�g
public class BlockController : MonoBehaviour
{
    //�u���b�N�f�[�^
    public BlockData Data;
    //�Q�[���}�l�[�W��
    [SerializeField] GameManager gameManager;
    //rigidbody
    Rigidbody2D rb2d;

    //���
    enum State
    {
        Alive,
        Dead
    }
    State state;

    //������
    public void Init(GameManager gameManager,BlockData blockData)
    {
        this.gameManager = gameManager;
        this.Data = blockData;

        rb2d = GetComponent<Rigidbody2D>();

        state = State.Alive;
    }
    //�_���[�W����
    public float Damage(float attack)
    {
        // ��A�N�e�B�u
        if (State.Alive != state) return 0;

        //float damage = (attack);
        Data.HP -= (int)attack;

        // ����
        if (0 > Data.HP)
        {
            SoundManager.Instance.PlaySE(10);
            //TODO Utills�N���X�ɂ��X�R�A�v�Z�ɕύX�\��(score*�R���{��)
            gameManager.score += Data.Score;
            VFXManager.Instance.SpawnAndPlayEffect(this.gameObject.transform, 1);
            setDead();
        }

        // �v�Z��̃_���[�W��Ԃ�(��)
        return attack;
    }

    //�{�[�����ŏ���
    void setDead()
    {
        if (State.Alive != state) return;

        // �����������~
        rb2d.simulated = false;

        SoundManager.Instance.PlaySE(3);

        //�_���[�W���[�g�{�[�i�X���Z
        WaveManager.Instance.damageRate += Data.DamageRateBonus;
        // �_���[�W���[�g�\��
        gameManager.DispDamageRate(gameObject, (Data.DamageRateBonus*100));
        //����
        Destroy(gameObject);

        state = State.Dead;
    }
}
