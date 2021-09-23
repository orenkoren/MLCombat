using MiddleAges.Database;
using MiddleAges.Events;

namespace MiddleAges.Resources
{
    public class RetributionPoints : RegenerableResource
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
        }

        private void DecreaseRetribution(object sender, AbilityEventArgs abilityArgs)
        {
            SafelyDecResource(abilities.GetAbility(abilityArgs.AbilityName).RetCost);
        }

        private void IncreaseRetribution(object sender, DamageDealtEventArgs eventArgs)
        {
            IncResource(eventArgs.AbilityData.RetGain);
        }
    }
}