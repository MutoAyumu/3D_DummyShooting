using UnityEngine;
using Cinemachine;
public class AimController : MonoBehaviour
{
    [SerializeField] Transform m_player = default;
    float m_x;
    float m_y;

    [SerializeField] Transform m_pivot = default;
    [SerializeField] Camera m_cam;
    float m_Angle;
    [SerializeField, Range(-0.5f, 0f)] float m_minY = -0.25f;
    [SerializeField, Range(0f, 0.5f)] float m_maxY = 0.25f;

    float m_h;
    float m_v;
    Quaternion m_rotation;
    [SerializeField] float m_speed = 3f;

    Rigidbody m_rb;
    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        InputMove();
        MoveUpdate();
    }
    void InputMove()
    {
        m_x = Input.GetAxis("Mouse Y");
        m_y = Input.GetAxis("Mouse X");

        m_h = Input.GetAxisRaw("Horizontal");
        m_v = Input.GetAxisRaw("Vertical");
    }
    void MoveUpdate()
    {
        m_Angle = m_pivot.localRotation.x;
        m_player.transform.Rotate(0, m_y, 0);

        if (m_x != 0.5f)
        {
            if (0.5f < m_x)
            {
                if (m_minY >= m_pivot.transform.localRotation.x)
                {
                    return;
                }
                else if (m_minY <= m_pivot.transform.localRotation.x)
                {
                    m_pivot.transform.Rotate(-m_x, 0, 0);
                }
            }
            else
            {
                if (m_Angle >= m_maxY)
                {
                    return;
                }
                else if (m_Angle <= m_maxY)
                {
                    m_pivot.transform.Rotate(-m_x, 0, 0);
                }
            }
        }

        m_rotation = Quaternion.AngleAxis(m_cam.transform.eulerAngles.y, Vector3.up);
        Vector3 move = m_rotation * new Vector3(m_h, 0, m_v).normalized;
        m_rb.velocity = move * m_speed;
    }
}