using UnityEngine;

//UI管理スクリプト
public class UIManager : MonoBehaviour
{
    //メニューパネル
    [SerializeField] GameObject menuPanel;

    void Start()
    {
        menuPanel.SetActive(false);
    }
}
