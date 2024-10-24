using UnityEngine;
using System.Collections;

//�}�~�[�A�^�b�N�X�|�i�[�Ǘ��X�N���v�g
public class AbilityMummyAttackSpawnerController : BaseAbilityActivator
{
    protected GameObject player;

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");

        if (IsSpawnTimerNotElapsed()) return;
        if (spawnTimer <= 0)
        {
            ballParent = GameObject.Find("ParentBall");
            StartCoroutine(ShootWithDelay());
        }
        //���̃^�C��
        spawnTimer = Data.SpawnTimerMax;
    }

    IEnumerator ShootWithDelay()
    {
        Transform playerTransform = player.transform;
        // ActivateCount �񂾂����[�v����
        for (int i = 0; i < (int)Data.ActivateCount; i++)
        {
            GameObject obj = CreateAbility(transform.position, ballParent.transform).gameObject;
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            Shoot(rb,playerTransform);

            // �e `i` �̃��[�v���Ƃ� 0.5 �b�x�点��i���Ԃ͎��R�ɐݒ�\�j
            yield return new WaitForSeconds(0.5f);
        }
    }
    //�e����
    private void Shoot(Rigidbody2D rb,Transform playerTransform)
    {
        SoundManager.Instance.PlaySE(16);
        //�v���C���[�̈ʒu�Ɍ������������v�Z
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        //����
        rb.velocity = direction * Data.ShotSpeed;
    }
}
