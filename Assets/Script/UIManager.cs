using UnityEngine;

//UI�Ǘ��X�N���v�g
public class UIManager : MonoBehaviour
{
    //���j���[�p�l��
    [SerializeField] GameObject menuPanel;

    void Start()
    {
        menuPanel.SetActive(false);
    }
}
