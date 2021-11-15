using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    [SerializeField] EnemyStatus m_status = default;
    [SerializeField, Tooltip("HPを表示するスライダー")] Slider m_hpSlider = default;
    [SerializeField, Tooltip("遷移の時間")] float m_transitionTime = 1f;
    [SerializeField, Tooltip("ラグドール")] GameObject m_ragdoll = default;
    Animator m_anim;
    NavMeshAgent m_agent;
    GameManager m_gmanager;
    float m_currentHp;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        m_gmanager = GameObject.FindObjectOfType<GameManager>();
        m_currentHp = m_status.HP;
        m_hpSlider.value = 1;
    }
    private void LateUpdate()
    {
        m_hpSlider.transform.rotation = Camera.main.transform.rotation;
    }
    public void TakeDamage(float damage)
    {
        m_anim.Play("Damage");
        DOTween.To(() => m_currentHp, x => m_hpSlider.value = x / m_status.HP, m_currentHp - damage, m_transitionTime);
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
            DOTween.To(() => m_currentHp, x => m_hpSlider.value = x / m_status.HP, m_currentHp - damage, m_transitionTime);
            Destroy(this.gameObject);
            m_gmanager.m_enemysList.Remove(this.gameObject);
            m_gmanager.Dead = true;
            Instantiate(m_ragdoll, this.transform.position, Quaternion.identity);
        }
    }
}
