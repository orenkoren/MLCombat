using UnityEngine;

namespace MiddleAges.Database
{
    [System.Serializable]
    public class WeaponData : ItemData
    {
        public int WeaponDamage;
        public GameObject WeaponPrefab;
        public AnimatorOverrideController AnimatorOverride;
        public WeaponLocation Location;
    }
}