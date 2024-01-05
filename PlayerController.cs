using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �O���ړ���.
    [SerializeField] float movePower = 20000f;
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
}