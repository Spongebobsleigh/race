using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    // ���W�b�h�{�f�B.
    Rigidbody rigid = null;


    // �J�����̎ԒǐՑ��x.
    [SerializeField, Range(1f, 10f)] float cameraTrackingSpeed = 4f;
    // �J�����������鍂���I�t�Z�b�g.
    [SerializeField, Range(0, 5f)] float cameraLookHeightOffset = 4f;
    // �J�����ʒu.
    [SerializeField] Vector3 tpCameraOffset = new Vector3(0, 4f, -10f);

    // �J����.
    [SerializeField] Camera tpCamera = null;

    //! �}�b�v�p�}�[�J�[�̃g�����X�t�H�[��.
    [SerializeField] Transform mapMarker = null;

    // �}�b�v�p�J������RootTransform.
    [SerializeField] Transform mapCamera = null;

    // ���b�v��.
    public int LapCount = 0;
    // �S�[������.
    public int GoalLap = 2;

    // �t���𔻒肷�邽�߂̃X�C�b�`.
    bool lapSwitch = false;

    // �v���C�X�e�[�g.
    public GameController.PlayState CurrentState = GameController.PlayState.None;

    // ���b�v�C�x���g.
    public UnityEvent LapEvent = new UnityEvent();
    // �S�[�����C�x���g,
    public UnityEvent GoalEvent = new UnityEvent();

    // �J�n���ʒu.
    Vector3 startPosition = Vector3.zero;
    // �J�n���p�x.
    Quaternion startRotation = Quaternion.identity;

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
        MoveMapMarker();
        MoveMapCamera();
    }

    // ------------------------------------------------------------
    /// <summary>
    /// �ړ�����.
    /// </summary>
    // ------------------------------------------------------------
    void MoveUpdate()
    {
        if (CurrentState != GameController.PlayState.Play) return;

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
        if (CurrentState != GameController.PlayState.Play) return;

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

    // ------------------------------------------------------------
    /// <summary>
    /// �O���Q�[�g�R�[��.
    /// </summary>
    // ------------------------------------------------------------
    public void OnFrontGateCall()
    {
        // �ʏ�̃Q�[�g�ʉ�.
        if (lapSwitch == true)
        {
            LapCount++;
            Debug.Log("Lap " + LapCount);
            lapSwitch = false;
            if (LapCount > GoalLap) OnGoal();
            else LapEvent?.Invoke();
        }
        // �t���Q�[�g�ʉ�.
        else
        {
            LapCount--;
            if (LapCount < 0) LapCount = 0;
            Debug.Log("�t�� Lap " + LapCount);
            LapEvent?.Invoke();
        }
    }

    // ------------------------------------------------------------
    /// <summary>
    /// ����Q�[�g�R�[��.
    /// </summary>
    // ------------------------------------------------------------
    public void OnBackGateCall()
    {
        if (lapSwitch == false)
        {
            lapSwitch = true;
        }
    }

    public void OnGoal()
    {
        LapCount = 0;
        Debug.Log("Goal!!");
        CurrentState = GameController.PlayState.Finish;
        GoalEvent?.Invoke();
    }

    // ------------------------------------------------------------
    /// <summary>
    /// �}�b�v�p�}�[�J�[���Ԃ̏㕔�ɔz�u.
    /// </summary>
    // ------------------------------------------------------------
    void MoveMapMarker()
    {
        if (mapMarker == null) return;

        var current = this.transform.position;
        current.y = mapMarker.transform.position.y;
        mapMarker.transform.position = current;
    }

    // ------------------------------------------------------------
    /// <summary>
    /// �}�b�v�p�J�����̈ʒu�p�x����.
    /// </summary>
    // ------------------------------------------------------------
    void MoveMapCamera()
    {
        if (mapCamera == null) return;

        var current = this.transform.position;
        current.y = mapCamera.position.y;
        mapCamera.position = current;

        mapCamera.forward = this.transform.forward;
    }

    // ------------------------------------------------------------
    /// <summary>
    /// �J�E���g�_�E���X�^�[�g���R�[��.
    /// </summary>
    // ------------------------------------------------------------
    public void OnStart()
    {
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
    }

    // ------------------------------------------------------------
    /// <summary>
    /// ���g���C���R�[��.
    /// </summary>
    // ------------------------------------------------------------
    public void OnRetry()
    {
        this.transform.position = startPosition;
        this.transform.rotation = startRotation;

        var rotOffset = this.transform.rotation * tpCameraOffset;
        var anchor = this.transform.position + rotOffset;
        tpCamera.gameObject.transform.position = anchor;
    }
}