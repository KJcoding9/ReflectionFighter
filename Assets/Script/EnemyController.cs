using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵を管理するスクリプト(必要になったときにボス専用のものも作る)
public class EnemyController : MonoBehaviour
{
    //敵データ
    public CharacterData Data;
    //ゲームマネージャー
    [SerializeField] GameManager gameManager;
    //敵HPバー
    [SerializeField] EnemyHPBar enemyHPBar;
    //rigidbody
    Rigidbody2D rb2d;
    //必要がある場合は、キャラクターデータに移行してそこから直接データ読み取る方式に変更
    public float mooveSpeed = 0.5f;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    //移動カウント
    private float moveCount;
    // 衝突チェックの半径
    public float checkRadius = 0.5f;
    //アニメーター
    private Animator anim;
    //移動フラグ
    private bool isMoving = false;

    //所持しているアビリティ
    public List<BaseAbilityActivator> abilityActivators;

    // 状態
    enum State
    {
        Alive,
        Dead
    }
    State state;
    //移動状態
    enum MooveState
    {
        Idle,
        Mooving
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Data.EnemyType == Type.Move)
        {
            MoveEnemyCount();
        }
    }
    //初期化
    public void Init(GameManager gameManager, CharacterData charaData)
    {
        abilityActivators = new List<BaseAbilityActivator>();

        enemyHPBar = gameObject.transform.GetChild(0).GetComponent<EnemyHPBar>();
        
        this.gameManager = gameManager;
        Data = charaData;

        rb2d = GetComponent<Rigidbody2D>();

        enemyHPBar.Init();
        enemyHPBar.UpDateMaxHPValue((int)Data.MaxHP);
        enemyHPBar.UpdateHPBarValue((int)Data.HP);

        state = State.Alive;

        //アビリティデータセット
        foreach(var item in Data.DefaultAbilityIds)
        {
            AddAbilityActivator(item);
        }
    }
    //ダメージ処理
    public float Damage(float attack,float speed)
    {
        // 非アクティブ
        if (State.Alive != state) return 0;

        //speedが一定以上でダメージボーナス
        float damage;

        if (speed >= 7)
        {
            damage = (Random.Range(1, (int)attack)) * WaveManager.Instance.damageRate * (1+(speed/10));
        }
        else
        {
            damage = (Random.Range(1, (int)attack)) * WaveManager.Instance.damageRate;
        }

        SoundManager.Instance.PlaySE(9);
        Data.HP -= (int)damage;

        if(enemyHPBar != null)
        {
            enemyHPBar.UpdateHPBarValue((int)Data.HP);
        }

        // ダメージ表示
        gameManager.DispDamage(gameObject, damage);
        // 消滅
        if (0 >= Data.HP)
        {
            gameManager.score += Data.Score;
            VFXManager.Instance.SpawnAndPlayEffect(this.gameObject.transform, 1);
            SetDead();
        }

        return damage;
    }
    //消滅処理
    void SetDead()
    {
        if (State.Alive != state) return;

        SoundManager.Instance.PlaySE(10);

        WaveManager.Instance.DestroyEnemy();

        // 物理挙動を停止
        rb2d.simulated = false;

        StageManager.Instance.sumExp += Data.DropExp;

        Destroy(gameObject);

        state = State.Dead;

    }

    //アビリティ追加
    void AddAbilityActivator(int id)
    {
        BaseAbilityActivator activator = abilityActivators.Find(item =>item.Data.Id==id);

        activator = AbilitySettings.Instance.CreateAbilityActivator(id,transform);
    }

    //移動処理(もっと動かしたい場合は変更が必要)
    void MoveEnemyCount()
    {
        moveCount += Time.deltaTime;
        if (Data.MoveInterval < moveCount && !isMoving)
        {
            StartCoroutine(MoveEnemy());
            moveCount = 0;
        }
    }
    IEnumerator MoveEnemy()
    {
        isMoving = true;
        float moveTime = 0;

        //向き決定
        float randomAngle = Random.Range(0f, 360f);
        //現状動きが少ないので必要に応じてtargetPosの計算式を変更
        Vector2 course = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        Vector2 targetPos = (Vector2)transform.position + course;

        while (Data.MoveDistance > moveTime)
        {
            anim.SetBool("isMooving", true);
            moveTime += Time.deltaTime;
            //もし当たり判定のあるものがなければ進むようにする
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPos, checkRadius);
            if (hitColliders.Length == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, mooveSpeed * Time.deltaTime);
            }
            // 次のフレームまで待機
            yield return null;
        }
        anim.SetBool("isMooving", false);
        isMoving = false;
    }
    //範囲可視化(確認用)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 position = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y, 0);
        Vector2 position2 = new Vector2(position.x, position.y);
        Gizmos.DrawWireSphere(position2, checkRadius);
    }
}
