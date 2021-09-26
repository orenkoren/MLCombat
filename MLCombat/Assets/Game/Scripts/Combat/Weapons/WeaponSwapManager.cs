using MiddleAges.Database;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class WeaponSwapManager : MonoBehaviour
    {
        public WeaponSet PrimaryWeapons;
        public WeaponSet SecondaryWeapons;

        private WeaponSet currentlyEquipped;
        private EquipWeaponManager equipManager;

        void Start()
        {
            currentlyEquipped = PrimaryWeapons;
            equipManager = GetComponent<EquipWeaponManager>();
            EquipWeapons();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwapWeapons();
            }
        }

        public void SwapWeapons()
        {
            if (currentlyEquipped.WeaponSetType == WeaponSetType.Primary)
            {
                UnequipWeapons();
                currentlyEquipped = SecondaryWeapons;
                EquipWeapons();
            }
            else
            {
                currentlyEquipped = PrimaryWeapons;
                EquipWeapons();
            }
        }

        private void EquipWeapons()
        {
            if (currentlyEquipped.LeftHandWeapon)
                equipManager.EquipWeapon(currentlyEquipped.LeftHandWeapon.WeaponInfo);
            if (currentlyEquipped.RightHandWeapon)
                equipManager.EquipWeapon(currentlyEquipped.RightHandWeapon.WeaponInfo);
        }

        private void UnequipWeapons()
        {
            equipManager.UnequipWeapon(WeaponLocation.LeftHand);
            equipManager.UnequipWeapon(WeaponLocation.RightHand);
        }
    }

    [System.Serializable]
    public class WeaponSet
    {
        public WeaponSetType WeaponSetType;
        public Weapon RightHandWeapon;
        public Weapon LeftHandWeapon;
    }
}

public enum WeaponSetType
{
    Primary,
    Secondary
}