using UnityEngine;

public class ObjectWidth : MonoBehaviour
{
    void Start()
    {
        // Renderer コンポーネントを取得
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // オブジェクトのバウンディングボックスの横幅を取得
            float width = renderer.bounds.size.x;
        }
    }
}