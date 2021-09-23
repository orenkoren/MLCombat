using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiddleAges.Database
{
    public class Abilities : MonoBehaviour
    {
        #region Public Properties
        public Ability[] AbilitiesInfo;


        [HideInInspector]
        public Dictionary<string, AbilityData> abilitiesData;

        #endregion Public Properties

        #region Constructor

        public void Awake()
        {
            abilitiesData = new Dictionary<string, AbilityData>();
            foreach (var ability in AbilitiesInfo)
            {
                AddAbility(ability.AbilityInfo);
            }
        }

        #endregion Constructor

        #region Public Methods

        public AbilityData GetAbility(string abilityName, GameObject abilityOverride = null)
        {
            if (abilitiesData.ContainsKey(abilityName))
                return abilitiesData[abilityName];
            Debug.Log("ability not found" + abilityName);
            return null;
        }

        public bool AbilityHasActionSlot(string abilityName) => abilitiesData[abilityName].ActionSlot > 0;

        public int GetActionSlotNumber(string abilityName) => abilitiesData[abilityName].ActionSlot - 1;

        public AbilityData GetAbilityBySlotNumber(int number) =>
            abilitiesData.First((ability) => ability.Value.ActionSlot == number + 1).Value;

        public AbilityData GetAbilityByKeybind(KeyCode bind) => abilitiesData.First((ability) => ability.Value.Keybind == bind).Value;

        public void SetKeybind(string abilityName, KeyCode keybind)
        {
            foreach (var ability in abilitiesData)
            {
                if (ability.Value.Keybind == keybind)
                    ability.Value.Keybind = KeyCode.None;
            }
            abilitiesData[abilityName].Keybind = keybind;
        }

        #endregion Public Methods

        #region Build Database

        public void AddAbility(AbilityData ability)
        {
            abilitiesData.Add(ability.Name, ability);
        }

        public void ReplaceAbility(AbilityData ability)
        {
            if (abilitiesData.ContainsKey(ability.Name))
                abilitiesData[ability.Name] = ability;
        }

        public AbilityData[] GetAllAbilitiesWithBlockCondition()
        {
            return (from ability in abilitiesData
                    where ability.Value.Conditions.Contains(AbilityConditionType.AfterBlock)
                    select ability.Value).ToArray();
        }

        #endregion Build Database
    }
}