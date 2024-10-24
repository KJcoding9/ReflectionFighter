using UnityEngine;
using System.Collections;

//�v���C���[�Ǘ��X�N���v�g
public class PlayerController : MonoBehaviour
{
    //�X�s�[�h(�K�v�ɉ����ăv���C���[�f�[�^�Ɉڍs)
    [SerializeField] float speed;
    //�Q�[���}�l�[�W���[
    [SerializeField] GameManager gameManager;
    //�{�[���R���g���[���[
    [SerializeField] BallController ballController;

    //�v���C���[�f�[�^
    public PlayerData Data;

    //rigidbody
    Rigidbody2D rb;
    //�R���C�_�[
    Collider2D col;
    //�ʒu
    Vector2 pos;
    //�`���[�W�G�t�F�N�g�p
    private GameObject currentEffect;
    //�����̕�
    private float _halfObjectWidth;
    //�ǂ����Ɉړ����Ă邩�̃t���O
    private bool _isMovingLeft = true;
    private bool _isMovingRight = true;
    //�`���[�W�p�t���O
    private bool oneShot = false;
    //�����ύX�t���O
    private bool flipped = false;
    //�`���[�W�pSE�̃t���O
    private bool chargeMax = false;

    //�K�v�ɉ����ăv���C���[�f�[�^�ɓ���

    //�ēx�U���\����
    public float attackTimer;
    //���݂̃`���[�W��
    public float currentCharge = 0.0f;
    //�O�̃`���[�W��
    public float previousCharge = 0.0f;
    //�U�����N�[���^�C��
    public float attackCoolTime;
    //���ɍU���\�Ȏ���
    public float nextAttackTime = 0;

    //�`���[�W��Ԉڍs����
    public float holdDuration = 0.5f;

    //���˕Ԃ����̊p�x�����߂���
    public GameObject arrow;

    //�v���C���[�A�j���[�^�[
    Animator playerAnim;

    //�U���p�L�[
    public KeyCode attackKey = KeyCode.Space;
    
    //���
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

    //������
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
    //�ړ����\�b�h
    private void MovingControll()
    {
        // �ړ��L�[�������ꂽ���Ɉړ�����
        float horizontal = Input.GetAxis("Horizontal");

        // �I�u�W�F�N�g����ʒ[�ɓ��B�����ꍇ�A���̕����ւ̈ړ��𖳌��ɂ���
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

        // �ړ�����
        if (horizontal < 0 && _isMovingLeft)
        {
            pos.x += horizontal * speed * Time.deltaTime;
        }
        else if (horizontal > 0 && _isMovingRight)
        {
            pos.x += horizontal * speed * Time.deltaTime;
        }

        // �ʒu�̍X�V
        rb.position = pos;
    }

    // �_���[�W����
    public void Damage(float attack)
    {
        // ��A�N�e�B�u�Ȃ甲����
        if (!enabled) return;

        int damage = (int)attack - (int)Data.Defense;
        if(damage < 0)
        {
            damage = 0;
        }
        SoundManager.Instance.PlaySE(9);

        Data.HP -= damage;


        // �Q�[���I�[�o�[
        if (0 >= Data.HP)
        {
            // ����ł��Ȃ��悤�ɂ���
            SetEnabled(false);

           gameManager.DispPanelGameOver();
        }
    }

    //�A�^�b�N�̔���
    void Attack()
    {
        //�L�[�������ꂽ���ɍU��
        if (Input.GetKeyUp(attackKey) && !ballController.ballSet && Time.time >= nextAttackTime
            && gameManager.isPosing == false)
        {
            //�`���[�W�擾(�v�Z�p)
            previousCharge = currentCharge;
            //�`���[�W���Z�b�g
            currentCharge = 0;
            //��ԑJ��
            state = State.Attack;
            SoundManager.Instance.PlaySE(14);
            //���������V�X�e����������Ȃ�ύX
            VFXManager.Instance.PlayEffect(this.gameObject.transform,0);
            //�A�^�b�N�^�C�}�[�v�Z
            attackTimer = Data.AttackActivationTime;
            nextAttackTime = Time.time + attackCoolTime;
        }
        else if (!Input.GetKeyUp(attackKey) && state != State.Charging)
        {
            state = State.Idle;
        }
    }
    //�`���[�W����
    public void Charge()
    {
        if (Input.GetKey(KeyCode.Space) && !ballController.ballSet
            &&gameManager.isPosing == false)
        {
            //�`���[�W���Ă�Ԃ̓G�t�F�N�g�Đ�
            if (currentEffect == null)
            {
                currentEffect = VFXManager.Instance.SpawnAndPlayEffectReturn(this.gameObject.transform, 2);
            }
            state = State.Charging;
            StartCoroutine(ChargeCalc());
            //�`���[�W�}�b�N�X��SE�Đ�
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
        //�`���[�W�G�t�F�N�g�̈ʒu�Œ�
        if (currentEffect != null)
        {
            currentEffect.transform.position = this.gameObject.transform.position;
        }
    }
    //�`���[�W�����l�v�Z
    IEnumerator ChargeCalc()
    {
        currentCharge += Data.ChargeRate * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0, Data.MaxCharge);
        yield return null;
    }

    //�L�[�������Ĕ��]������
    public void PlayerFlip()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            float previousRotation = arrow.transform.rotation.z;
            arrow.transform.parent = null;
            //�v���C���[�𔽓]
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
    //�A�j���[�V�������Ǘ�
    void UpdateAnim()
    {
        if(state == State.Attack)
        {
            playerAnim.SetTrigger("IsAttack");
        }
    }
    // ��~
    public void SetEnabled(bool enabled = true)
    {
        this.enabled = enabled;
    }
}
