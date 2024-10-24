using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;


[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]

//�v���C���[�Z�b�e�B���O
public class PlayerSettings : ScriptableObject
{
    string playerDataSave = "PlayerData.json";
    string playerDataFilePath;

    // �L�����N�^�[�f�[�^
    public List<PlayerData> datas;

    public static PlayerSettings instance;

    public static PlayerSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<PlayerSettings>(nameof(PlayerSettings));
            }

            return instance;
        }
    }

    //���X�g��ID����f�[�^����������
    public PlayerData Get(int id)
    {
        return (PlayerData)datas.Find(data => data.Id == id).GetCopy();
    }

    //�v���C���[����
    public PlayerController CreatePlayer(int id, GameManager gameManager, BallController ballController)
    {

        playerDataFilePath = Application.dataPath + "/" + playerDataSave;
        PlayerData data;


        // �����t�@�C�����������炻������ǂݍ���(�L�����Q�l�ȏ�Ȃ�v�ύX)
        //�L�����N�^�[�ǉ��̏ꍇ��(ID����v���Ă�����̂ɉ����ēǂݍ��ޕK�v������)
        if (File.Exists(playerDataFilePath))
        {
            data = Utils.JsonLoad<PlayerData>((playerDataFilePath));
            Debug.Log("json����ǂݎ��");
        }
        else
        {
            //�X�e�[�^�X�擾
            data = Instance.Get(id);
            Debug.Log("settings����ǂݎ��");
        }

        Debug.Log("Prefab Path: " + data.prefabPath);
        Debug.Log("Prefab: " + data.Prefab);

        //�v���n�u���p�X���烍�[�h
        data.LoadPrefab();
        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(data.Prefab, new Vector3(-3f, -5f, 0f), Quaternion.identity);
        //�f�[�^�Z�b�g
        PlayerController ctrl = obj.GetComponent<PlayerController>();
        Debug.Log(ctrl);
        ctrl.Init(gameManager, data, ballController);

        return ctrl;
    }
    //�X�e�[�^�X�U�蕪��
    public void StatAllocation(string type,PlayerData data)
    {
        SoundManager.Instance.PlaySE(23);
        playerDataFilePath = Application.dataPath + "/" + playerDataSave;
        data.BonusPoint--;
        switch (type)
        {
            case "HP":
                data.MaxHP += 3;
                break;
            case "ATK":
                data.Attack++;
                break;
            case "DEF":
                data.Defense++;
                break;
            default:
                break;
        }
        Utils.JsonSave<PlayerData>(data, playerDataFilePath);
    }
}
//�v���C���[�f�[�^
[Serializable]
public class PlayerData : CharacterData
{
    //�U������L������
    public float AttackActivationTime;
    //�`���[�W����
    public float ChargeRate;
    //�`���[�W�\��
    public float MaxCharge;
    //�{�[�����x������
    public float DecayRate;
    //�U�����̃N�[���^�C��
    public float AttackCoolTime;
    //���x��
    public int Lv;
    //�o���l
    public int Exp;
    //�K�v�o���l
    public int NeedExp;
    //�{�[�i�X�|�C���g
    public int BonusPoint;

    // �v���n�u�̃p�X��ۑ�
    public string prefabPath;

    // �v���n�u�𓮓I�Ƀ��[�h���郁�\�b�h
    public void LoadPrefab()
    {
        if (!string.IsNullOrEmpty(prefabPath))
        {
            Prefab = Resources.Load<GameObject>(prefabPath);
        }
    }
}
