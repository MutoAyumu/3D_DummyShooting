using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStatus : ScriptableObject
{
    [SerializeField] float m_hp, m_speed, m_power;

    public float HP
    {
        get { return m_hp; }
    }
    public float Speed
    {
        get { return m_speed; }
    }
    public float Power
    {
        get { return m_power; }
    }

}
