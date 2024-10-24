using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

//ブロック管理スクリプト
public class BlockController : MonoBehaviour
{
    //ブロックデータ
    public BlockData Data;
    //ゲームマネージャ
    [SerializeField] GameManager gameManager;
    //rigidbody
    Rigidbody2D rb2d;

    //状態
    enum State
    {
        Alive,
        Dead
    }
    State state;

    //初期化
    public void Init(GameManager gameManager,BlockData blockData)
    {
        this.gameManager = gameManager;
        this.Data = blockData;

        rb2d = GetComponent<Rigidbody2D>();

        state = State.Alive;
    }
    //ダメージ処理
    public float Damage(float attack)
    {
        // 非アクティブ
        if (State.Alive != state) return 0;

        //float damage = (attack);
        Data.HP -= (int)attack;

        // 消滅
        if (0 > Data.HP)
        {
            SoundManager.Instance.PlaySE(10);
            //TODO Utillsクラスによるスコア計算に変更予定(score*コンボ数)
            gameManager.score += Data.Score;
            VFXManager.Instance.SpawnAndPlayEffect(this.gameObject.transform, 1);
            setDead();
        }

        // 計算後のダメージを返す(仮)
        return attack;
    }

    //ボール消滅処理
    void setDead()
    {
        if (State.Alive != state) return;

        // 物理挙動を停止
        rb2d.simulated = false;

        SoundManager.Instance.PlaySE(3);

        //ダメージレートボーナス加算
        WaveManager.Instance.damageRate += Data.DamageRateBonus;
        // ダメージレート表示
        gameManager.DispDamageRate(gameObject, (Data.DamageRateBonus*100));
        //消滅
        Destroy(gameObject);

        state = State.Dead;
    }
}
