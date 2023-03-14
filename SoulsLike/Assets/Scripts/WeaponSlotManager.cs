using System;
using UnityEngine;

namespace OGS
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private WeaponHolderSlot leftHandSlot;
        private WeaponHolderSlot rightHandSlot;

        private DamageCollider leftHandDamageCollider;
        private DamageCollider rightHandDamageCollider;

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
            LoadWeaponDamageCollider(isLeft);
        }

        #region Handle Weapon's Damage Collider

        public void LoadWeaponDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }

        private void InvokeFuncWeaponDamageCollider(Action func)
        {
            func.Invoke();
        }

        public void OpenLeftHandDamageCollider()
        {
            InvokeFuncWeaponDamageCollider(leftHandDamageCollider.EnableDamageCollider);
        }

        public void CloseLeftHandDamageCollider()
        {
            InvokeFuncWeaponDamageCollider(leftHandDamageCollider.DisableDamageCollider);
        }

        public void OpenRightHandDamageCollider()
        {
            InvokeFuncWeaponDamageCollider(rightHandDamageCollider.EnableDamageCollider);
        }

        public void CloseRightHandDamageCollider()
        {
            InvokeFuncWeaponDamageCollider(rightHandDamageCollider.DisableDamageCollider);
        }
        #endregion
    }
}
