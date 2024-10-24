using UnityEngine;

//�G�t�F�N�g�Ǘ��X�N���v�g
public class VFXController : MonoBehaviour
{
    private Animator animator;
    public float animationSpeed = 1.0f;
    new Collider2D collider;

    private void Start()
    {
        if (GetComponent<Collider2D>())
        {
            collider = GetComponent<Collider2D>();
        }

        if(animator != null)
        {
            animator.speed = animationSpeed;
        }
    }
    //�A�j���[�V�����̏I�����ɌĂяo�����֐�
    public void OnAnimationEnd()
    {
        // �Q�[���I�u�W�F�N�g���폜
        Destroy(gameObject);
    }
}
