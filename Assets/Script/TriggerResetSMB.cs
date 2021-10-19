using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerResetSMB : StateMachineBehaviour
{
    [SerializeField] string m_triggerName;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(m_triggerName);
    }
}
