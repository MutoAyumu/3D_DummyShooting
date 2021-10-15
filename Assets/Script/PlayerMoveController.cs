using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMoveController : MonoBehaviour
{
    Rigidbody m_rb;
    Animator m_anim;
    Vector3 m_move;
    EnemyController m_enemy;

    Quaternion m_rotation;
    float m_h;
    float m_v;

    [Header("動き")]
    [SerializeField,Tooltip("通常のスピード")] float m_moveSpeed = 5f;
    [SerializeField,Tooltip("ダッシュ時のスピード")] float m_dushSpeed = 10f;
    float m_currentSpeed;
    float m_dush;
    [Space(10)]
    [SerializeField, Tooltip("ジャンプの数値")] float m_jumpPower = 5f;
    bool isJump;
    [Space(10), Header("各種設定")]
    [SerializeField, Tooltip("回転の滑らかさ")]float rotationSpeed = 7f;
    [SerializeField, Tooltip("攻撃力")] float m_attackPower = 1f;
    [SerializeField, Tooltip("ダメージを与える敵のタグ")] string m_enemyTag = "Enemy";
    [SerializeField, Tooltip("接地判定のタグ")] string m_groundTag = "Ground";
    [SerializeField, Tooltip("マウスカーソルの表示非表示")] bool m_mouseCursor;

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

        //マウスカーソルをオンオフさせる
        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(!m_mouseCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void InputMove()
    {
        //移動入力の取得
        m_h = Input.GetAxisRaw("Horizontal");
        m_v = Input.GetAxisRaw("Vertical");
        m_move = new Vector3(m_h, 0, m_v).normalized;

        if(Input.GetButton("Shift") && (m_h != 0 || m_v != 0))
        {
            //今のスピード値とパラメータを変更
            m_currentSpeed = m_dushSpeed;
            m_dush = 1;//あとで三項演算子使う
        }
        else
        {
            m_currentSpeed = m_moveSpeed;
            m_dush = 0;
        }

        //攻撃のアニメーションを流す
        if(Input.GetButtonDown("Fire1"))
        {
            m_anim.SetTrigger("Attack");
        }
    }
    void UpdateMove()
    {
        //動きのベクトルをCameraのローカル座標系に変更
        m_move = Camera.main.transform.TransformDirection(m_move);
        //Ｙ成分だけを0にする
        m_move.y = 0;

        //移動方向を向く
        if (m_move.magnitude > 0.5f)
        {
            m_rotation = Quaternion.LookRotation(m_move, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_rotation, rotationSpeed);

        m_move.y = -1;
        //移動のアップデート
        m_rb.velocity = m_move * m_currentSpeed;

        //パラメータにXYの値を入れる
        m_anim.SetFloat("X", Mathf.Abs(m_h) + m_dush);
        m_anim.SetFloat("Y", Mathf.Abs(m_v) + m_dush);
    }
    void InputAttack()
    {
        if(m_enemy != null)
        {
            m_enemy.TakeDamage(m_attackPower);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        //攻撃対象の取得
        if(other.gameObject.CompareTag(m_enemyTag))
        {
            m_enemy = other.gameObject.GetComponent<EnemyController>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //攻撃対象の情報破棄
        if(other.gameObject.CompareTag(m_enemyTag))
        {
            m_enemy = default;
        }
    }
}