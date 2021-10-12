using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    Rigidbody m_rb;
    Animator m_anim;
    Vector3 m_move;

    Quaternion m_rotation;
    float m_h;
    float m_v;

    [Header("動き")]
    [SerializeField,Tooltip("通常のスピード")] float m_moveSpeed = 5f;
    [SerializeField,Tooltip("ダッシュ時のスピード")] float m_dushSpeed = 10f;
    float m_currentSpeed;
    [Space(10)]
    [SerializeField, Tooltip("ジャンプの数値")] float m_jumpPower = 5f;
    [Space(10), Header("各種設定")]
    [SerializeField, Tooltip("回転の滑らかさ")]float rotationSpeed = 7f;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_rotation = transform.rotation;
    }
    private void Update()
    {
        InputMove();
        UpdateMove();
    }
    void InputMove()
    {
        //移動入力の取得
        m_h = Input.GetAxis("Horizontal");
        m_v = Input.GetAxis("Vertical");
        m_move = new Vector3(m_h, 0, m_v).normalized;

        if(Input.GetButton("Shift"))
        {
            //今のスピード値とパラメータを変更
            m_currentSpeed = m_dushSpeed;
            m_anim.SetBool("Dush", true);
        }
        else
        {
            m_currentSpeed = m_moveSpeed;
            m_anim.SetBool("Dush", false);
        }

        //マウスカーソルをオンオフさせる
        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void UpdateMove()
    {
        m_move = Camera.main.transform.TransformDirection(m_move);
        m_move.y = 0;

        //移動方向を向く
        if (m_move.magnitude > 0.5f)
        {
            m_rotation = Quaternion.LookRotation(m_move, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_rotation, rotationSpeed);

        //移動のアップデート
        m_rb.velocity = m_move * m_currentSpeed;

        //パラメータにXYの値を入れる
        m_anim.SetFloat("X", m_h);
        m_anim.SetFloat("Y", m_v);
    }
}