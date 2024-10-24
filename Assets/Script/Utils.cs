using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using UnityEngine.SceneManagement;

//汎用スクリプト
public class Utils
{
    // 秒数を0:00の文字列に変換
    public static string GetTextTimer(float timer)
    {
        int seconds = (int)timer % 60;
        int minutes = (int)timer / 60;
        return minutes.ToString() + ":" + seconds.ToString("00");
    }
    // アルファ値設定
    public static void SetAlpha(Graphic graphic, float alpha)
    {
        // 元のカラー
        Color color = graphic.color;
        // アルファ値設定
        color.a = alpha;
        graphic.color = color;
    }
    // アルファ値設定（ボタン）
    public static void SetAlpha(Button button, float alpha)
    {
        // ボタン自体
        SetAlpha(button.image, alpha);
        // 子オブジェクト全て
        foreach (var item in button.GetComponentsInChildren<Graphic>())
        {
            SetAlpha(item, alpha);
        }
    }
    //アルファ値設定（マテリアル)
    public static void SetAlpha(Material material, float alpha)
    {
        // マテリアルの色を取得
        Color color = material.color;
        //アルファ値設定
        color.a = alpha;
        material.color = color;
    }
    //点滅させる
    public static void FlashScreen(Image image,float setAlpha,float flashDuration,int flashCount)
    {
        Sequence flashSequence = DOTween.Sequence();
        flashSequence.Append(image.DOFade(setAlpha, flashDuration / 2)) // アルファ値を1にして点滅させる
                     .Append(image.DOFade(0, flashDuration / 2)) // アルファ値を0にして戻す
                     .SetLoops(flashCount, LoopType.Yoyo); // 回数分繰り返す
    }
    //小数点をパーセンテージに置換
    public static string ConvertToPercentage(float value)
    {
        //パーセント表記に変換
        int percentage = Mathf.RoundToInt(value * 100);
        return percentage.ToString() + "%";
    }
    //jsonファイル読み込み
    public static T JsonLoad<T>(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<T>(json);
    }
    //jsonでデータ保存
    public static void JsonSave<T>(T data, string filepath)
    {
        string json = JsonUtility.ToJson(data);
        using (StreamWriter writer = new StreamWriter(filepath, false))
        {
            writer.WriteLine(json);
        }
    }
    //シーンに戻る
    public static void GoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
