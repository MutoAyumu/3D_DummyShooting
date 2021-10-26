using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [System.NonSerialized]public Rigidbody m_rb;
    Animator m_anim;
    [System.NonSerialized]public CapsuleCollider m_collider;
    GameManager m_gmanager;
    NavMeshAgent m_agent;
    [SerializeField] PlayerMoveController m_player;

    [Header("HP")]
    [SerializeField, Tooltip("HPの値")] float m_hp = 3f;
    float m_currentHp;
    [SerializeField, Tooltip("HPを表示するスライダー")] Slider m_hpSlider = default;
    [Space(10)]
    [Header("動き")]
    [SerializeField] float m_moveSpeed = 5f;

    private void Awake()
    {
        m_gmanager = GameObject.FindObjectOfType<GameManager>();
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_collider = GetComponent<CapsuleCollider>();
        m_agent = GetComponent<NavMeshAgent>();
        m_currentHp = m_hp;
        m_hpSlider.value = 1;
    }
    private void Start()
    {

    }
    private void Update()
    {
        m_agent.SetDestination(m_player.transform.position);
    }
    private void LateUpdate()
    {
        m_hpSlider.transform.rotation = Camera.main.transform.rotation;
    }
    public void TakeDamage(float damage)
    {
        if (m_currentHp > 1)
        {
            Debug.Log("hit");
            m_currentHp -= damage;
            m_hpSlider.value = m_currentHp / m_hp;
            m_anim.SetTrigger("TakeDamage");
        }
        else
        {
            m_currentHp -= damage;
            m_hpSlider.value = m_currentHp / m_hp;
            m_anim.SetTrigger("Death");
            m_gmanager.m_enemysList.Remove(this.gameObject);
            m_gmanager.m_enemysList.Sort();
            Destroy(this.gameObject);
        }
    }
    private void OnEnable()
    {
        if(m_gmanager)
        m_gmanager.m_enemysList.Add(this.gameObject);
    }
    private void OnDisable()
    {
        m_gmanager.m_enemysList.Remove(this.gameObject);
        m_gmanager.m_enemysList.Sort();
    }
}
