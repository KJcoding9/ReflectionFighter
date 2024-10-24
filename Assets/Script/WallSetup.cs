using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UIElements;

//�ǐݒ�X�N���v�g
public class WallSetup : MonoBehaviour
{
    //�J����
    [SerializeField] Camera cam;
    //�����}�e���A��
    [SerializeField] PhysicsMaterial2D zeroFriction;
    // �p�l����RectTransform
    [SerializeField] RectTransform panelRectTransform; 
    //�Q�[���}�l�[�W���[
    [SerializeField] GameManager gameManager;

    private float rightWallPosition;

    //�K�v�ɉ����ăQ�[���}�l�[�W���[�ɏ��������Ă��炤
    void Start()
    {
        Init();
    }

    public void Init()
    {
        // ��ʂ̋��E���擾
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;
        float wallThickness = 2f;

        // ��̕�
        GameObject topWall = new GameObject("TopWall");
        topWall.transform.parent = this.transform;
        BoxCollider2D topWallCollider = topWall.AddComponent<BoxCollider2D>();
        topWallCollider.sharedMaterial = zeroFriction;
        topWallCollider.size = new Vector2(screenWidth + 2 * wallThickness, wallThickness);
        topWall.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y + screenHeight / 2 + wallThickness / 2);
        topWall.tag = "Wall";

        // ���̕�
        GameObject leftWall = new GameObject("LeftWall");
        leftWall.transform.parent = this.transform;
        BoxCollider2D leftWallCollider = leftWall.AddComponent<BoxCollider2D>();
        leftWallCollider.sharedMaterial = zeroFriction;
        leftWallCollider.size = new Vector2(wallThickness, screenHeight + 2 * wallThickness);
        leftWall.transform.position = new Vector2(cam.transform.position.x - screenWidth / 2 - wallThickness / 2, cam.transform.position.y);
        leftWall.tag = "Wall";


        // �E�̕�
        GameObject rightWall = new GameObject("RightWall");
        rightWall.transform.parent = this.transform;
        BoxCollider2D rightWallCollider = rightWall.AddComponent<BoxCollider2D>();
        rightWallCollider.sharedMaterial = zeroFriction;
        rightWallCollider.size = new Vector2(wallThickness, screenHeight + 2 * wallThickness);

        // �[�ɔz�u���邽�߂ɁARectTransform�̍��[���W���擾����
        float gameObjectLeftEdge = panelRectTransform.position.x - panelRectTransform.rect.width * panelRectTransform.pivot.x;

        var offset = 0.55f;

        rightWallPosition = gameObjectLeftEdge - wallThickness / 2 - offset;

        rightWall.transform.position = new Vector2(rightWallPosition, cam.transform.position.y);
        rightWall.tag = "Wall";

        gameManager.rightEnd = rightWallPosition;

        // ���̕�(����������{�[�����Ŕ���)
        GameObject deathWall = new GameObject("DeathWall");
        deathWall.transform.parent = this.transform;
        BoxCollider2D deathWallCollider = deathWall.AddComponent<BoxCollider2D>();
        deathWallCollider.size = new Vector2(screenWidth + 2 * wallThickness, wallThickness);
        deathWall.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y - screenHeight / 2 - wallThickness / 2);
        deathWall.tag = "DeathWall";
        deathWallCollider.isTrigger = true;
    }
}
