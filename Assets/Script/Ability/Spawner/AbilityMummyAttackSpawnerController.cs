using UnityEngine;
using System.Collections;

//マミーアタックスポナー管理スクリプト
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
        //次のタイム
        spawnTimer = Data.SpawnTimerMax;
    }

    IEnumerator ShootWithDelay()
    {
        Transform playerTransform = player.transform;
        // ActivateCount 回だけループする
        for (int i = 0; i < (int)Data.ActivateCount; i++)
        {
            GameObject obj = CreateAbility(transform.position, ballParent.transform).gameObject;
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            Shoot(rb,playerTransform);

            // 各 `i` のループごとに 0.5 秒遅らせる（時間は自由に設定可能）
            yield return new WaitForSeconds(0.5f);
        }
    }
    //弾発射
    private void Shoot(Rigidbody2D rb,Transform playerTransform)
    {
        SoundManager.Instance.PlaySE(16);
        //プレイヤーの位置に向かう方向を計算
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        //発射
        rb.velocity = direction * Data.ShotSpeed;
    }
}
