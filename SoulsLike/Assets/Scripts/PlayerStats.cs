using UnityEngine;

namespace OGS
{
    public class PlayerStats : MonoBehaviour
    {
        [field: SerializeField]
        public int HealthLevel { get; private set; }
        [field: SerializeField]
        public int MaxHealth { get; private set; }
        [field: SerializeField]
        public int CurrentHealth { get; private set; }

        [field: SerializeField]
        public HealthBar HealthBar;

        private AnimatorHandler anim;

        void Start()
        {
            MaxHealth = SetMaxHealthFromHealthLevel();
            CurrentHealth = MaxHealth;
            HealthBar.SetMaxHealth(MaxHealth);
            anim = GetComponentInChildren<AnimatorHandler>();
        }

        private int SetMaxHealthFromHealthLevel()
        {
            MaxHealth = HealthLevel * 10;
            return MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Max(CurrentHealth, 0);
            HealthBar.SetCurrentHealth(CurrentHealth);
            anim.PlayTargetAnimation("FlinchSmall", true);
            if (CurrentHealth <= 0)
            {
                anim.PlayTargetAnimation("Die", true);
            }
        }
    }
}
