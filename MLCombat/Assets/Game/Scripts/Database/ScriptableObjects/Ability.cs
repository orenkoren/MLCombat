using UnityEngine;

namespace MiddleAges.Database
{
    [CreateAssetMenu(fileName = "Abilities", menuName = "Database/Ability", order = 0)]
    public class Ability : ScriptableObject
    {
        public AbilityData AbilityInfo;
    }
}