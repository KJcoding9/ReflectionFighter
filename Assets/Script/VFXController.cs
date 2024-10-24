using UnityEngine;

//エフェクト管理スクリプト
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
    //アニメーションの終了時に呼び出される関数
    public void OnAnimationEnd()
    {
        // ゲームオブジェクトを削除
        Destroy(gameObject);
    }
}
