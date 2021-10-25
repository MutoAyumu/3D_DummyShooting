using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerResetSMB : StateMachineBehaviour
{
    [SerializeField] string m_triggerName;
    [SerializeField] string m_boolName;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(m_triggerName);
        animator.SetBool(m_boolName, false);
        animator.applyRootMotion = false;
    }
}
