using UnityEngine;

//ベースアビリティスクリプト
public class BaseAbility : MonoBehaviour
{
    //親発動装置
    protected BaseAbilityActivator activator;
    //アビリティデータ
    protected AbilityActivatorData data;
    //物理挙動
    protected Rigidbody2D rb2d;

    //初期化
    public void Init(BaseAbilityActivator activator)
    {
        //親発動装置
        this.activator = activator;
        //アビリティデータセット
        this.data = (AbilityActivatorData)activator.Data.GetCopy();
        //物理挙動
        this.rb2d = GetComponent<Rigidbody2D>();
    }
}
