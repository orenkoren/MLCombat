using MiddleAges.Database;
using MiddleAges.Events;
using System;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class ChannelAbilities : MonoBehaviour
    {
        private GameEvents events;
        private Abilities abilities;
        private AbilityData currentlyChanneled;

        void Start()
        {
            events = GetComponent<GameEvents>();
            abilities = GetComponent<Abilities>();
            events.AbilityTriggeredListeners += StartChanneling;
            events.AbilityEndedListeners += StopChanneling;
        }

        public AbilityData GetCurrentlyChanneledAbility() => currentlyChanneled;

        public bool IsCurrentlyChanneling() => currentlyChanneled != null;

        private void StartChanneling(object sender, AbilityEventArgs abilityArgs)
        {
            AbilityData abilityData = abilities.GetAbility(abilityArgs.AbilityName);
            currentlyChanneled = abilityData;
            if (abilityData.IsChanneled)
                events.FireAbilityExecuted(this, new AbilityEventArgs(abilityArgs.AbilityName));
        }

        private void StopChanneling(object sender, AbilityEventArgs abilityArgs)
        {
            currentlyChanneled = null;
        }

        private void OnDestroy()
        {
            events.AbilityTriggeredListeners -= StartChanneling;
            events.AbilityEndedListeners -= StopChanneling;
        }
    }
}