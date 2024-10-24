using System.Runtime.CompilerServices;
using UnityEngine;

//弾オブジェクト管理スクリプト
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

    //弾発射処理(必要に応じてスポナーの方に移行)
    private void Shoot(Transform playerTransform)
    {
        SoundManager.Instance.PlaySE(16);
        //プレイヤーの位置に向かう方向を計算
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        //発射
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
