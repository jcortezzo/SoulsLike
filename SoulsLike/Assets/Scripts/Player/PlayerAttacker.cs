using UnityEngine;

namespace OGS
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler animatorHandler;
        private PlayerManager playerManager;


        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (!playerManager.IsActionable)
            {
                return;
            }
            animatorHandler.PlayTargetAnimation(weapon.OneHandedLightAttack1, true);
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (!playerManager.IsActionable)
            {
                return;
            }
            animatorHandler.PlayTargetAnimation(weapon.OneHandedHeavyAttack1, true);
        }
    }
}
