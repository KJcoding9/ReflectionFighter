using System.Runtime.CompilerServices;
using UnityEngine;

//�e�I�u�W�F�N�g�Ǘ��X�N���v�g
public class AbilityBallController : BaseAbility
{
    enum State
    {
        Alive,
        Dead
    }
    State state;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        Transform playerTransform = player.transform;
        if (playerTransform)
        {
            Shoot(playerTransform);
        }
    }

    //�e���ˏ���(�K�v�ɉ����ăX�|�i�[�̕��Ɉڍs)
    private void Shoot(Transform playerTransform)
    {
        SoundManager.Instance.PlaySE(16);
        //�v���C���[�̈ʒu�Ɍ������������v�Z
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        //����
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = direction * data.ShotSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int finalAttack = Random.Range((int)data.Attack-(int)data.CalcMin, (int)data.Attack+(int)data.CalcMax);
            collision.gameObject.GetComponent<PlayerController>().Damage(finalAttack);
            setDead();
        }
    }
    //���ŏ���
    public void setDead()
    {
        if (State.Alive != state) return;

        // �����������~
        rb2d.simulated = false;

        Destroy(gameObject);

        state = State.Dead;
    }
}
