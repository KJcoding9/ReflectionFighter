using UnityEngine;


//プレイヤーの矢印操作(ボール発射位置決定)スクリプト
public class ArrowController : MonoBehaviour
{

    // 回転速度
    public float rotationSpeed = 180f; // 90度回転の速度（度数法）
    //現在の角度と移動方向
    public float currentAngle = 0f;
    // 回転方向（1: 正方向, -1: 負方向）
    public int rotationDirection = 1;
    // 角度の制限
    private float maxAngle = 80f;
    private float minAngle = -80f;
    // 中心オブジェクトからの距離
    public float distanceFromCenter = 5.0f;
    //中心オブジェクト(プレイヤー想定)
    public Transform centerObject;

    void Start()
    {
        // 初期の回転を上方向に設定（必要に応じて調整）
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    void FixedUpdate()
    {
        NewArrowMove();
    }

    void NewArrowMove()
    {
        // マウスのスクリーン座標を取得
        Vector3 mouseScreenPosition = Input.mousePosition;

        // マウス座標をワールド座標に変換
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));

        // 中心オブジェクトとマウス位置の差分ベクトルを計算
        Vector3 direction = mouseWorldPosition - centerObject.position;

        // 2DなのでZ軸は無視
        direction.z = 0; 

        // マウス位置に対する角度を計算
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 矢印の向きをマウスの方向に合わせる
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // 矢印が上を基準にするために90度オフセット

        // ワールド座標で距離を計算し、矢印を中心オブジェクトから一定距離に配置
        transform.position = centerObject.position + direction.normalized * distanceFromCenter;
    }

    //旧メソッド(操作性が悪すぎたため没)
    //void ArrowMove()
    //{
    //    if (isInitialRotationSet)
    //    {

    //    }
    //    // 上矢印キーが押されている間のみ回転を行う
    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        // 時間に応じて角度を計算
    //        float angle = Time.deltaTime * rotationSpeed * rotationDirection;

    //        // 現在の角度を更新
    //        currentAngle += angle;

    //        // 角度の制限を超えた場合、回転方向を逆転する
    //        if (currentAngle >= maxAngle)
    //        {
    //            currentAngle = maxAngle;
    //            rotationDirection *= -1;
    //        }
    //        else if (currentAngle <= minAngle)
    //        {
    //            currentAngle = minAngle;
    //            rotationDirection *= -1;
    //        }

    //        // 回転の中心となるオブジェクトの周りで回転する
    //        transform.RotateAround(centerObject.position, Vector3.forward, angle);
    //    }
    //}

}
