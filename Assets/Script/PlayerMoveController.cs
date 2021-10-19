using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMoveController : MonoBehaviour, IMatchTarget
{
    Rigidbody m_rb;
    Animator m_anim;
    Vector3 m_move;
    EnemyController m_enemy;

    Quaternion m_rotation;
    float m_h;
    float m_v;

    [Header("動き")]
    [SerializeField, Tooltip("通常のスピード")] float m_moveSpeed = 5f;
    [SerializeField, Tooltip("ダッシュ時のスピード")] float m_dushSpeed = 10f;
    float m_currentSpeed;
    float m_dush;
    [Space(10)]
    [SerializeField, Tooltip("ジャンプの数値")] float m_jumpPower = 5f;
    bool isJump;
    [Header("各種設定")]
    [SerializeField, Tooltip("回転の滑らかさ")] float rotationSpeed = 7f;
    [SerializeField, Tooltip("攻撃力")] float m_attackPower = 1f;
    bool isAttack;
    [SerializeField, Tooltip("ダメージを与える敵のタグ")] string m_enemyTag = "Enemy";
    [SerializeField, Tooltip("接地判定のタグ")] string m_groundTag = "Ground";
    [SerializeField, Tooltip("マウスカーソルの表示非表示")] bool m_mouseCursor;
    [SerializeField] Transform target;
    Collider targetCollider;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();

        target.TryGetComponent(out targetCollider);
        m_anim.keepAnimatorControllerStateOnDisable = true;

        foreach(var smb in m_anim.GetBehaviours<MatchPositionSMB>())
        {
            smb.target = this;
        }
    }

    private void Start()
    {
        
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
        else if (!m_mouseCursor)
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

        // 入力方向のベクトルを組み立てる
        m_move = Vector3.forward * m_v + Vector3.right * m_h;

        if (Input.GetButton("Shift") && (m_h != 0 || m_v != 0))
        {
            //今のスピード値とパラメータを変更
            m_currentSpeed = m_dushSpeed;
            m_dush = 1;
        }
        else
        {
            m_currentSpeed = m_moveSpeed;
            m_dush = 0;
        }

        //攻撃のアニメーションを流す
        if (Input.GetButtonDown("Fire1"))
        {
            m_anim.SetTrigger("Attack");
            m_anim.SetInteger("AttackNum", 0);
        }

        // ジャンプの入力を取得し、接地している時に押されていたらジャンプする
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, m_jumpPower, m_rb.velocity.z);
            m_anim.SetTrigger("Jump");
        }
    }
    void UpdateMove()
    {
        if (!isAttack)
        {
            if (m_move != Vector3.zero)
            {
                m_move = Camera.main.transform.TransformDirection(m_move);    // カメラのローカル座標に変換する
                m_move.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

                // 入力方向に滑らかに回転させる
                Quaternion targetRotation = Quaternion.LookRotation(m_move);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);  // Slerp を使うのがポイント

                Vector3 dir = m_move.normalized * m_currentSpeed; // 入力した方向に移動する
                dir.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
                m_rb.velocity = dir;   // 計算した速度ベクトルをセットする
            }
            else
            {
                Vector3 dir = Vector3.zero;
                dir.y = m_rb.velocity.y;
                m_rb.velocity = dir;
            }

            //パラメータにXYの値を入れる
            m_anim.SetFloat("X", Mathf.Abs(m_h) + m_dush, 0.3f, Time.deltaTime);
            m_anim.SetFloat("Y", Mathf.Abs(m_v) + m_dush, 0.3f, Time.deltaTime);
        }
    }
    void InputAttack()
    {
        if (m_enemy != null)
        {
            m_enemy.TakeDamage(m_attackPower);
        }
    }

    public Vector3 TargetPosition => targetCollider.ClosestPoint(transform.position);

    private void OnTriggerEnter(Collider other)
    {
        //攻撃対象の取得
        if (other.gameObject.CompareTag(m_enemyTag))
        {
            m_enemy = other.gameObject.GetComponent<EnemyController>();
        }
        if (other.gameObject.CompareTag(m_groundTag))
        {
            isJump = false;
            m_anim.SetBool("Air", false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //攻撃対象の情報破棄
        if (other.gameObject.CompareTag(m_enemyTag))
        {
            m_enemy = default;
        }

        if (other.gameObject.CompareTag(m_groundTag))
        {
            isJump = true;
            m_anim.SetBool("Air", true);
        }
    }
}