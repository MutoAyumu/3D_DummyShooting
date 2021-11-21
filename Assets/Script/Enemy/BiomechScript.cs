using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BiomechScript : EnemyAiBase
{
    Animator m_anim;
    NavMeshAgent m_agent;

    [System.NonSerialized] public Rigidbody m_rb;
    [System.NonSerialized] public CapsuleCollider m_collider;
    GameManager m_gmanager;

    PlayerMoveController m_player;
    bool isOn;
    bool isInstance;
    [SerializeField] GameObject[] m_childrens = default;
    [SerializeField] Transform[] m_childrenPos = default;
    [SerializeField] int m_childrenNum = 1;
    float m_timer;
    [SerializeField] float m_setTime = 20f;

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
        m_timer += Time.deltaTime;//タイマーを動かして

        if (m_timer >= m_setTime)//時間になったら
        {
            AiState = EnemyAiState.Wait;
            m_timer = 0;
            isOn = true;
        }

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>();
        m_agent.SetDestination(m_player.transform.position);
    }
    public override void Wait()
    {
        m_agent.isStopped = true;
        Instance();

        if (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("StartMotion"))
        {
            AiState = EnemyAiState.Move;
            isInstance = false;
            isOn = false;
        }
    }
    public override void Move()
    {
        if (m_agent.remainingDistance <= m_agent.stoppingDistance && !isOn)
        {
            m_anim.SetTrigger("IsAttack");
            AiState = EnemyAiState.Attack;
        }

        m_agent.isStopped = false;
    }
    public override void Attack()
    {
        m_agent.isStopped = true;

        if (!m_anim.GetCurrentAnimatorStateInfo(0).IsName("AttackMotion"))
        {
            m_agent.isStopped = false;
            AiState = EnemyAiState.Move;
        }
    }
    public void Instance()
    {
        if (m_childrens.Length != 0 && !isInstance)
        {
            m_anim.SetTrigger("IsInstance");
            isInstance = true;

            for (int i = 0; i < m_childrenNum; i++)
            {
                Instantiate(m_childrens[i], m_childrenPos[i].transform.position, Quaternion.identity);
            }
        }
    }
    public override void Idle()
    {

    }

    void OnEnable()
    {
        if (m_gmanager)
            m_gmanager.m_enemysList.Add(this.gameObject);
    }
    void OnDisable()
    {
        m_gmanager.m_enemysList.Remove(this.gameObject);
    }
}
