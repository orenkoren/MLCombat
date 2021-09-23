using MiddleAges.Database;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class EquipWeaponManager : MonoBehaviour
    {
        public Transform LeftHand;
        public Transform RightHand;

        private WeaponEvents weaponEvents;
        private Dictionary<WeaponLocation, GameObject> currentWeapons;

        private void Awake()
        {
            weaponEvents = GetComponent<WeaponEvents>();
            currentWeapons = new Dictionary<WeaponLocation, GameObject>();
        }

        public void EquipWeapon(WeaponData weapon)
        {
            if (weapon.Location == WeaponLocation.LeftHand)
            {
                UnequipWeapon(weapon.Location);
                currentWeapons.Add(weapon.Location, Instantiate(weapon.WeaponPrefab, LeftHand));
            }
            if (weapon.Location == WeaponLocation.RightHand)
            {
                UnequipWeapon(weapon.Location);
                currentWeapons.Add(weapon.Location, Instantiate(weapon.WeaponPrefab, RightHand));
            }
            weaponEvents.FireWeaponEquipped(gameObject, weapon);
        }

        public void UnequipWeapon(WeaponLocation location)
        {
            if (!currentWeapons.ContainsKey(location)) return;
            Destroy(currentWeapons[location]);
            currentWeapons.Remove(location);
        }
    }
}