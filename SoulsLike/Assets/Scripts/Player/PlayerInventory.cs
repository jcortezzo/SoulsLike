using UnityEngine;

namespace OGS
{
    public class PlayerInventory : MonoBehaviour
    {
        private WeaponSlotManager weaponSlotManager;

        [field: SerializeField]
        public WeaponItem RightWeapon { get; private set; }
        [field: SerializeField]
        public WeaponItem LeftWeapon { get; private set; }

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);
        }
    }
}
