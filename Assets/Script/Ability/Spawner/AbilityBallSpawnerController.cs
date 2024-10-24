using UnityEngine;

//弾スポナー管理スクリプト
public class AbilityBallSpawnerController : BaseAbilityActivator
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
