using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    Animator m_anim;
    NavMeshAgent m_agent;

    [System.NonSerialized] public Rigidbody m_rb;
    [System.NonSerialized] public CapsuleCollider m_collider;
    GameManager m_gmanager;

    [SerializeField, Tooltip("HPの値")] float m_hp = 3f;
    float m_currentHp;
    [SerializeField, Tooltip("HPを表示するスライダー")] Slider m_hpSlider = default;
    [SerializeField, Tooltip("遷移の時間")] float m_transitionTime = 1f;
    EnemyAiState m_aiState = EnemyAiState.Wait;
    PlayerMoveController m_player;
    bool isOn;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
        m_gmanager = GameObject.FindObjectOfType<GameManager>();
        m_collider = GetComponent<CapsuleCollider>();
        m_currentHp = m_hp;
        m_hpSlider.value = 1;
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
        if (m_player)
        {
            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                m_anim.SetTrigger("Attack");
            }
        }

        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            m_agent.isStopped = true;
        }
        else
        {
            m_agent.isStopped = false;
        }

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
    private void LateUpdate()
    {
        m_hpSlider.transform.rotation = Camera.main.transform.rotation;
    }
    void Wait()
    {

    }
    void Move()
    {
        if (m_player)
            m_agent.SetDestination(m_player.transform.position);
    }
    void Attack()
    {

    }
    void Idle()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isOn)
        {
            isOn = true;
            m_player = other.GetComponent<PlayerMoveController>();
            m_anim.SetTrigger("IsStarted");
            this.transform.LookAt(m_player.transform);
            StartCoroutine(StartMotion());
        }
    }
    IEnumerator StartMotion()
    {
        yield return new WaitForSeconds(3);
        m_anim.SetTrigger("IsMove");
        m_aiState = EnemyAiState.Move;
    }
    public void TakeDamage(float damage)
    {
        m_anim.Play("Damage");
        DOTween.To(() => m_currentHp, x => m_hpSlider.value = x / m_hp, m_currentHp - damage, m_transitionTime);
        m_currentHp -= damage;

        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            m_agent.isStopped = true;
        }
        else
        {
            m_agent.isStopped = false;
        }


        if (m_currentHp <= 0)
        {
            DOTween.To(() => m_currentHp, x => m_hpSlider.value = x / m_hp, m_currentHp - damage, m_transitionTime);
            Destroy(this.gameObject);
            m_gmanager.m_enemysList.Remove(this.gameObject);
            m_gmanager.m_enemysList.Sort();
        }
    }
    private void OnEnable()
    {
        if (m_gmanager)
            m_gmanager.m_enemysList.Add(this.gameObject);
    }
    private void OnDisable()
    {
        m_gmanager.m_enemysList.Remove(this.gameObject);
        m_gmanager.m_enemysList.Sort();
    }
}