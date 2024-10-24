using UnityEngine;
using System.Collections;

//プレイヤー管理スクリプト
public class PlayerController : MonoBehaviour
{
    //スピード(必要に応じてプレイヤーデータに移行)
    [SerializeField] float speed;
    //ゲームマネージャー
    [SerializeField] GameManager gameManager;
    //ボールコントローラー
    [SerializeField] BallController ballController;

    //プレイヤーデータ
    public PlayerData Data;

    //rigidbody
    Rigidbody2D rb;
    //コライダー
    Collider2D col;
    //位置
    Vector2 pos;
    //チャージエフェクト用
    private GameObject currentEffect;
    //半分の幅
    private float _halfObjectWidth;
    //どっちに移動してるかのフラグ
    private bool _isMovingLeft = true;
    private bool _isMovingRight = true;
    //チャージ用フラグ
    private bool oneShot = false;
    //向き変更フラグ
    private bool flipped = false;
    //チャージ用SEのフラグ
    private bool chargeMax = false;

    //必要に応じてプレイヤーデータに統合

    //再度攻撃可能時間
    public float attackTimer;
    //現在のチャージ量
    public float currentCharge = 0.0f;
    //前のチャージ量
    public float previousCharge = 0.0f;
    //攻撃時クールタイム
    public float attackCoolTime;
    //次に攻撃可能な時間
    public float nextAttackTime = 0;

    //チャージ状態移行条件
    public float holdDuration = 0.5f;

    //跳ね返す時の角度を決める矢印
    public GameObject arrow;

    //プレイヤーアニメーター
    Animator playerAnim;

    //攻撃用キー
    public KeyCode attackKey = KeyCode.Space;
    
    //状態
    public enum State
    {
        Idle,
        Charging,
        Attack
    }
    public State state;

    void Update()
    {
        MovingControll();
        Charge();
        Attack();
        PlayerFlip();
        UpdateAnim();
    }

    //初期化
    public void Init(GameManager gameManager, PlayerData data,BallController ballController)
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();

        this.gameManager = gameManager;
        this.ballController = ballController;
        this.Data = data;

        if (col != null)
        {
            _halfObjectWidth = (col.bounds.size.x / 2);

            col.enabled = false;
        }

        pos = rb.position;
        attackTimer = 0f;
        attackCoolTime = Data.AttackCoolTime;
        state = State.Idle;
    }
    //移動メソッド
    private void MovingControll()
    {
        // 移動キーが押された時に移動する
        float horizontal = Input.GetAxis("Horizontal");

        // オブジェクトが画面端に到達した場合、その方向への移動を無効にする
        if (rb.position.x <= gameManager.leftEnd + _halfObjectWidth + 0.1f)
        {
            _isMovingLeft = false;
        }
        else
        {
            _isMovingLeft = true;
        }

        if (rb.position.x >= gameManager.rightEnd - _halfObjectWidth - 1.2f)
        {
            _isMovingRight = false;
        }
        else
        {
            _isMovingRight = true;
        }

        // 移動制御
        if (horizontal < 0 && _isMovingLeft)
        {
            pos.x += horizontal * speed * Time.deltaTime;
        }
        else if (horizontal > 0 && _isMovingRight)
        {
            pos.x += horizontal * speed * Time.deltaTime;
        }

        // 位置の更新
        rb.position = pos;
    }

    // ダメージ処理
    public void Damage(float attack)
    {
        // 非アクティブなら抜ける
        if (!enabled) return;

        int damage = (int)attack - (int)Data.Defense;
        if(damage < 0)
        {
            damage = 0;
        }
        SoundManager.Instance.PlaySE(9);

        Data.HP -= damage;


        // ゲームオーバー
        if (0 >= Data.HP)
        {
            // 操作できないようにする
            SetEnabled(false);

           gameManager.DispPanelGameOver();
        }
    }

    //アタックの判定
    void Attack()
    {
        //キーが離された時に攻撃
        if (Input.GetKeyUp(attackKey) && !ballController.ballSet && Time.time >= nextAttackTime
            && gameManager.isPosing == false)
        {
            //チャージ取得(計算用)
            previousCharge = currentCharge;
            //チャージリセット
            currentCharge = 0;
            //状態遷移
            state = State.Attack;
            SoundManager.Instance.PlaySE(14);
            //もし装備システム実装するなら変更
            VFXManager.Instance.PlayEffect(this.gameObject.transform,0);
            //アタックタイマー計算
            attackTimer = Data.AttackActivationTime;
            nextAttackTime = Time.time + attackCoolTime;
        }
        else if (!Input.GetKeyUp(attackKey) && state != State.Charging)
        {
            state = State.Idle;
        }
    }
    //チャージ処理
    public void Charge()
    {
        if (Input.GetKey(KeyCode.Space) && !ballController.ballSet
            &&gameManager.isPosing == false)
        {
            //チャージしてる間はエフェクト再生
            if (currentEffect == null)
            {
                currentEffect = VFXManager.Instance.SpawnAndPlayEffectReturn(this.gameObject.transform, 2);
            }
            state = State.Charging;
            StartCoroutine(ChargeCalc());
            //チャージマックス時SE再生
            if (oneShot == false)
            {
                SoundManager.Instance.PlaySELoop(6);
                oneShot = true;
            }
            if(currentCharge == Data.MaxCharge && chargeMax == false)
            {
                SoundManager.Instance.PlaySE(18);
                chargeMax = true;
            }
        }
        else
        {
            chargeMax = false;
            if(currentEffect != null)
            {
                Destroy(currentEffect);
            }
            SoundManager.Instance.StopSELoop();
            oneShot = false;
        }
        //チャージエフェクトの位置固定
        if (currentEffect != null)
        {
            currentEffect.transform.position = this.gameObject.transform.position;
        }
    }
    //チャージ増加値計算
    IEnumerator ChargeCalc()
    {
        currentCharge += Data.ChargeRate * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0, Data.MaxCharge);
        yield return null;
    }

    //キーを押して反転させる
    public void PlayerFlip()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            float previousRotation = arrow.transform.rotation.z;
            arrow.transform.parent = null;
            //プレイヤーを反転
            Vector2 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            flipped = !flipped;
            arrow.transform.parent = gameObject.transform;
            Vector3 currentRotation = arrow.transform.eulerAngles;
            currentRotation.z = previousRotation;
            arrow.transform.eulerAngles = currentRotation;
            arrow.transform.position = new Vector3(gameObject.transform.position.x - 0.0005f, gameObject.transform.position.y + 1.4f, 0f);
            arrow.GetComponent<ArrowController>().currentAngle = 0;
        }
    }
    //アニメーションを管理
    void UpdateAnim()
    {
        if(state == State.Attack)
        {
            playerAnim.SetTrigger("IsAttack");
        }
    }
    // 停止
    public void SetEnabled(bool enabled = true)
    {
        this.enabled = enabled;
    }
}
