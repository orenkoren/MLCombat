using MiddleAges.Database;
using MiddleAges.Events;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class AICooldownParameters : MonoBehaviour
    {
        public Animator animator;
        public AbilityNameWithTrigger[] AbilityNameAndTrigger;

        private GameEvents events;
        private Dictionary<string, string> abilityNameToTrigger;

        void Start()
        {
            events = GetComponent<GameEvents>();
            abilityNameToTrigger = new Dictionary<string, string>();
            events.AbilityReadyListeners += SetParameterToTrue;
            events.AbilityExecutedListeners += SetParameterToFalse;
            foreach (var nameAndTrigger in AbilityNameAndTrigger)
            {
                abilityNameToTrigger.Add(nameAndTrigger.Ability.name, nameAndTrigger.TriggerName);
            }
        }

        public void SetParameterTrigger(string abilityName) => SetParameterToTrue(gameObject, new AbilityEventArgs(abilityName));

        private void SetParameterToFalse(object sender, AbilityEventArgs abilityArgs)
        {
            if (abilityNameToTrigger.ContainsKey(abilityArgs.AbilityName) == false) return;
            animator.SetBool(abilityNameToTrigger[abilityArgs.AbilityName], false);
        }

        private void SetParameterToTrue(object sender, AbilityEventArgs abilityArgs)
        {
            if (abilityNameToTrigger.ContainsKey(abilityArgs.AbilityName) == false) return;
            animator.SetBool(abilityNameToTrigger[abilityArgs.AbilityName], true);
        }

        private void OnDestroy()
        {
            events.AbilityReadyListeners -= SetParameterToTrue;
            events.AbilityExecutedListeners -= SetParameterToFalse;
        }
    }

    [System.Serializable]
    public class AbilityNameWithTrigger
    {
        public Ability Ability;
        public string TriggerName;

        public AbilityNameWithTrigger(Ability ability, string triggerName)
        {
            Ability = ability;
            TriggerName = triggerName;
        }
    }
}