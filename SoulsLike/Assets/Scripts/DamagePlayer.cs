using UnityEngine;

namespace OGS
{
    public class DamagePlayer : MonoBehaviour
    {
        [field: SerializeField]
        public int Damage { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            PlayerStats ps = other.GetComponent<PlayerStats>();
            if (ps != null)
            {
                ps.TakeDamage(Damage);
            }
        }
    }
}
