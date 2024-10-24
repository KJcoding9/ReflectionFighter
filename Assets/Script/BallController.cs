using UnityEngine;

//�{�[���̈ړ��𐧌䂷��X�N���v�g
public class BallController : MonoBehaviour
{
    //�����F�X�ȃ{�[����ǉ�����Ȃ�{�[���f�[�^�������Ă�������ɕύX(���̓{�[���P��z��)

    //�{�[���̏������x
    [SerializeField]float defaultSpeed; 
    //�{�[���T�C�Y
    [SerializeField] float ballSize;
    //�v���C���[
    [SerializeField] GameObject player;
    //�v���C���[�̖��
    [SerializeField] Transform arrow;
    //rigidbody
    private Rigidbody2D rb2d;
    //�v���p�e�B(�{�[���֌W�͒l���ς���Ă��܂��ƕ|���̂�)
    public Rigidbody2D Rb2d { get { return rb2d; } set { rb2d = value; }  }
    //�����_���[
   private new Renderer renderer;
    //�{�[���̑��x
    public float speed; 
    //�^�C�~���O�őO�̑��x���L�^
    public float previousSpeed;
    //�ő�o�͑��x
    public float maxSpeed;
    // ���ˊp�x�𐧌䂷��W����ݒ�
    public float reflectionAngleFactor = 0.3f;
    //�������x�̍ŏ��l
    public float minVerticalSpeed = 0.5f;
    //�p�h���Ƃ̏Փ˂�������t���O
    private bool _canCollide = true; 
    //�{�[�����Z�b�g����Ă��邩�̃t���O
    public bool ballSet = true;
    //�{�[����SE���������`�F�b�N
    private bool ballPlayShotSE = false;
    //�������鎞�Ԃ��v�Z
    private float decelerationTimer;
    //�v���C���[�X�N���v�g
    private PlayerController playerController;
    //�v���C���[��Arrow����p�x���擾�ƁA�x�N�g���ϊ��p
    public float reflectionAngle;
    //�����x�N�g���v�Z�p
    Vector2 forceDirection;

    void Update()
    {
        //�ŏ����{�[�����Ŏ��Ɉʒu���v���C���[��
        if (player && ballSet)
        {
            transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.E) && ballSet)
        {
            LaunchBall();
        }
        //���ˊp�x�ݒ�
        reflectionAngle = arrow.transform.rotation.eulerAngles.z;
        // �p�x�����W�A���ɕϊ�
        float angleRad = reflectionAngle * Mathf.Deg2Rad;
        // �p�x��������x�N�g�����v�Z
        forceDirection = new Vector2(-Mathf.Sin(angleRad), Mathf.Cos(angleRad));
        //�X�s�[�h�ɑ��
        speed = rb2d.velocity.magnitude;

        //�{�[�������^�C�~���O�����U���Ȃ̂ŁA��p��AudioSource��������t����K�v������
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
    //����������
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
    //�Փˎ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.Instance.PlaySE(1);

        // �Փ˓_�̖@���x�N�g�����擾
        Vector2 normal = collision.contacts[0].normal;

        if (collision.gameObject.CompareTag("Wall") && _canCollide)
        {
            speed += 0.5f;
            //�Փ˂𖳌��ɂ���
            _canCollide = false;
            //0.5�b��Ƀ��Z�b�g
            Invoke("ResetCollision", 0.5f);
        }

        if (collision.gameObject.CompareTag("Player") && _canCollide)
        {
            //�Փ˂𖳌��ɂ���
            _canCollide = false;
            // ���݂̑��x�x�N�g��
            Vector2 incomingVelocity = rb2d.velocity;
            // �@���x�N�g���Ɋ�Â��Ĕ��˕������v�Z
            Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, normal);
            // ���˕������w�肵���p�x�ɒ���
            float angleBetween = Vector2.SignedAngle(reflectedVelocity, forceDirection);
            Vector2 newDirection = Quaternion.Euler(0, 0, angleBetween) * reflectedVelocity;
            //�����X�s�[�h����
            float totalSpeed = speed + playerController.previousCharge;
            //�`���[�W���Ă����x����l�𒴂��Ȃ��悤�ɂ���
            totalSpeed = Mathf.Clamp(totalSpeed, 0, maxSpeed);
            //�������Ԑݒ�
            decelerationTimer = playerController.previousCharge;
            //�O�̑��x���
            previousSpeed = speed;
            // �{�[���̑��x���X�V
            rb2d.velocity = newDirection.normalized * totalSpeed;
            //2.5�b��Ƀ��Z�b�g
            Invoke("ResetCollision", 2.5f);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //�G�l�~�[�R���g���[���[�擾
            var enemyController = collision.gameObject.GetComponent<EnemyController>();
            //�_���[�W����
            enemyController.Damage(playerController.Data.Attack,speed);
        }

        if(collision.gameObject.CompareTag("Block")|| collision.gameObject.CompareTag("AbilityBlock"))
        {
            if (collision.gameObject.TryGetComponent<BlockController>(out var blockController)){
                //�t���Ă���_���[�W����
                blockController.Damage(playerController.Data.Attack);
            }
            else
            {
                //�A�r���e�B�u���b�N�������ꍇ
                var abilityBlockController = collision.gameObject.GetComponent<AbilityBlockController>();

                abilityBlockController.Damage(playerController.Data.Attack);
            }
        }

        //�{�[���̐����ړ��ɂ�郋�[�v�j�~
        Vector2 vec = rb2d.velocity;
        if (0.25f > Mathf.Abs(vec.y))
        {
            if (0.0f <= (vec.x * vec.y))
                vec = Quaternion.Euler(0.0f, 0.0f, 15.0f) * vec;
            else
                vec = Quaternion.Euler(0.0f, 0.0f, -15.0f) * vec;
        }
        rb2d.velocity = vec;

        //���x�ቺ�ɂ�郋�[�v��j�~
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

    //DeathWall��p
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathWall"))
        {
            SoundManager.Instance.PlaySE(2);
            //33%HP���������Ă���{�[�����ŏ���
            float ratioDamage = (float)player.GetComponent<PlayerController>().Data.MaxHP * 0.33f;
            player.GetComponent<PlayerController>().Data.HP -= (int)Mathf.Ceil(ratioDamage);
            rb2d.velocity = Vector2.zero;
            Utils.SetAlpha(renderer.material, 0f);
            ballSet = true;
        }
    }

    //�{�[���𔭎˂���֐�
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

    //�{�[���̌����i�`���[�W���ĉ����������������j
    void BallSpeedControll()
    {
        if (decelerationTimer > 0)
        {
            float reducedSpeed = speed - (playerController.Data.DecayRate * Time.deltaTime);
            //����speed�ȉ��ɂ����Ȃ�
            rb2d.velocity = rb2d.velocity.normalized * Mathf.Max(reducedSpeed, previousSpeed); 
            decelerationTimer -= Time.deltaTime;
        }

        if (rb2d.velocity.magnitude < defaultSpeed)
        {
            rb2d.velocity += rb2d.velocity.normalized * 0.1f;
        }
    }
}
