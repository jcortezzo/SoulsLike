using UnityEngine;

namespace OGS
{
    public class ResetIsInteracting : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsInteracting", false);
            PlayerManager playerManager = animator.gameObject.GetComponentInParent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.PlayerState = PlayerState.IDLE;
            }
        }
    }
}
