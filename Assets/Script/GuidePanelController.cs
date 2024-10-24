using UnityEngine;
using System.Collections.Generic;

//�K�C�h�p�l�����Ǘ�����X�N���v�g
public class GuidePanelController : MonoBehaviour
{
    //�y�[�W
    enum State
    {
        Guide_1,
        Guide_2, 
        Guide_3, 
        Guide_4, 
        Guide_5
    }
    State guideState;
    //�p�l���I�u�W�F�N�g
    [SerializeField] List<GameObject> guidePanels;

    void Start()
    {
        //�����y�[�W
        guideState = State.Guide_1;
    }

    void Update()
    {
        ShowGuide();
    }

    //�\������
    private void ShowGuide()
    {
        switch (guideState){
            case State.Guide_1:
                DeactivateAllObjects();
                guidePanels[0].SetActive(true);
                break;
            case State.Guide_2:
                DeactivateAllObjects();
                guidePanels[1].SetActive(true);
                break;
            case State.Guide_3:
                DeactivateAllObjects();
                guidePanels[2].SetActive(true);
                break;
            case State.Guide_4:
                DeactivateAllObjects();
                guidePanels[3].SetActive(true);
                break;
            case State.Guide_5:
                DeactivateAllObjects();
                guidePanels[4].SetActive(true);
                break;
        }
    }
    //��\��
    void DeactivateAllObjects()
    {
        foreach (GameObject obj in guidePanels)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    //�{�^���p
    public void SetGuide_1()
    {
        SoundManager.Instance.PlaySE(21);
        guideState = State.Guide_1;
    }
    public void SetGuide_2()
    {
        SoundManager.Instance.PlaySE(21);
        guideState = State.Guide_2;
    }
    public void SetGuide_3()
    {
        SoundManager.Instance.PlaySE(21);
        guideState = State.Guide_3;
    }
    public void SetGuide_4()
    {
        SoundManager.Instance.PlaySE(21);
        guideState = State.Guide_4;
    }
    public void SetGuide_5()
    {
        SoundManager.Instance.PlaySE(21);
        guideState = State.Guide_5;
    }
}
