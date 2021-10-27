using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Animator m_anim;
    NavMeshAgent m_agent;

    [SerializeField]EnemyAiState m_aiState = EnemyAiState.Wait;
    PlayerMoveController m_player;
    [SerializeField] SphereCollider m_searchColloder;
    [SerializeField] float m_angle = 90f;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
    }
    private void Start()
    {

    }
    enum EnemyAiState
    {
        Wait,       //一旦停止
        Move,       //行動
        Attack,     //攻撃
        Idle,       //停止
    }
    private void Update()
    {
        MainRoutine();
        UpdateAI();
    }
    void MainRoutine()
    {

    }
    void UpdateAI()
    {
        switch (m_aiState)
        {
            case EnemyAiState.Wait:
                Wait();
                break;
            case EnemyAiState.Move:
                Move();
                break;
            case EnemyAiState.Attack:
                Attack();
                break;
            case EnemyAiState.Idle:
                Idle();
                break;
        }
    }
    void Wait()
    {

    }
    void Move()
    {
        m_agent.SetDestination(m_player.transform.position);
    }
    void Attack()
    {

    }
    void Idle()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            m_player = other.GetComponent<PlayerMoveController>();

            var playerDirection = other.transform.position - transform.position;
            //　敵の前方からの主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //　サーチする角度内だったら発見
            if (angle <= m_angle)
            {
                m_aiState = EnemyAiState.Move;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_player = null;
            m_aiState = EnemyAiState.Idle;
        }
    }
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -m_angle, 0f) * transform.forward, m_angle * 2f, m_searchColloder.radius);
    }
}
