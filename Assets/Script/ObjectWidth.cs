using UnityEngine;

public class ObjectWidth : MonoBehaviour
{
    void Start()
    {
        // Renderer �R���|�[�l���g���擾
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // �I�u�W�F�N�g�̃o�E���f�B���O�{�b�N�X�̉������擾
            float width = renderer.bounds.size.x;
        }
    }
}