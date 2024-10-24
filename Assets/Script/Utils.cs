using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using UnityEngine.SceneManagement;

//�ėp�X�N���v�g
public class Utils
{
    // �b����0:00�̕�����ɕϊ�
    public static string GetTextTimer(float timer)
    {
        int seconds = (int)timer % 60;
        int minutes = (int)timer / 60;
        return minutes.ToString() + ":" + seconds.ToString("00");
    }
    // �A���t�@�l�ݒ�
    public static void SetAlpha(Graphic graphic, float alpha)
    {
        // ���̃J���[
        Color color = graphic.color;
        // �A���t�@�l�ݒ�
        color.a = alpha;
        graphic.color = color;
    }
    // �A���t�@�l�ݒ�i�{�^���j
    public static void SetAlpha(Button button, float alpha)
    {
        // �{�^������
        SetAlpha(button.image, alpha);
        // �q�I�u�W�F�N�g�S��
        foreach (var item in button.GetComponentsInChildren<Graphic>())
        {
            SetAlpha(item, alpha);
        }
    }
    //�A���t�@�l�ݒ�i�}�e���A��)
    public static void SetAlpha(Material material, float alpha)
    {
        // �}�e���A���̐F���擾
        Color color = material.color;
        //�A���t�@�l�ݒ�
        color.a = alpha;
        material.color = color;
    }
    //�_�ł�����
    public static void FlashScreen(Image image,float setAlpha,float flashDuration,int flashCount)
    {
        Sequence flashSequence = DOTween.Sequence();
        flashSequence.Append(image.DOFade(setAlpha, flashDuration / 2)) // �A���t�@�l��1�ɂ��ē_�ł�����
                     .Append(image.DOFade(0, flashDuration / 2)) // �A���t�@�l��0�ɂ��Ė߂�
                     .SetLoops(flashCount, LoopType.Yoyo); // �񐔕��J��Ԃ�
    }
    //�����_���p�[�Z���e�[�W�ɒu��
    public static string ConvertToPercentage(float value)
    {
        //�p�[�Z���g�\�L�ɕϊ�
        int percentage = Mathf.RoundToInt(value * 100);
        return percentage.ToString() + "%";
    }
    //json�t�@�C���ǂݍ���
    public static T JsonLoad<T>(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<T>(json);
    }
    //json�Ńf�[�^�ۑ�
    public static void JsonSave<T>(T data, string filepath)
    {
        string json = JsonUtility.ToJson(data);
        using (StreamWriter writer = new StreamWriter(filepath, false))
        {
            writer.WriteLine(json);
        }
    }
    //�V�[���ɖ߂�
    public static void GoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
