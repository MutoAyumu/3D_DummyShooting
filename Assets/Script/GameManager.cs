using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]public List<GameObject> m_enemysList;
    PlayerMoveController m_player;

    static GameManager instance = null;

    //public List<GameObject> EnemysList
    //{
    //    get { return m_enemysList; }
    //    set { m_enemysList = value; }
    //}

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            m_player = GameObject.FindObjectOfType<PlayerMoveController>();
        }
    }

    private void Update()
    {
        if (m_enemysList.Count != 0)
        {
            GameObject enemy = default;
            enemy = m_enemysList[0];

            enemy = m_enemysList.OrderBy(go => Vector3.Distance(m_player.transform.position, go.transform.position)).FirstOrDefault();

            if (m_player)
            {
                m_player.m_target = enemy;
            }
        }
        else
        {
            if (m_player)
            {
                m_player.m_target = null;
            }
        }
    }
}
