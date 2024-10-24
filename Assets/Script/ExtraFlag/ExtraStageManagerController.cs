using UnityEngine;
using System.Collections.Generic;

//エクストラステージ管理スクリプト
public class ExtraStageManagerController : MonoBehaviour
{
    [SerializeField] List<GameObject> extraStages;

    void Start()
    {
        StageClose();
        StageOpen();
    }
    //ステージ開放
    void StageOpen()
    {
        if (StageSelectManager.Instance.stageFlagData.IsClear[1] == true)
        {
            extraStages[0].SetActive(true);
        }

        if(StageSelectManager.Instance.stageFlagData.IsClear[6] == true)
        {
            extraStages[1].SetActive(true);
        }
    }
    //ステージを全部隠す
    void StageClose()
    {
        foreach (var stage in extraStages)
        {
            stage.SetActive(false);
        }
    }
}
