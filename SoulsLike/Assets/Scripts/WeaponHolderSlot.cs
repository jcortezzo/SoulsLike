using UnityEngine;

namespace OGS
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        [field: SerializeField]
        public bool IsLeftHandSlot { get; private set; }
        [field: SerializeField]
        public bool IsRightHandSlot { get; private set; }

        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weapon)
        {
            UnloadWeaponAndDestroy();
            if (weapon == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weapon.modelPrefab) as GameObject;
            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = model;
        }

        public void LoadWeaponModel()
        {
            LoadWeaponModel(null);
        }
    }
}
