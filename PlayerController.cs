using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 前方移動力.
    [SerializeField] float movePower = 20000f;
    // 横回転力.
    [SerializeField] float rotPower = 30000f;
    // 速度制限.
    [SerializeField] float speedSqrLimit = 200f;
    // 回転速度制限.
    [SerializeField] float rotationSqrLimit = 0.5f;
    // カメラの車追跡速度.
    [SerializeField, Range(1f, 10f)] float cameraTrackingSpeed = 4f;
    // カメラを向ける高さオフセット.
    [SerializeField, Range(0, 5f)] float cameraLookHeightOffset = 4f;
    // カメラ位置.
    [SerializeField] Vector3 tpCameraOffset = new Vector3(0, 4f, -10f);

    // カメラ.
    [SerializeField] Camera tpCamera = null;

    // リジッドボディ.
    Rigidbody rigid = null;

    void Start()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        MoveUpdate();
        RotationUpdate();
        TrackingCameraUpdate();
    }

    // ------------------------------------------------------------
    /// <summary>
    /// 移動処理.
    /// </summary>
    // ------------------------------------------------------------
    void MoveUpdate()
    {
        float sqrVel = rigid.velocity.sqrMagnitude;
        // 前速度制限.
        if (sqrVel > speedSqrLimit) return;

        if (Input.GetKey(KeyCode.Space) == true)
        {
            rigid.AddForce(transform.forward * movePower, ForceMode.Force);
        }

        // 後速度制限.
        if (sqrVel > (speedSqrLimit * 0.2f)) return;

        if (Input.GetKey(KeyCode.S) == true)
        {
            rigid.AddForce(-transform.forward * movePower, ForceMode.Force);
        }
    }

    // ------------------------------------------------------------
    /// <summary>
    /// 回転処理.
    /// </summary>
    // ------------------------------------------------------------
    void RotationUpdate()
    {
        float sqrAng = rigid.angularVelocity.sqrMagnitude;
        // 回転速度制限.
        if (sqrAng > rotationSqrLimit) return;

        if (Input.GetKey(KeyCode.A) == true)
        {
            rigid.AddTorque(-transform.up * rotPower, ForceMode.Force);
        }

        if (Input.GetKey(KeyCode.D) == true)
        {
            rigid.AddTorque(transform.up * rotPower, ForceMode.Force);
        }
    }

    // ------------------------------------------------------------
    /// <summary>
    /// カメラの車追跡.
    /// </summary>
    // ------------------------------------------------------------
    void TrackingCameraUpdate()
    {
        // オフセット値を現在の角度で回転.
        var rotOffset = this.transform.rotation * tpCameraOffset;
        // 現在の位置の値に算出したオフセット値をプラスしてカメラの位置を算出.
        var anchor = this.transform.position + rotOffset;
        // カメラの位置を現在位置から徐々に変更.
        tpCamera.gameObject.transform.position = Vector3.Lerp(tpCamera.gameObject.transform.position, anchor, Time.fixedDeltaTime * cameraTrackingSpeed);

        // カメラを車の方向に向ける.
        var look = this.transform.position;
        look.y += cameraLookHeightOffset;
        tpCamera.gameObject.transform.LookAt(look);
    }
}