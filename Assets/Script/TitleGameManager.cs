using UnityEngine;
using UnityEngine.SceneManagement;

//�^�C�g���Ǘ��X�N���v�g
public class TitleGameManager : MonoBehaviour
{
    //�Q�[���X�^�[�g
    public void OnClickStart()
    {
        SoundManager.Instance.PlaySE(0);
        // �Q�[���V�[����
        SceneManager.LoadScene("FirstMapScene");
    }
    //�Q�[���I��
    public void QuitGame()
    {
        // �r���h��̃A�v���P�[�V�������I��
        Application.Quit();

        // Unity�G�f�B�^��ł̓���m�F�p
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
