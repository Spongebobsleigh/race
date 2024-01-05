using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �O���ړ���.
    [SerializeField] float movePower = 20000f;
    // ����]��.
    [SerializeField] float rotPower = 30000f;
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
    }

    // ------------------------------------------------------------
    /// <summary>
    /// �ړ�����.
    /// </summary>
    // ------------------------------------------------------------
    void MoveUpdate()
    {
        if (Input.GetKey(KeyCode.Space) == true)
        {
            rigid.AddForce(transform.forward * movePower, ForceMode.Force);
        }

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