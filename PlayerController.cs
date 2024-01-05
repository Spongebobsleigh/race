using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �O���ړ���.
    [SerializeField] float movePower = 20000f;
    // ����]��.
    [SerializeField] float rotPower = 30000f;
    // ���x����.
    [SerializeField] float speedSqrLimit = 200f;
    // ��]���x����.
    [SerializeField] float rotationSqrLimit = 0.5f;
    // �J�����̎ԒǐՑ��x.
    [SerializeField, Range(1f, 10f)] float cameraTrackingSpeed = 4f;
    // �J�����������鍂���I�t�Z�b�g.
    [SerializeField, Range(0, 5f)] float cameraLookHeightOffset = 4f;
    // �J�����ʒu.
    [SerializeField] Vector3 tpCameraOffset = new Vector3(0, 4f, -10f);

    // �J����.
    [SerializeField] Camera tpCamera = null;

    // ���W�b�h�{�f�B.
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
    /// �ړ�����.
    /// </summary>
    // ------------------------------------------------------------
    void MoveUpdate()
    {
        float sqrVel = rigid.velocity.sqrMagnitude;
        // �O���x����.
        if (sqrVel > speedSqrLimit) return;

        if (Input.GetKey(KeyCode.Space) == true)
        {
            rigid.AddForce(transform.forward * movePower, ForceMode.Force);
        }

        // �㑬�x����.
        if (sqrVel > (speedSqrLimit * 0.2f)) return;

        if (Input.GetKey(KeyCode.S) == true)
        {
            rigid.AddForce(-transform.forward * movePower, ForceMode.Force);
        }
    }

    // ------------------------------------------------------------
    /// <summary>
    /// ��]����.
    /// </summary>
    // ------------------------------------------------------------
    void RotationUpdate()
    {
        float sqrAng = rigid.angularVelocity.sqrMagnitude;
        // ��]���x����.
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
    /// �J�����̎Ԓǐ�.
    /// </summary>
    // ------------------------------------------------------------
    void TrackingCameraUpdate()
    {
        // �I�t�Z�b�g�l�����݂̊p�x�ŉ�].
        var rotOffset = this.transform.rotation * tpCameraOffset;
        // ���݂̈ʒu�̒l�ɎZ�o�����I�t�Z�b�g�l���v���X���ăJ�����̈ʒu���Z�o.
        var anchor = this.transform.position + rotOffset;
        // �J�����̈ʒu�����݈ʒu���珙�X�ɕύX.
        tpCamera.gameObject.transform.position = Vector3.Lerp(tpCamera.gameObject.transform.position, anchor, Time.fixedDeltaTime * cameraTrackingSpeed);

        // �J�������Ԃ̕����Ɍ�����.
        var look = this.transform.position;
        look.y += cameraLookHeightOffset;
        tpCamera.gameObject.transform.LookAt(look);
    }
}