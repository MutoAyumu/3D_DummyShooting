using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddListScript : MonoBehaviour
{
    [SerializeField] string m_enemyTag;
    List<Transform> m_enemysList;

    public List<Transform> EnemysList
    {
        get { return m_enemysList; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(m_enemyTag))
        {
            m_enemysList.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(m_enemyTag))
        {
            m_enemysList.Remove(other.transform);
            m_enemysList.Sort();
        }
    }
}
