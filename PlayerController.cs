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
}