using UnityEngine;

namespace OGS
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot slot in weaponHolderSlots)
            {
                if (slot.IsLeftHandSlot)
                {
                    leftHandSlot = slot;
                }
                else if (slot.IsRightHandSlot)
                {
                    rightHandSlot = slot;
                }
                else
                {
                    Debug.LogError($"Unassigned hand slot {slot}");
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weapon);
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weapon);
            }
        }
    }
}
