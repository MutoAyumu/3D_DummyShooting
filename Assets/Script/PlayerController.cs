using UnityEngine;
using Cinemachine;
public class PlayerController : MonoBehaviour
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
    float a = 0;
    Quaternion m_rotation;
    [SerializeField] int m_speed = 3;
    [SerializeField] int m_dashSpeed = 5;
    int m_currentSpeed;

    Rigidbody m_rb;
    Animator m_anim;
    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_currentSpeed = m_speed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        InputMove();
    }
    private void FixedUpdate()
    {
        MoveUpdate();
    }
    void InputMove()
    {
        m_x = Input.GetAxis("Mouse Y");
        m_y = Input.GetAxis("Mouse X");

        m_h = Input.GetAxisRaw("Horizontal");
        m_v = Input.GetAxisRaw("Vertical");

        if(Input.GetButton("Shift") && m_v > 0)
        {
            m_currentSpeed = m_dashSpeed;
            a = 1;
            m_anim.SetBool("Run", true);
        }
        else
        {
            m_currentSpeed = m_speed;
            a = 0;
            m_anim.SetBool("Run", false);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    void MoveUpdate()
    {
        m_Angle = m_pivot.localRotation.x;
        m_player.transform.Rotate(0, m_y, 0);

        if (m_x != 0.5f)
        {
            if (0.5f < m_x)
            {
                if(m_minY >= m_pivot.transform.localRotation.x)
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
                if(m_Angle >= m_maxY)
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
        m_rb.velocity = move * m_currentSpeed;

        //if(m_v == 0 && m_h == 0)
        //{
        //    m_anim.SetBool("Run", false);
        //    m_anim.SetBool("Back", false);
        //    m_anim.SetBool("Right", false);
        //    m_anim.SetBool("Left", false);
        //    m_anim.SetInteger("Speed", 0);
        //}
        //else if(m_v > 0 && m_currentSpeed == m_dashSpeed)
        //{
        //    m_anim.SetBool("Right", false);
        //    m_anim.SetBool("Left", false);
        //    m_anim.SetBool("Run", true);
        //    m_anim.SetInteger("Speed", m_currentSpeed);
        //}
        //else if (m_v > 0 && m_currentSpeed == m_speed)
        //{
        //    m_anim.SetBool("Right", false);
        //    m_anim.SetBool("Left", false);
        //    m_anim.SetBool("Run", true);
        //    m_anim.SetInteger("Speed", m_currentSpeed);
        //}
        //else if(m_h > 0 && m_v == 0)
        //{
        //    m_anim.SetBool("Run", false);
        //    m_anim.SetBool("Back", false);
        //    m_anim.SetBool("Left", false);
        //    m_anim.SetInteger("Speed", 0);
        //    m_anim.SetBool("Right", true);
        //}
        //else if(m_h < 0 && m_v == 0)
        //{
        //    m_anim.SetBool("Run", false);
        //    m_anim.SetBool("Back", false);
        //    m_anim.SetBool("Right", false);
        //    m_anim.SetInteger("Speed", 0);
        //    m_anim.SetBool("Left", true);
        //}
        //else
        //{
        //    m_anim.SetBool("Run", false);
        //    m_anim.SetBool("Back", false);
        //    m_anim.SetBool("Right", false);
        //    m_anim.SetBool("Left", false);
        //    m_anim.SetInteger("Speed", 0);
        //    m_anim.SetBool("Back", true);
        //}
        m_anim.SetFloat("X", m_h);
        m_anim.SetFloat("Y", m_v + a);
    }
}