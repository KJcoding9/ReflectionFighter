using UnityEngine;
using UnityEngine.SceneManagement;

//タイトル管理スクリプト
public class TitleGameManager : MonoBehaviour
{
    //ゲームスタート
    public void OnClickStart()
    {
        SoundManager.Instance.PlaySE(0);
        // ゲームシーンへ
        SceneManager.LoadScene("FirstMapScene");
    }
    //ゲーム終了
    public void QuitGame()
    {
        // ビルド後のアプリケーションを終了
        Application.Quit();

        // Unityエディタ上での動作確認用
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
