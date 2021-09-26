using MiddleAges.Database;
using System;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class WeaponEvents : MonoBehaviour
    {
        public event EventHandler<WeaponData> WeaponEquippedListeners;

        public void FireWeaponEquipped(object sender, WeaponData weapon) => WeaponEquippedListeners.Invoke(sender, weapon);
    }
}