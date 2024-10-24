using UnityEngine;

//�A�r���e�B�u���b�N�����X�|�i�[�Ǘ��X�N���v�g
public class AbilityBlockSpawnerController : BaseAbilityActivator
{
    //�d������͈�
    private float checkRadius = 0.3f;

    void Update()
    {
        if (IsSpawnTimerNotElapsed()) return;

        if (spawnTimer <= 0)
        {
            for (int i = 0; i < (int)Data.ActivateCount; i++)
            {
                float randomAngle = Random.Range(0f, 360f);

                Vector3 position = new Vector3(
                parentTransform.position.x + Mathf.Cos(randomAngle * Mathf.Deg2Rad),
                parentTransform.position.y + Mathf.Sin(randomAngle * Mathf.Deg2Rad),
                0 // Z���W��2D�Ȃ̂�0�̂܂�
                );

                Vector2 position2 = new Vector2(position.x, position.y);
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position2, checkRadius);
                if (hitColliders.Length == 0)
                {
                    blockParent = GameObject.Find("ParentBlock");
                    if(blockParent != null)
                    {
                        CreateAbility(position, blockParent.transform);
                    }
                }
            }
        }
        //���̃^�C��
        spawnTimer = Data.SpawnTimerMax;
    }
    //�����蔻�����
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 position = new Vector3(parentTransform.gameObject.transform.position.x,
            parentTransform.gameObject.transform.position.y - 1.1f, 0);
        Vector2 position2 = new Vector2(position.x, position.y);
        Gizmos.DrawWireSphere(position2, checkRadius);
    }
}
