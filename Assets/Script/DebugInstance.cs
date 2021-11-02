using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInstance : MonoBehaviour
{
    [SerializeField] GameObject[] m_object;

    public void Instance()
    {
        foreach(var go in m_object)
        {
            Instantiate(go);
        }
    }
}
