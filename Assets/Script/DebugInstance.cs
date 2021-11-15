using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInstance : MonoBehaviour
{
    [SerializeField] GameObject[] m_object;
    float time;
    [SerializeField] float instanceTime = 5f;
    [SerializeField] Transform instancePos = default;

    private void FixedUpdate()
    {
        time += Time.deltaTime;

        if(time > instanceTime)
        {
            Instance();
            time = 0;
        }
    }
    public void Instance()
    {
        foreach(var go in m_object)
        {
            var obj = Instantiate(go, instancePos.position, Quaternion.identity);
            Destroy(obj, 2);
        }
    }
}
