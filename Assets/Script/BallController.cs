using UnityEngine;

//ボールの移動を制御するスクリプト
public class BallController : MonoBehaviour
{
    //もし色々なボールを追加するならボールデータから取ってくる方式に変更(今はボール１種想定)

    //ボールの初期速度
    [SerializeField]float defaultSpeed; 
    //ボールサイズ
    [SerializeField] float ballSize;
    //プレイヤー
    [SerializeField] GameObject player;
    //プレイヤーの矢印
    [SerializeField] Transform arrow;
    //rigidbody
    private Rigidbody2D rb2d;
    //プロパティ(ボール関係は値が変わってしまうと怖いので)
    public Rigidbody2D Rb2d { get { return rb2d; } set { rb2d = value; }  }
    //レンダラー
   private new Renderer renderer;
    //ボールの速度
    public float speed; 
    //タイミングで前の速度を記録
    public float previousSpeed;
    //最大出力速度
    public float maxSpeed;
    // 反射角度を制御する係数を設定
    public float reflectionAngleFactor = 0.3f;
    //垂直速度の最小値
    public float minVerticalSpeed = 0.5f;
    //パドルとの衝突を許可するフラグ
    private bool _canCollide = true; 
    //ボールがセットされているかのフラグ
    public bool ballSet = true;
    //ボールのSEが鳴ったかチェック
    private bool ballPlayShotSE = false;
    //減速する時間を計算
    private float decelerationTimer;
    //プレイヤースクリプト
    private PlayerController playerController;
    //プレイヤーのArrowから角度を取得と、ベクトル変換用
    public float reflectionAngle;
    //方向ベクトル計算用
    Vector2 forceDirection;

    void Update()
    {
        //最初かボール消滅時に位置をプレイヤーに
        if (player && ballSet)
        {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.E) && ballSet)
        {
            LaunchBall();
        }
        //反射角度設定
        reflectionAngle = arrow.transform.rotation.eulerAngles.z;
        // 角度をラジアンに変換
        float angleRad = reflectionAngle * Mathf.Deg2Rad;
        // 角度から方向ベクトルを計算
        forceDirection = new Vector2(-Mathf.Sin(angleRad), Mathf.Cos(angleRad));
        //スピードに代入
        speed = rb2d.velocity.magnitude;

        //ボール加速タイミングが剣攻撃なので、専用のAudioSourceをもう一つ付ける必要がある
        //if (rb2d.velocity.magnitude > 7f)
        //{
        //    if (!ballPlayShotSE)
        //    {
        //        SoundManager.Instance.PlaySE(17);
        //        ballPlayShotSE = true;
        //    }
        //}
        //else
        //{
        //    ballPlayShotSE = false;
        //}
        BallSpeedControll();
    }
    //初期化処理
    public void Init(GameObject player,Transform arrow)
    {
        this.player = player;
        this.arrow = arrow;
        renderer = GetComponent<Renderer>();
        rb2d = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();

        Utils.SetAlpha(renderer.material, 0f);
        ballSet = true;
        reflectionAngle = arrow.transform.rotation.eulerAngles.z;
        
    }
    //衝突時
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.Instance.PlaySE(1);

        // 衝突点の法線ベクトルを取得
        Vector2 normal = collision.contacts[0].normal;

        if (collision.gameObject.CompareTag("Wall") && _canCollide)
        {
            speed += 0.5f;
            //衝突を無効にする
            _canCollide = false;
            //0.5秒後にリセット
            Invoke("ResetCollision", 0.5f);
        }

        if (collision.gameObject.CompareTag("Player") && _canCollide)
        {
            //衝突を無効にする
            _canCollide = false;
            // 現在の速度ベクトル
            Vector2 incomingVelocity = rb2d.velocity;
            // 法線ベクトルに基づいて反射方向を計算
            Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, normal);
            // 反射方向を指定した角度に調整
            float angleBetween = Vector2.SignedAngle(reflectedVelocity, forceDirection);
            Vector2 newDirection = Quaternion.Euler(0, 0, angleBetween) * reflectedVelocity;
            //総合スピードを代入
            float totalSpeed = speed + playerController.previousCharge;
            //チャージしても速度上限値を超えないようにする
            totalSpeed = Mathf.Clamp(totalSpeed, 0, maxSpeed);
            //減速時間設定
            decelerationTimer = playerController.previousCharge;
            //前の速度代入
            previousSpeed = speed;
            // ボールの速度を更新
            rb2d.velocity = newDirection.normalized * totalSpeed;
            //2.5秒後にリセット
            Invoke("ResetCollision", 2.5f);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //エネミーコントローラー取得
            var enemyController = collision.gameObject.GetComponent<EnemyController>();
            //ダメージ処理
            enemyController.Damage(playerController.Data.Attack,speed);
        }

        if(collision.gameObject.CompareTag("Block")|| collision.gameObject.CompareTag("AbilityBlock"))
        {
            if (collision.gameObject.TryGetComponent<BlockController>(out var blockController)){
                //付けてたらダメージ処理
                blockController.Damage(playerController.Data.Attack);
            }
            else
            {
                //アビリティブロックだった場合
                var abilityBlockController = collision.gameObject.GetComponent<AbilityBlockController>();

                abilityBlockController.Damage(playerController.Data.Attack);
            }
        }

        //ボールの水平移動によるループ阻止
        Vector2 vec = rb2d.velocity;
        if (0.25f > Mathf.Abs(vec.y))
        {
            if (0.0f <= (vec.x * vec.y))
                vec = Quaternion.Euler(0.0f, 0.0f, 15.0f) * vec;
            else
                vec = Quaternion.Euler(0.0f, 0.0f, -15.0f) * vec;
        }
        rb2d.velocity = vec;

        //速度低下によるループを阻止
        Vector2 vec2 = rb2d.velocity;
        if (Mathf.Abs(rb2d.velocity.y) < 1)
        {
            vec2.y = rb2d.velocity.y * 3;
        }
        if (Mathf.Abs(rb2d.velocity.x) < 1)
        {
            vec2.x = rb2d.velocity.x * 3;
        }

        rb2d.velocity = vec2;
    }

    //DeathWall専用
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathWall"))
        {
            SoundManager.Instance.PlaySE(2);
            //33%HP減少させてからボール消滅処理
            float ratioDamage = (float)player.GetComponent<PlayerController>().Data.MaxHP * 0.33f;
            player.GetComponent<PlayerController>().Data.HP -= (int)Mathf.Ceil(ratioDamage);
            rb2d.velocity = Vector2.zero;
            Utils.SetAlpha(renderer.material, 0f);
            ballSet = true;
        }
    }

    //ボールを発射する関数
    void LaunchBall()
    {
        ballSet = false;
        Utils.SetAlpha(renderer.material, 1f);
        rb2d.AddForce(forceDirection * 300f);

        SoundManager.Instance.PlaySE(4);
    }

    void ResetCollision()
    {
        _canCollide = true;
    }

    //ボールの減速（チャージして加速した分を引く）
    void BallSpeedControll()
    {
        if (decelerationTimer > 0)
        {
            float reducedSpeed = speed - (playerController.Data.DecayRate * Time.deltaTime);
            //元のspeed以下にさせない
            rb2d.velocity = rb2d.velocity.normalized * Mathf.Max(reducedSpeed, previousSpeed); 
            decelerationTimer -= Time.deltaTime;
        }

        if (rb2d.velocity.magnitude < defaultSpeed)
        {
            rb2d.velocity += rb2d.velocity.normalized * 0.1f;
        }
    }
}
