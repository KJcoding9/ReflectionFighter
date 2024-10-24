using UnityEngine;

//マミーアタックオブジェクト管理スクリプト
public class AbilityMummyAttackController : BaseAbility
{
    enum State
    {
        Alive,
        Dead
    }
    State state;

    //衝突時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int finalAttack = Random.Range((int)data.Attack - (int)data.CalcMin, (int)data.Attack + (int)data.CalcMax);
            collision.gameObject.GetComponent<PlayerController>().Damage(finalAttack);
            setDead();
        }
    }
    //消滅処理
    public void setDead()
    {
        if (State.Alive != state) return;

        // 物理挙動を停止
        rb2d.simulated = false;

        Destroy(gameObject);

        state = State.Dead;
    }
}
