using UnityEngine;

namespace MiddleAges.Database
{
    [CreateAssetMenu(fileName = "Weapons", menuName = "Database/Items/Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        public WeaponData WeaponInfo;
    }
}