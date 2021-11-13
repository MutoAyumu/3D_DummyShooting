using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField]Rigidbody[] m_childrens = default;
    [SerializeField] float m_power = 1f;
    [SerializeField] Vector3 m_direction = Vector3.zero;

    private void Start()
    {
        var rb = m_childrens[Random.Range(0, m_childrens.Length)];
        rb.AddForce(m_direction * m_power, ForceMode.VelocityChange);
        Debug.Log("on");
        Debug.Log(rb.name);
    }
}
