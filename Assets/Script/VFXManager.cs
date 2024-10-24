using UnityEngine;
using System.Collections.Generic;

//�G�t�F�N�g�}�l�[�W���[
public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    void Awake()
    {
        // ����������΃Z�b�g����
        if (null == Instance)
        { 
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //�G�t�F�N�g�i�[�p
    [SerializeField] List<GameObject> effectPrefab;
    //�G�t�F�N�g�̐e
    public GameObject parentEffect;

    //�G�t�F�N�g�Đ�
    public void PlayEffect(Transform transform,int id)
    {
        Instantiate(effectPrefab[id],transform);
    }
    //�G�t�F�N�g�Đ�(���]����ꍇ)
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
    //�G�t�F�N�g(�_���[�W)
    public void SpawnAndPlayEffect(Transform transform,int id)
    {
        GameObject effect = Instantiate(effectPrefab[id], transform);
        //�����p�v���n�u�̎q�v�f�Ɏw��
        effect.transform.SetParent(parentEffect.transform);

        // �G�t�F�N�g�̃X�P�[�����Œ�l�ɐݒ�
        Vector3 fixedScale = new Vector3(1.5f, 1.5f, 1.5f); 
        effect.transform.localScale = fixedScale;
    }
    //�G�t�F�N�g(�`���[�W)
    public GameObject SpawnAndPlayEffectReturn(Transform transform, int id)
    {
        GameObject effect = Instantiate(effectPrefab[id], transform);
        //�����p�v���n�u�̎q�v�f�Ɏw��
        effect.transform.SetParent(parentEffect.transform);

        // �G�t�F�N�g�̃X�P�[�����Œ�l�ɐݒ�
        Vector3 fixedScale = new Vector3(1.5f, 1.5f, 1.5f); 
        effect.transform.localScale = fixedScale;

        return effect;
    }
}
