using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [System.NonSerialized]public Rigidbody m_rb;
    Animator m_anim;
    [System.NonSerialized]public CapsuleCollider m_collider;
    GameManager m_gmanager;

    [Header("HP")]
    [SerializeField, Tooltip("HPの値")] float m_hp = 3f;
    float m_currentHp;
    [SerializeField, Tooltip("HPを表示するスライダー")] Slider m_hpSlider = default;
    [SerializeField, Tooltip("")] float m_transitionTime = 1f;
    [Space(10)]
    [Header("動き")]
    [SerializeField] float m_moveSpeed = 5f;

    private void Awake()
    {
        m_gmanager = GameObject.FindObjectOfType<GameManager>();
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_collider = GetComponent<CapsuleCollider>();
        m_currentHp = m_hp;
        m_hpSlider.value = 1;
    }
    
    private void LateUpdate()
    {
        m_hpSlider.transform.rotation = Camera.main.transform.rotation;
    }
    public void TakeDamage(float damage)
    {
        m_currentHp -= damage;

        if (m_currentHp > 1)
        {
            DOTween.To(() => m_currentHp, x => m_hpSlider.value = x / m_hp, m_currentHp - damage, m_transitionTime);
            m_anim.SetTrigger("TakeDamage");
        }
        else
        {
            DOTween.To(() => m_currentHp, x => m_hpSlider.value = x / m_hp, m_currentHp - damage, m_transitionTime);
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
