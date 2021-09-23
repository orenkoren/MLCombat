using Assets.Game.Scripts.Combat;
using MiddleAges.Database;
using MiddleAges.Events;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class PaladinCombat : CombatCore
    {
        #region Private Properties

        private Dictionary<KeyCode, AbilityData> possibleActions;
        private AbilityCombos combos;
        private AbilityConditionManager conditionManager;

        #endregion Private Properties

        #region Unity Methods

        protected override void Start()
        {
            base.Start();
            possibleActions = new Dictionary<KeyCode, AbilityData>();
            combos = GetComponent<AbilityCombos>();
            conditionManager = GetComponent<AbilityConditionManager>();
            foreach (var ability in abilities.abilitiesData)
            {
                if (ability.Value.Keybind != KeyCode.None && combos.IsAbilityInCombo(ability.Key) == false)
                    possibleActions.Add(ability.Value.Keybind, ability.Value);
            }
        }

        private void Update()
        {
            if (!combatEnabled) return;
            foreach (var action in possibleActions)
            {
                if (Input.GetKeyDown(action.Key))
                    UseAbility(action.Value);
                if (Input.GetKeyUp(action.Key))
                    FinishChanneledAbility(action.Value);
            }
        }

        #endregion Unity Methods

        #region Private Methods

        private void UseAbility(AbilityData ability)
        {
            if (conditionManager.IsAbilityUseable(ability))
            {
                events.FireAbilityQueued(this, new AbilityEventArgs(ability.Name, new AnimationEventArgs(ability.Name)));
            }
        }

        private void FinishChanneledAbility(AbilityData ability)
        {
            if (ability.IsChanneled)
            {
                events.FireAbilityDequeued(gameObject, new AbilityEventArgs(ability.Name, new AnimationEventArgs(ability.Name, false)));
            }
        }

        #endregion Private Methods
    }
}
