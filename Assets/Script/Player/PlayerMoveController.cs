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
    EnemyAI m_enemy;
    GameManager m_gmanager;

    Quaternion m_rotation;
    float m_h;
    float m_v;
    Vector3 mtf;
    Vector3 etf;

    [Header("動き")]
    [SerializeField, Tooltip("ジャンプの移動値")] float m_jumpMovePower = 5f;
    float m_dush;
    [Space(10)]
    [Header("各種設定")]
    [SerializeField, Tooltip("回転の滑らかさ")] float rotationSpeed = 7f;
    [SerializeField, Tooltip("攻撃力")] float m_attackPower = 1f;
    [SerializeField, Tooltip("ダメージを与える敵のタグ")] string m_enemyTag = "Enemy";
    [SerializeField, Tooltip("接地判定のレイヤー")] LayerMask m_groundLayer;
    [SerializeField, Tooltip("壁判定のレイヤー")] LayerMask m_wallLayer;
    [SerializeField, Tooltip("Linecastの高さ")] float m_rayHeight = 2;
    [SerializeField, Tooltip("Rayの方向")] Vector3 m_rayDir = Vector3.zero;
    bool isGround;
    [SerializeField, Tooltip("マウスカーソルの表示非表示")] bool m_mouseCursor;
    [SerializeField] public GameObject m_target;
    Collider targetCollider;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_gmanager = GameObject.FindObjectOfType<GameManager>();

        m_anim.keepAnimatorControllerStateOnDisable = true;
    }

    private void Start()
    {

    }
    private void Update()
    {
        InputMove();
        UpdateMove();
        IsGround();


        EnemyFocus();


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
            m_dush = 1;
        else
            m_dush = 0;

        //攻撃のアニメーションを流す
        if (Input.GetButtonDown("Fire1") && isGround)
        {
            //if (m_target && !Physics.Linecast(mtf, etf, m_wallLayer))
            //{
            //    Vector3 dir = m_target.transform.position;
            //    dir.y = this.transform.position.y;
            //    this.transform.LookAt(dir);
            //}

            m_anim.SetBool("Attacking", true);
            m_anim.SetTrigger("Attack");
            m_anim.SetInteger("AttackNum", 0);
        }

        // ジャンプの入力を取得し、接地している時に押されていたらジャンプする
        if (Input.GetButtonDown("Jump") && isGround)
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, m_jumpMovePower, m_rb.velocity.z);
        }
    }
    void UpdateMove()
    {
        if (m_move != Vector3.zero && !m_anim.GetBool("Attacking"))
        {
            m_move = Camera.main.transform.TransformDirection(m_move);    // カメラのローカル座標に変換する
            m_move.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(m_move);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);  // Slerp を使うのがポイント

            Vector3 dir = m_move.normalized * m_jumpMovePower; // 入力した方向に移動する
            dir.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            m_rb.velocity = dir;   // 計算した速度ベクトルをセットする
        }
        else if (!m_anim.GetBool("Air"))
        {
            Vector3 dir = Vector3.zero;
            dir.y = m_rb.velocity.y;
            m_rb.velocity = dir;
        }

        //パラメータにXYの値を入れる
        m_anim.SetFloat("X", Mathf.Abs(m_h) + m_dush, 0.1f, Time.deltaTime);
        m_anim.SetFloat("Y", Mathf.Abs(m_v) + m_dush, 0.1f, Time.deltaTime);
    }

    void EnemyFocus()
    {
        if (m_target != null)
        {
            mtf = this.transform.position;
            mtf.y = m_rayHeight;
            etf = m_target.transform.position;
            etf.y = m_rayHeight;
            Debug.DrawLine(mtf, etf, Color.red);

            if (!Physics.Linecast(mtf, etf,m_wallLayer))
            {
                m_target.TryGetComponent(out targetCollider);

                foreach (var smb in m_anim.GetBehaviours<MatchPositionSMB>())
                {
                    smb.target = this;
                }
            }
            else
            {
                foreach (var smb in m_anim.GetBehaviours<MatchPositionSMB>())
                {
                    smb.target = null;
                }
            }
        }
        else
        {
            foreach (var smb in m_anim.GetBehaviours<MatchPositionSMB>())
            {
                smb.target = null;
            }
        }
    }

    void IsGround()
    {
        var mtf = this.transform.position;
        isGround = Physics.Raycast(mtf, m_rayDir, m_rayDir.magnitude, m_groundLayer);
        Debug.DrawRay(mtf, m_rayDir, Color.red);

        if (isGround)
        {
            m_anim.SetBool("Air", false);
            m_anim.applyRootMotion = true;
        }
        else
        {
            m_anim.SetBool("Air", true);
            m_anim.applyRootMotion = false;
        }
    }

    void InputAttack()
    {
        if (m_enemy != null)
        {
            m_enemy.TakeDamage(m_attackPower);
            Vector3 dir = (m_enemy.transform.position - this.transform.position).normalized;
            dir.y = 1;
            m_enemy.m_rb.AddForce(dir * 10, ForceMode.Impulse);
        }
    }

    public Vector3 TargetPosition => targetCollider.ClosestPoint(transform.position);

    private void OnTriggerStay(Collider other)
    {
        //攻撃対象の取得
        if (other.gameObject.CompareTag(m_enemyTag))
        {
            m_enemy = other.gameObject.GetComponent<EnemyAI>();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        //攻撃対象の情報破棄
        if (other.gameObject.CompareTag(m_enemyTag))
        {
            m_enemy = default;
        }
    }
}