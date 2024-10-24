using System.Collections;
using UnityEngine;

//360度弾スポナー管理スクリプト
public class AbilityBall360SpawnerController : BaseAbilityActivator
{
    //弾生成数
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
        //次のタイム
        spawnTimer = Data.SpawnTimerMax;
    }

    //発射
    private void Shot(Rigidbody2D rb,float angle)
    {
        float ballDirX = Mathf.Sin(angle * Mathf.Deg2Rad);
        float ballDirY = Mathf.Cos(angle * Mathf.Deg2Rad);

        Vector2 ballDirection = new Vector2(ballDirX, ballDirY).normalized;

        rb.velocity = ballDirection * Data.ShotSpeed;
    }

    IEnumerator ShootWithDelay(float angleStep)
    {
        // ActivateCount 回だけループする
        for (int i = 0; i < (int)Data.ActivateCount; i++)
        {
            float angle = 0f;

            // 弾の数だけループして発射
            for (int j = 0; j < (int)numberOfBalls; j++)
            {
                GameObject obj = CreateAbility(transform.position, ballParent.transform).gameObject;
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

                // Shotメソッドで発射処理
                Shot(rb, angle);

                // 角度を更新
                angle += angleStep;
            }

            // 各iのループごとに 0.5 秒遅らせる（時間は自由に設定可能）
            yield return new WaitForSeconds(0.5f);
        }
    }
}
