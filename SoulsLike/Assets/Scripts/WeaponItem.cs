using UnityEngine;

namespace OGS
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One Handed ATtack Animations")]
        public string OneHandedLightAttack1;
        public string OneHandedHeavyAttack1;
    }
}
