using UnityEngine;

namespace OGS
{
    public class EnemyStats : MonoBehaviour
    {
        [field: SerializeField]
        public int HealthLevel { get; private set; }
        [field: SerializeField]
        public int MaxHealth { get; private set; }
        [field: SerializeField]
        public int CurrentHealth { get; private set; }

        private Animator anim;

        void Start()
        {
            MaxHealth = SetMaxHealthFromHealthLevel();
            CurrentHealth = MaxHealth;
            anim = GetComponentInChildren<Animator>();
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
            anim.Play("FlinchSmall");
            if (CurrentHealth <= 0)
            {
                anim.Play("Die");
            }
        }
    }
}
