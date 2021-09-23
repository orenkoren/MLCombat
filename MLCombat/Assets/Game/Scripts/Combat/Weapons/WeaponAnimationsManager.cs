using MiddleAges.Database;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class WeaponAnimationsManager : MonoBehaviour
    {
        public Animator animator;

        private WeaponEvents weaponEvents;

        void Awake()
        {
            weaponEvents = GetComponent<WeaponEvents>();
            weaponEvents.WeaponEquippedListeners += UpdateAnimatorOverride;
        }

        private void UpdateAnimatorOverride(object sender, WeaponData weapon)
        {
            animator.runtimeAnimatorController = weapon.AnimatorOverride;
            animator.SetTrigger("EquipWeapon");
        }
    }
}