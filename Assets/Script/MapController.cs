using UnityEngine;

//�}�b�v�Ǘ��X�N���v�g
public class MapController : MonoBehaviour
{
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
