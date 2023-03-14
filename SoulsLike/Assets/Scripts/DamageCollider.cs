using UnityEngine;

namespace OGS
{
    public class DamageCollider : MonoBehaviour
    {
        private Collider damageCollider;

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStats ps = collision.GetComponent<PlayerStats>();
                if (ps != null)
                {
                    ps.TakeDamage(currentWeaponDamage);
                }
            }

            if (collision.tag == "Enemy")
            {
                EnemyStats es = collision.GetComponent<EnemyStats>();
                if (es != null)
                {
                    es.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}
