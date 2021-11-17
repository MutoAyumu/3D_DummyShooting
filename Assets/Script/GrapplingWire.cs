using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GrapplingWire : MonoBehaviour
{
    Collider[] m_targets = default;
    Collider m_hitTarget = default;
    RaycastHit m_hitPos;

    [SerializeField, Tooltip("ワイヤーターゲットを見つける範囲")] float m_radius = 3f;
    [SerializeField, Tooltip("その範囲")] Transform m_center = default;
    [SerializeField, Tooltip("プレイヤーの位置")] Transform m_playerPos = default;
    [SerializeField, Tooltip("Anchorの位置")] GameObject m_anchor = default;
    [SerializeField, Tooltip("springJointコンポーネントをセット")] SpringJoint m_joint = default;
    [SerializeField, Tooltip("ターゲットのレイヤー")] LayerMask m_targetLayer = default;
    [SerializeField, Tooltip("壁のレイヤー")] LayerMask m_wallLayer = default;
    [SerializeField, Tooltip("ロックオンImage")] Image m_image = default;
    [SerializeField] float m_float;

    private void Start()
    {
        m_anchor.SetActive(false);
        m_image.enabled = false;
    }
    private void Update()
    {
        Selected();
        if (Input.GetButtonDown("Fire2"))
            Grapple();

        if (m_joint.minDistance >= Vector3.Distance(m_playerPos.position, m_anchor.transform.position))
        {
            m_anchor.SetActive(false);
            m_image.enabled = false;
        }
    }
    private void LateUpdate()
    {
        m_image.transform.rotation = Camera.main.transform.rotation;
    }
    void Selected()
    {
        m_targets = Physics.OverlapSphere(m_center.position, m_radius, m_targetLayer);
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
            m_anchor.SetActive(true);
            m_anchor.transform.position = m_hitTarget.transform.position;
            m_image.enabled = true;
            m_image.transform.position = m_hitTarget.transform.position;
        }
    }
}
