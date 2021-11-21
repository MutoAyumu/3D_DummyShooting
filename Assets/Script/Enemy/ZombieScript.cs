using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieScript : EnemyAiBase
{
    Animator m_anim;
    NavMeshAgent m_agent;

    [System.NonSerialized] public Rigidbody m_rb;
    [System.NonSerialized] public CapsuleCollider m_collider;
    GameManager m_gmanager;

    PlayerMoveController m_player;
    bool isOn;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
        m_gmanager = GameObject.FindObjectOfType<GameManager>();
        m_collider = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {

    }
    private void Update()
    {
        MainRoutine();
        UpdateAI();
    }
    public override void MainRoutine()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>();
        m_agent.SetDestination(m_player.transform.position);
    }
    public override void Wait()
    {

    }
    public override void Move()
    {
        if (m_player)
        {
            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                m_agent.isStopped = true;
                m_anim.SetTrigger("Attack");
                AiState = EnemyAiState.Attack;
            }
        }
    }
    public override void Attack()
    {
        if (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            m_agent.isStopped = false;
            AiState = EnemyAiState.Move;
        }
    }
    public override void Idle()
    {

    }

    IEnumerator StartMotion()
    {
        float waitTime = 3;
        yield return new WaitForSeconds(waitTime);
        m_anim.SetTrigger("IsMove");
        AiState = EnemyAiState.Move;
        m_agent.isStopped = false;
    }

    void OnEnable()
    {
        if (m_gmanager)
            m_gmanager.m_enemysList.Add(this.gameObject);

        if (!isOn)
        {
            isOn = true;
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>();
            m_agent.isStopped = true;
            m_anim.SetTrigger("IsStarted");
            this.transform.LookAt(m_player.transform);
            StartCoroutine(StartMotion());
        }
    }
    void OnDisable()
    {
        m_gmanager.m_enemysList.Remove(this.gameObject);
    }
}
