using System;
using UnityEngine;
using System.Collections.Generic;

//�C���X�y�N�^�[����ݒ�
[Serializable]
public class BlockSpawnData
{
    //�����p
    public string Title;

    //��������s
    public int rows;
    //���������
    public int columns;
    //��������Ԋu
    public float spacingX;
    public float spacingY;

    //�ǂ̈ʒu���琶�����邩
    public Vector2 firstPos;

    // �o���o�ߎ���
    public int ElapsedMinutes;
    public int ElapsedSeconds;

    //��������
    public float SpawnDuration;
    //������
    public int SpawnCountMax;
    //��������u���b�NID
    public List<int> BlockIds;
}

//�u���b�N�X�|�[�i�[�X�N���v�g
public class BlockSpawnerController : MonoBehaviour
{
    [System.Serializable]
    public class BlockSpawnDataList
    {
        //�u���b�N�f�[�^
        public List<BlockSpawnData> blockSpawnDataList;
    }
    [System.Serializable]
    public class WaveSpawnBlockList
    {
        //Wave�P�ʂ̃u���b�N�f�[�^
        public List<BlockSpawnDataList> waveBlockSpawnDataList;
    }

    [SerializeField] List<WaveSpawnBlockList> blockSpawnDatas;
    //���������u���b�N
    List<BlockController> blocks;
    //�Q�[���}�l�[�W���[
    GameManager gameManager;
    //�Q�ƃf�[�^
    BlockSpawnData blockSpawnData;
    //���������u���b�N������I�u�W�F�N�g
    [SerializeField] GameObject parentBlock;
    //�E�F�[�u�}�l�[�W���[
    [SerializeField] WaveManager waveManager;


    //�����_�������͎g��Ȃ�
    ////�o�ߎ���
    //float oldSeconds;
    //float spawnTimer;
    ////���݂̃f�[�^�ʒu
    //int spawnDataIndex;


    // ������
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;

        // ���������u���b�N��ۑ�
        blocks = new List<BlockController>();
        //spawnDataIndex = -1;
        SpecifiedSpawn(waveManager.CurrentWave);
    }

    //�w�肵���f�[�^�̃u���b�N�𐶐�
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

    // �S�Ẵu���b�N��Ԃ�
    public List<BlockController> GetBlocks()
    {
        blocks.RemoveAll(bl => !bl);
        return blocks;
    }

    //�u���b�N�������_���Ő������鏈���͖v
    ////�u���b�N����
    //void SpawnBlock()
    //{
    //    //���݂̃f�[�^
    //    if (null == blockSpawnData) return;

    //    //�^�C�}�[����
    //    spawnTimer -= Time.deltaTime;
    //    if (0 < spawnTimer) return;

    //    SpawnNormal();

    //    spawnTimer = blockSpawnData.SpawnDuration;
    //}

    ////����
    //void SpawnNormal()
    //{
    //    // �J�����̃r���[�̍����ƕ����v�Z
    //    float viewportHeight = Camera.main.orthographicSize * 2;
    //    float viewportWidth = viewportHeight * Camera.main.aspect;

    //    // �J�����̍���[�ƉE��[�̈ʒu���v�Z
    //    Vector2 cameraTopLeft = Camera.main.transform.position - new Vector3(viewportWidth / 2, viewportHeight / 2, 0);
    //    Vector2 cameraTopRight = cameraTopLeft + new Vector2(viewportWidth, 0);

    //    // �J�����̏�[�ƒ�����Y���W���v�Z
    //    float cameraTop = cameraTopLeft.y + viewportHeight; // �J�����̏�[��Y���W
    //    float cameraCenterY = Camera.main.transform.position.y; // �J�����̒�����Y���W

    //    // �I�t�Z�b�g
    //    float offset = 1.0f;
    //    float rightEndOffset = 2.0f;

    //    // �v���n�u�𐶐�����X���W�͈̔͂�����
    //    float minX = cameraTopLeft.x;
    //    float maxX = cameraTopRight.x;

    //    // �J�����̏�[���璆���܂ł̃����_����Y���W������
    //    float randomX;
    //    float randomY;
    //    Vector3 spawnPosition;
    //    bool positionFound = false;

    //    // �d�Ȃ�Ȃ��ʒu��������܂Ń��[�v
    //    int maxAttempts = 100; // ���s�񐔂̏��
    //    int attempts = 0;

    //    do
    //    {
    //        randomX = UnityEngine.Random.Range(minX + offset, gameManager.rightEnd - rightEndOffset);
    //        randomY = UnityEngine.Random.Range(cameraCenterY, cameraTop - offset);

    //        // �X�|�[���ʒu��ݒ�
    //        spawnPosition = new Vector3(randomX, randomY, 0);

    //        // ���̃u���b�N�Əd�Ȃ�Ȃ����`�F�b�N
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
    //        Debug.LogWarning("�d�Ȃ�Ȃ��ʒu��������܂���ł���");
    //    }
    //}

    //void CreateRandomBlock(Vector3 pos)
    //{
    //    //�f�[�^���烉���_����ID���擾
    //    int rnd = UnityEngine.Random.Range(0, blockSpawnData.BlockIds.Count);
    //    int id = blockSpawnData.BlockIds[rnd];

    //    //TODO �����������A�j���[�V��������(DOTween)

    //    //�u���b�N����
    //    BlockController block = BlockSettings.Instance.CreateBlock(id, gameManager, pos);
    //    blocks.Add(block);
    //}

    ////�o�ߕb���Ńu���b�N�����f�[�^����ꊷ����
    //void UpdateBlockSpawnData()
    //{
    //    //�o�ߕb�����Ⴄ�ꍇ
    //    if (oldSeconds == gameManager.oldSeconds) return;

    //    // 1��̃f�[�^���Q��
    //    int idx = spawnDataIndex + 1;

    //    // �f�[�^�̍Ō�
    //    if (blockSpawnDatas.Count - 1 < idx) return;

    //    // �ݒ肳�ꂽ�o�ߎ��Ԃ𒴂��Ă�����f�[�^����ꊷ���� TODO 0�̕����̓X�e�[�W
    //    BlockSpawnData data = blockSpawnDatas[0].waveBlockSpawnDataList[0].blockSpawnDataList[idx];
    //    int elapsedSeconds = data.ElapsedMinutes * 60 + data.ElapsedSeconds;

    //    if (elapsedSeconds < gameManager.gameTimer)
    //    {
    //        blockSpawnData = blockSpawnDatas[0].waveBlockSpawnDataList[0].blockSpawnDataList[idx];

    //        // ����p�̐ݒ�
    //        spawnDataIndex = idx;
    //        spawnTimer = 0;
    //        oldSeconds = gameManager.oldSeconds;
    //    }
    //}
}
