using MiddleAges.Database;
using MiddleAges.Events;
using System;

namespace MiddleAges.Resources
{
    public class SanctuaryPoints : RegenerableResource
    {
        private GameEvents events;
        private Abilities abilities;

        protected override void Start()
        {
            base.Start();
            events = GetComponentInParent<GameEvents>();
            abilities = GetComponentInParent<Abilities>();
            events.DamageDealtListeners += IncreaseRetribution;
            events.AbilityExecutedListeners += DecreaseRetribution;
            events.ResurrectionListeners += RestorePoints;
        }

        private void RestorePoints(object sender, int e)
        {
            SetCurrentResourcePoints(GetMaxResourcePoints());
        }

        private void DecreaseRetribution(object sender, AbilityEventArgs abilityArgs)
        {
            SafelyDecResource(abilities.GetAbility(abilityArgs.AbilityName).SancCost);
        }

        private void IncreaseRetribution(object sender, DamageDealtEventArgs eventArgs)
        {
            IncResource(eventArgs.AbilityData.SancGain);
        }

        private void OnDestroy()
        {
            events.DamageDealtListeners -= IncreaseRetribution;
            events.AbilityExecutedListeners -= DecreaseRetribution;
            events.ResurrectionListeners -= RestorePoints;
        }
    }
}