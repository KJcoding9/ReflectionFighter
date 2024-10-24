using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G���Ǘ�����X�N���v�g(�K�v�ɂȂ����Ƃ��Ƀ{�X��p�̂��̂����)
public class EnemyController : MonoBehaviour
{
    //�G�f�[�^
    public CharacterData Data;
    //�Q�[���}�l�[�W���[
    [SerializeField] GameManager gameManager;
    //�GHP�o�[
    [SerializeField] EnemyHPBar enemyHPBar;
    //rigidbody
    Rigidbody2D rb2d;
    //�K�v������ꍇ�́A�L�����N�^�[�f�[�^�Ɉڍs���Ă������璼�ڃf�[�^�ǂݎ������ɕύX
    public float mooveSpeed = 0.5f;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    //�ړ��J�E���g
    private float moveCount;
    // �Փ˃`�F�b�N�̔��a
    public float checkRadius = 0.5f;
    //�A�j���[�^�[
    private Animator anim;
    //�ړ��t���O
    private bool isMoving = false;

    //�������Ă���A�r���e�B
    public List<BaseAbilityActivator> abilityActivators;

    // ���
    enum State
    {
        Alive,
        Dead
    }
    State state;
    //�ړ����
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
    //������
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

        //�A�r���e�B�f�[�^�Z�b�g
        foreach(var item in Data.DefaultAbilityIds)
        {
            AddAbilityActivator(item);
        }
    }
    //�_���[�W����
    public float Damage(float attack,float speed)
    {
        // ��A�N�e�B�u
        if (State.Alive != state) return 0;

        //speed�����ȏ�Ń_���[�W�{�[�i�X
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

        // �_���[�W�\��
        gameManager.DispDamage(gameObject, damage);
        // ����
        if (0 >= Data.HP)
        {
            gameManager.score += Data.Score;
            VFXManager.Instance.SpawnAndPlayEffect(this.gameObject.transform, 1);
            SetDead();
        }

        return damage;
    }
    //���ŏ���
    void SetDead()
    {
        if (State.Alive != state) return;

        SoundManager.Instance.PlaySE(10);

        WaveManager.Instance.DestroyEnemy();

        // �����������~
        rb2d.simulated = false;

        StageManager.Instance.sumExp += Data.DropExp;

        Destroy(gameObject);

        state = State.Dead;

    }

    //�A�r���e�B�ǉ�
    void AddAbilityActivator(int id)
    {
        BaseAbilityActivator activator = abilityActivators.Find(item =>item.Data.Id==id);

        activator = AbilitySettings.Instance.CreateAbilityActivator(id,transform);
    }

    //�ړ�����(�����Ɠ����������ꍇ�͕ύX���K�v)
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

        //��������
        float randomAngle = Random.Range(0f, 360f);
        //���󓮂������Ȃ��̂ŕK�v�ɉ�����targetPos�̌v�Z����ύX
        Vector2 course = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        Vector2 targetPos = (Vector2)transform.position + course;

        while (Data.MoveDistance > moveTime)
        {
            anim.SetBool("isMooving", true);
            moveTime += Time.deltaTime;
            //���������蔻��̂�����̂��Ȃ���ΐi�ނ悤�ɂ���
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPos, checkRadius);
            if (hitColliders.Length == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, mooveSpeed * Time.deltaTime);
            }
            // ���̃t���[���܂őҋ@
            yield return null;
        }
        anim.SetBool("isMooving", false);
        isMoving = false;
    }
    //�͈͉���(�m�F�p)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 position = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y, 0);
        Vector2 position2 = new Vector2(position.x, position.y);
        Gizmos.DrawWireSphere(position2, checkRadius);
    }
}
