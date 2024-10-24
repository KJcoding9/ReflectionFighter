using UnityEngine;
using System.Collections.Generic;

//�G�N�X�g���X�e�[�W�Ǘ��X�N���v�g
public class ExtraStageManagerController : MonoBehaviour
{
    [SerializeField] List<GameObject> extraStages;

    void Start()
    {
        StageClose();
        StageOpen();
    }
    //�X�e�[�W�J��
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
    //�X�e�[�W��S���B��
    void StageClose()
    {
        foreach (var stage in extraStages)
        {
            stage.SetActive(false);
        }
    }
}
