using UnityEngine;

//コウモリ弾スポナー管理スクリプト
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
        //次のタイム
        spawnTimer = Data.SpawnTimerMax;
    }
}
