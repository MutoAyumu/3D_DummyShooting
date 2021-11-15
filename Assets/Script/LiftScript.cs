using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    [SerializeField, Tooltip("ポイント")] Transform[] m_points = default;
    [SerializeField, Tooltip("スピード")] float m_moveSpeed = 1f;
    [SerializeField, Tooltip("移動先を切り替えるときのポイントとの距離")] float m_stopDistance = 0.2f;

    Vector3 m_move;
    //Rigidbody m_rb;
    int m_count;

    private void Awake()
    {
        //m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Patrol();   
    }
    void Patrol()
    {
        float distance = Vector3.Distance(this.transform.position, m_points[m_count].position);

        if (distance > m_stopDistance)
        {
            m_move = (m_points[m_count].transform.position - this.transform.position);
            //m_rb.velocity = new Vector3(m_move.x, m_move.y, m_move.z).normalized * m_moveSpeed;
            this.transform.Translate(m_move.normalized * m_moveSpeed * Time.deltaTime);
        }
        else
        {
            m_count = (m_count + 1) % m_points.Length;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_count = (m_count + 1) % m_points.Length;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(gameObject.transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
