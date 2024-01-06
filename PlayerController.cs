using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    // ラップ数.
    public int LapCount = 0;
    // ゴール周回数.
    public int GoalLap = 2;

    // 逆走を判定するためのスイッチ.
    bool lapSwitch = false;

    // ラップイベント.
    public UnityEvent LapEvent = new UnityEvent();
    // ゴール時イベント,
    public UnityEvent GoalEvent = new UnityEvent();

    // プレイステート.
    public GameController.PlayState CurrentState = GameController.PlayState.None;

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
        if (CurrentState != GameController.PlayState.Play) return;

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
        if (CurrentState != GameController.PlayState.Play) return;

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

    // ------------------------------------------------------------
    /// <summary>
    /// 前方ゲートコール.
    /// </summary>
    // ------------------------------------------------------------
    public void OnFrontGateCall()
    {
        // 通常のゲート通過.
        if (lapSwitch == true)
        {
            if (LapCount > GoalLap) OnGoal();
            else LapEvent?.Invoke();
        }
        // 逆走ゲート通過.
        else
        {
            LapEvent?.Invoke();
        }
    }

    // ------------------------------------------------------------
    /// <summary>
    /// 後方ゲートコール.
    /// </summary>
    // ------------------------------------------------------------
    public void OnBackGateCall()
    {
        if (lapSwitch == false)
        {
            lapSwitch = true;
        }
    }

    // ------------------------------------------------------------
    /// <summary>
    /// ゴール時処理.
    /// </summary>
    // ------------------------------------------------------------
    public void OnGoal()
    {
        LapCount = 0;
        Debug.Log("Goal!!");
        CurrentState = GameController.PlayState.Finish;
        GoalEvent?.Invoke();
    }
}