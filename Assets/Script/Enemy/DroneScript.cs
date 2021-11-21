using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    void LockOn()
    {
        var playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveController>();

    }
}
