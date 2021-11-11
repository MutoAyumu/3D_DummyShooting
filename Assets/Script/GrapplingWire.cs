using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrapplingWire : MonoBehaviour
{
    Collider[] m_targets = default;
    Collider m_hitTarget = default;
    [SerializeField] float m_radius = 3f;
    [SerializeField] string m_targetTag = "";
    [SerializeField] Transform m_center = default;
    [SerializeField] Transform m_playerPos = default;
    [SerializeField] LayerMask m_objectLayer = default;
    [SerializeField] LayerMask m_wallLayer = default;

    private void Awake()
    {

    }
    private void Update()
    {
        Selected();
        if (Input.GetButtonDown("Fire2"))
            Grapple();
    }
    void Selected()
    {
        m_targets = Physics.OverlapSphere(m_center.position, m_radius, m_objectLayer);
        m_hitTarget = m_targets.Where(go => !Physics.Linecast(m_playerPos.position, go.transform.position, m_wallLayer)).OrderBy(go => Vector3.Distance(m_center.position, go.transform.position)).FirstOrDefault();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_center.position, m_radius);
    }
    void Grapple()
    {
        if (m_hitTarget)
        {
            foreach (var a in m_targets)
                a.GetComponent<Renderer>().material.color = Color.white;
            m_hitTarget.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
