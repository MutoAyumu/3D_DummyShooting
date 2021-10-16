using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayermoveScript : MonoBehaviour
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
    [Space(10), Header("各種設定")]
    [SerializeField, Tooltip("入力のデッドゾーン")] float m_deadZone = 0.2f;
    [SerializeField, Tooltip("回転の滑らかさ")] float m_rotationSpeed = 7f;
    [SerializeField, Tooltip("攻撃力")] float m_attackPower = 1f;
    [SerializeField, Tooltip("ダメージを与える敵のタグ")] string m_enemyTag = "Enemy";
    [SerializeField, Tooltip("接地判定のタグ")] string m_groundTag = "Ground";
    [SerializeField, Tooltip("マウスカーソルの表示非表示")] bool m_mouseCursor;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
    }
    private void Start()
    {

    }
    private void Update()
    {
        float m_h = Input.GetAxisRaw("Horizontal");
        float m_v = Input.GetAxisRaw("Vertical");

        m_move = new Vector3(m_h, m_rb.velocity.y, m_v);

        //ジャンプの入力
        if (Input.GetButtonDown("Jump"))
        {
            
        }
    }
    private void FixedUpdate()
    {
        //動きのベクトルをCameraのローカル座標系に変更
        m_move = Camera.main.transform.TransformDirection(m_move);
        //Ｙ成分だけを0にする
        m_move.y = 0;
        //移動
        if (m_move != Vector3.zero)
        {
            m_rb.AddForce(m_move.normalized * m_moveSpeed, ForceMode.Force);

            //移動方向を向く
            m_rotation = Quaternion.LookRotation(m_move, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, m_rotation, m_rotationSpeed);


    }
}
