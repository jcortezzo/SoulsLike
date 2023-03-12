using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string targetBool;
    public bool status;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(targetBool, status);
    }
}
