using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    Rigidbody m_rb;
    Animator m_anim;
    CapsuleCollider m_collider;

    [Header("HP")]
    [SerializeField, Tooltip("HPの値")] float m_hp = 3f;
    float m_currentHp;
    [SerializeField, Tooltip("HPを表示するスライダー")] Slider m_hpSlider = default;
    [Space(10)]
    [Header("動き")]
    [SerializeField] float m_moveSpeed = 5f;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_collider = GetComponent<CapsuleCollider>();
        m_currentHp = m_hp;
        m_hpSlider.value = 1;
    }
    private void Update()
    {
        
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
            Debug.Log("Death");
            m_currentHp -= damage;
            m_hpSlider.value = m_currentHp / m_hp;
            m_anim.SetTrigger("Death");
            m_collider.enabled = false;
            m_rb.useGravity = false;
            m_rb.isKinematic = true;
        }
    }
}
