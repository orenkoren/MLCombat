using MiddleAges.Database;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class AbilityInfoOverrides : MonoBehaviour
    {
        public AbilityData[] AbilityOverrides;

        private Abilities abilities;

        void Start()
        {
            abilities = GetComponent<Abilities>();
            foreach (var abilityOverride in AbilityOverrides)
            {
                abilities.ReplaceAbility(abilityOverride);
            }
        }
    }
}