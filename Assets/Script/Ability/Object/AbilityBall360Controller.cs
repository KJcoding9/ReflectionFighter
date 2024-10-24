using UnityEngine;

//360�x�e�I�u�W�F�N�g�Ǘ��X�N���v�g
public class AbilityBall360Controller : BaseAbility
{
    enum State
    {
        Alive,
        Dead
    }
    State state;

    //�Փˎ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int finalAttack = Random.Range((int)data.Attack - (int)data.CalcMin, (int)data.Attack + (int)data.CalcMax);
            collision.gameObject.GetComponent<PlayerController>().Damage(finalAttack);
            setDead();
        }
    }
    //����
    public void setDead()
    {
        if (State.Alive != state) return;

        // �����������~
        rb2d.simulated = false;

        Destroy(gameObject);

        state = State.Dead;
    }
}
