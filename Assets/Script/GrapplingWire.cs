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
    bool isLine;

    [SerializeField, Tooltip("ワイヤーターゲットを見つける範囲")] float m_radius = 3f;
    [SerializeField, Tooltip("その範囲")] Transform m_center = default;
    [SerializeField, Tooltip("プレイヤーPosのオフセット")] Vector3 m_playerOffset = Vector3.zero;
    [SerializeField, Tooltip("Anchorの位置")] GameObject m_anchor = default;
    [SerializeField, Tooltip("springJointコンポーネントをセット")] ConfigurableJoint m_joint = default;
    [SerializeField, Tooltip("ターゲットのレイヤー")] LayerMask m_targetLayer = default;
    [SerializeField, Tooltip("壁のレイヤー")] LayerMask m_wallLayer = default;
    [SerializeField, Tooltip("ロックオンImage")] Image m_image = default;
    [SerializeField] LineRenderer m_line = default;
    [SerializeField] float m_float;

    private void Start()
    {
        m_anchor.SetActive(false);
        m_image.enabled = false;
    }
    private void Update()
    {
        Selected();

        if(m_hitTarget)
        {
            m_image.enabled = true;
            m_image.transform.position = m_hitTarget.transform.position;
            Debug.DrawLine(this.transform.position + m_playerOffset, m_hitTarget.transform.position, Color.black);
        }
        else
        {
            m_image.enabled = false;
        }

        if (Input.GetButtonDown("Fire2"))
            Grapple();

        if (m_joint.linearLimit.contactDistance >= Vector3.Distance(this.transform.position + m_playerOffset, m_anchor.transform.position))
        {
            m_anchor.SetActive(false);
            isLine = false;
        }

        if(isLine)
        {
            m_line.SetPosition(0, this.transform.position + m_playerOffset);
            m_line.SetPosition(1, m_anchor.transform.position);
        }
        else
        {
            m_line.SetPosition(0, Vector3.zero);
            m_line.SetPosition(1, Vector3.zero);
        }
    }
    private void LateUpdate()
    {
        m_image.transform.rotation = Camera.main.transform.rotation;
    }
    void Selected()
    {
        m_targets = Physics.OverlapSphere(m_center.position, m_radius, m_targetLayer);
        m_hitTarget = m_targets.Where(go => !Physics.Linecast(this.transform.position + m_playerOffset, go.transform.position, m_wallLayer)).OrderBy(go => Vector3.Distance(m_center.position, go.transform.position)).FirstOrDefault();
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
            isLine = true;
        }
    }
}
