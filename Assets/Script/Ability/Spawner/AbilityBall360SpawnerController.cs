using System.Collections;
using UnityEngine;

//360�x�e�X�|�i�[�Ǘ��X�N���v�g
public class AbilityBall360SpawnerController : BaseAbilityActivator
{
    //�e������
    public int numberOfBalls = 16;

    void Update()
    {
        if (IsSpawnTimerNotElapsed()) return;
        if (spawnTimer <= 0)
        {
            float angleStep = 360f / numberOfBalls;
            ballParent = GameObject.Find("ParentBall");
            StartCoroutine(ShootWithDelay(angleStep));
        }
        //���̃^�C��
        spawnTimer = Data.SpawnTimerMax;
    }

    //����
    private void Shot(Rigidbody2D rb,float angle)
    {
        float ballDirX = Mathf.Sin(angle * Mathf.Deg2Rad);
        float ballDirY = Mathf.Cos(angle * Mathf.Deg2Rad);

        Vector2 ballDirection = new Vector2(ballDirX, ballDirY).normalized;

        rb.velocity = ballDirection * Data.ShotSpeed;
    }

    IEnumerator ShootWithDelay(float angleStep)
    {
        // ActivateCount �񂾂����[�v����
        for (int i = 0; i < (int)Data.ActivateCount; i++)
        {
            float angle = 0f;

            // �e�̐��������[�v���Ĕ���
            for (int j = 0; j < (int)numberOfBalls; j++)
            {
                GameObject obj = CreateAbility(transform.position, ballParent.transform).gameObject;
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

                // Shot���\�b�h�Ŕ��ˏ���
                Shot(rb, angle);

                // �p�x���X�V
                angle += angleStep;
            }

            // �ei�̃��[�v���Ƃ� 0.5 �b�x�点��i���Ԃ͎��R�ɐݒ�\�j
            yield return new WaitForSeconds(0.5f);
        }
    }
}
