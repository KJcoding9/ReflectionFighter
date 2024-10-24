using UnityEngine;

//�R�E�����e�X�|�i�[�Ǘ��X�N���v�g
public class AbilityBatBallSpawnerController : BaseAbilityActivator
{
    private void Update()
    {
        if (IsSpawnTimerNotElapsed()) return;
        if(spawnTimer <= 0)
        {
            ballParent = GameObject.Find("ParentBall");
            for(int i=0; i<(int)Data.ActivateCount; i++)
            {
                CreateAbility(transform.position, ballParent.transform);
            }
        }
        //���̃^�C��
        spawnTimer = Data.SpawnTimerMax;
    }
}
