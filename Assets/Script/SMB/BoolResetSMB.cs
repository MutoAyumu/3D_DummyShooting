using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolResetSMB : StateMachineBehaviour
{
    [SerializeField] string m_boolName;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(m_boolName, false);
    }
}
