using UnityEngine;

//アビリティブロックオブジェクト管理スクリプト
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
    //ダメージ処理
    public float Damage(float attack)
    {

        // 非アクティブ
        if (State.Alive != state) return 0;

        data.HP -= (int)attack;

        // 消滅
        if (0 > data.HP)
        {
            GameManager.Instance.score += data.Score;
            VFXManager.Instance.SpawnAndPlayEffect(this.gameObject.transform, 1);
            setDead();
        }
        return attack;
    }
    //消滅処理
    void setDead()
    {
        if (State.Alive != state) return;

        // 物理挙動を停止
        rb2d.simulated = false;

        SoundManager.Instance.PlaySE(3);
        //エフェクトが呼び出されないので少し遅延を入れる
        Destroy(gameObject);

        state = State.Dead;
    }
}
