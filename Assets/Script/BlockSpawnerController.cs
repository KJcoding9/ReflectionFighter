using System;
using UnityEngine;
using System.Collections.Generic;

//インスペクターから設定
[Serializable]
public class BlockSpawnData
{
    //説明用
    public string Title;

    //生成する行
    public int rows;
    //生成する列
    public int columns;
    //生成する間隔
    public float spacingX;
    public float spacingY;

    //どの位置から生成するか
    public Vector2 firstPos;

    // 出現経過時間
    public int ElapsedMinutes;
    public int ElapsedSeconds;

    //生成時間
    public float SpawnDuration;
    //生成数
    public int SpawnCountMax;
    //生成するブロックID
    public List<int> BlockIds;
}

//ブロックスポーナースクリプト
public class BlockSpawnerController : MonoBehaviour
{
    [System.Serializable]
    public class BlockSpawnDataList
    {
        //ブロックデータ
        public List<BlockSpawnData> blockSpawnDataList;
    }
    [System.Serializable]
    public class WaveSpawnBlockList
    {
        //Wave単位のブロックデータ
        public List<BlockSpawnDataList> waveBlockSpawnDataList;
    }

    [SerializeField] List<WaveSpawnBlockList> blockSpawnDatas;
    //生成したブロック
    List<BlockController> blocks;
    //ゲームマネージャー
    GameManager gameManager;
    //参照データ
    BlockSpawnData blockSpawnData;
    //生成したブロックを入れるオブジェクト
    [SerializeField] GameObject parentBlock;
    //ウェーブマネージャー
    [SerializeField] WaveManager waveManager;


    //ランダム生成は使わない
    ////経過時間
    //float oldSeconds;
    //float spawnTimer;
    ////現在のデータ位置
    //int spawnDataIndex;


    // 初期化
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;

        // 生成したブロックを保存
        blocks = new List<BlockController>();
        //spawnDataIndex = -1;
        SpecifiedSpawn(waveManager.CurrentWave);
    }

    //指定したデータのブロックを生成
    public void SpecifiedSpawn(int id)
    {
        for(int i=0; i < blockSpawnDatas[0].waveBlockSpawnDataList[id].blockSpawnDataList.Count; i++)
        {
            blockSpawnData = blockSpawnDatas[0].waveBlockSpawnDataList[id].blockSpawnDataList[i];

            for (int row = 0; row < blockSpawnData.rows; row++)
            {
                int blockId = blockSpawnData.BlockIds[row];

                for (int column = 0; column < blockSpawnData.columns; column++)
                {
                    Vector2 spawnPosition = new Vector2(blockSpawnData.firstPos.x + column * blockSpawnData.spacingX,
                        blockSpawnData.firstPos.y + row * blockSpawnData.spacingY);
                
                    BlockController block = BlockSettings.Instance.CreateBlock(blockId, gameManager, spawnPosition);
                    block.transform.SetParent(parentBlock.transform);
                    blocks.Add(block);
                }
            }
        }
    }

    // 全てのブロックを返す
    public List<BlockController> GetBlocks()
    {
        blocks.RemoveAll(bl => !bl);
        return blocks;
    }

    //ブロックをランダムで生成する処理は没
    ////ブロック生成
    //void SpawnBlock()
    //{
    //    //現在のデータ
    //    if (null == blockSpawnData) return;

    //    //タイマー消化
    //    spawnTimer -= Time.deltaTime;
    //    if (0 < spawnTimer) return;

    //    SpawnNormal();

    //    spawnTimer = blockSpawnData.SpawnDuration;
    //}

    ////生成
    //void SpawnNormal()
    //{
    //    // カメラのビューの高さと幅を計算
    //    float viewportHeight = Camera.main.orthographicSize * 2;
    //    float viewportWidth = viewportHeight * Camera.main.aspect;

    //    // カメラの左上端と右上端の位置を計算
    //    Vector2 cameraTopLeft = Camera.main.transform.position - new Vector3(viewportWidth / 2, viewportHeight / 2, 0);
    //    Vector2 cameraTopRight = cameraTopLeft + new Vector2(viewportWidth, 0);

    //    // カメラの上端と中央のY座標を計算
    //    float cameraTop = cameraTopLeft.y + viewportHeight; // カメラの上端のY座標
    //    float cameraCenterY = Camera.main.transform.position.y; // カメラの中央のY座標

    //    // オフセット
    //    float offset = 1.0f;
    //    float rightEndOffset = 2.0f;

    //    // プレハブを生成するX座標の範囲を決定
    //    float minX = cameraTopLeft.x;
    //    float maxX = cameraTopRight.x;

    //    // カメラの上端から中央までのランダムなY座標を決定
    //    float randomX;
    //    float randomY;
    //    Vector3 spawnPosition;
    //    bool positionFound = false;

    //    // 重ならない位置を見つけるまでループ
    //    int maxAttempts = 100; // 試行回数の上限
    //    int attempts = 0;

    //    do
    //    {
    //        randomX = UnityEngine.Random.Range(minX + offset, gameManager.rightEnd - rightEndOffset);
    //        randomY = UnityEngine.Random.Range(cameraCenterY, cameraTop - offset);

    //        // スポーン位置を設定
    //        spawnPosition = new Vector3(randomX, randomY, 0);

    //        // 他のブロックと重ならないかチェック
    //        positionFound = true;
    //        foreach (var block in blocks)
    //        {
    //            if (block != null)
    //            {
    //                if (Vector3.Distance(spawnPosition, block.transform.position) < offset)
    //                {
    //                    positionFound = false;
    //                    break;
    //                }
    //            }
    //        }

    //        attempts++;
    //    } while (!positionFound && attempts < maxAttempts);

    //    if (positionFound)
    //    {
    //        CreateRandomBlock(spawnPosition);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("重ならない位置が見つかりませんでした");
    //    }
    //}

    //void CreateRandomBlock(Vector3 pos)
    //{
    //    //データからランダムなIDを取得
    //    int rnd = UnityEngine.Random.Range(0, blockSpawnData.BlockIds.Count);
    //    int id = blockSpawnData.BlockIds[rnd];

    //    //TODO 生成した時アニメーション処理(DOTween)

    //    //ブロック生成
    //    BlockController block = BlockSettings.Instance.CreateBlock(id, gameManager, pos);
    //    blocks.Add(block);
    //}

    ////経過秒数でブロック生成データを入れ換える
    //void UpdateBlockSpawnData()
    //{
    //    //経過秒数が違う場合
    //    if (oldSeconds == gameManager.oldSeconds) return;

    //    // 1つ先のデータを参照
    //    int idx = spawnDataIndex + 1;

    //    // データの最後
    //    if (blockSpawnDatas.Count - 1 < idx) return;

    //    // 設定された経過時間を超えていたらデータを入れ換える TODO 0の部分はステージ
    //    BlockSpawnData data = blockSpawnDatas[0].waveBlockSpawnDataList[0].blockSpawnDataList[idx];
    //    int elapsedSeconds = data.ElapsedMinutes * 60 + data.ElapsedSeconds;

    //    if (elapsedSeconds < gameManager.gameTimer)
    //    {
    //        blockSpawnData = blockSpawnDatas[0].waveBlockSpawnDataList[0].blockSpawnDataList[idx];

    //        // 次回用の設定
    //        spawnDataIndex = idx;
    //        spawnTimer = 0;
    //        oldSeconds = gameManager.oldSeconds;
    //    }
    //}
}
