using UnityEngine;
using System.Collections.Generic;

//エフェクトマネージャー
public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    void Awake()
    {
        // もし無ければセットする
        if (null == Instance)
        { 
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //エフェクト格納用
    [SerializeField] List<GameObject> effectPrefab;
    //エフェクトの親
    public GameObject parentEffect;

    //エフェクト再生
    public void PlayEffect(Transform transform,int id)
    {
        Instantiate(effectPrefab[id],transform);
    }
    //エフェクト再生(反転する場合)
    public void PlayEffect(Transform transform,int id,bool flipped)
    {
        if (flipped)
        {
            Vector2 scale = effectPrefab[id].transform.localScale;
            scale.x *= -1f;
            effectPrefab[id].transform.localScale = scale;
            Instantiate(effectPrefab[id], transform);
        }
        else
        {
            Instantiate(effectPrefab[id], transform);
        }
    }
    //エフェクト(ダメージ)
    public void SpawnAndPlayEffect(Transform transform,int id)
    {
        GameObject effect = Instantiate(effectPrefab[id], transform);
        //生成用プレハブの子要素に指定
        effect.transform.SetParent(parentEffect.transform);

        // エフェクトのスケールを固定値に設定
        Vector3 fixedScale = new Vector3(1.5f, 1.5f, 1.5f); 
        effect.transform.localScale = fixedScale;
    }
    //エフェクト(チャージ)
    public GameObject SpawnAndPlayEffectReturn(Transform transform, int id)
    {
        GameObject effect = Instantiate(effectPrefab[id], transform);
        //生成用プレハブの子要素に指定
        effect.transform.SetParent(parentEffect.transform);

        // エフェクトのスケールを固定値に設定
        Vector3 fixedScale = new Vector3(1.5f, 1.5f, 1.5f); 
        effect.transform.localScale = fixedScale;

        return effect;
    }
}
