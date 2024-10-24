using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UIElements;

//壁設定スクリプト
public class WallSetup : MonoBehaviour
{
    //カメラ
    [SerializeField] Camera cam;
    //物理マテリアル
    [SerializeField] PhysicsMaterial2D zeroFriction;
    // パネルのRectTransform
    [SerializeField] RectTransform panelRectTransform; 
    //ゲームマネージャー
    [SerializeField] GameManager gameManager;

    private float rightWallPosition;

    //必要に応じてゲームマネージャーに初期化してもらう
    void Start()
    {
        Init();
    }

    public void Init()
    {
        // 画面の境界を取得
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;
        float wallThickness = 2f;

        // 上の壁
        GameObject topWall = new GameObject("TopWall");
        topWall.transform.parent = this.transform;
        BoxCollider2D topWallCollider = topWall.AddComponent<BoxCollider2D>();
        topWallCollider.sharedMaterial = zeroFriction;
        topWallCollider.size = new Vector2(screenWidth + 2 * wallThickness, wallThickness);
        topWall.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y + screenHeight / 2 + wallThickness / 2);
        topWall.tag = "Wall";

        // 左の壁
        GameObject leftWall = new GameObject("LeftWall");
        leftWall.transform.parent = this.transform;
        BoxCollider2D leftWallCollider = leftWall.AddComponent<BoxCollider2D>();
        leftWallCollider.sharedMaterial = zeroFriction;
        leftWallCollider.size = new Vector2(wallThickness, screenHeight + 2 * wallThickness);
        leftWall.transform.position = new Vector2(cam.transform.position.x - screenWidth / 2 - wallThickness / 2, cam.transform.position.y);
        leftWall.tag = "Wall";


        // 右の壁
        GameObject rightWall = new GameObject("RightWall");
        rightWall.transform.parent = this.transform;
        BoxCollider2D rightWallCollider = rightWall.AddComponent<BoxCollider2D>();
        rightWallCollider.sharedMaterial = zeroFriction;
        rightWallCollider.size = new Vector2(wallThickness, screenHeight + 2 * wallThickness);

        // 端に配置するために、RectTransformの左端座標を取得する
        float gameObjectLeftEdge = panelRectTransform.position.x - panelRectTransform.rect.width * panelRectTransform.pivot.x;

        var offset = 0.55f;

        rightWallPosition = gameObjectLeftEdge - wallThickness / 2 - offset;

        rightWall.transform.position = new Vector2(rightWallPosition, cam.transform.position.y);
        rightWall.tag = "Wall";

        gameManager.rightEnd = rightWallPosition;

        // 下の壁(当たったらボール消滅判定)
        GameObject deathWall = new GameObject("DeathWall");
        deathWall.transform.parent = this.transform;
        BoxCollider2D deathWallCollider = deathWall.AddComponent<BoxCollider2D>();
        deathWallCollider.size = new Vector2(screenWidth + 2 * wallThickness, wallThickness);
        deathWall.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y - screenHeight / 2 - wallThickness / 2);
        deathWall.tag = "DeathWall";
        deathWallCollider.isTrigger = true;
    }
}
