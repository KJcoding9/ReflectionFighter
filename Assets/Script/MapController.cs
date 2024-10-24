using UnityEngine;

//マップ管理スクリプト
public class MapController : MonoBehaviour
{
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
