using MiddleAges.Events;
using MiddleAges.Utils;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class RelatedAbilitiesManager : MonoBehaviour
    {
        private GameEvents events;
        private CooldownUtil cooldowns;

        private void Start()
        {
            events = GetComponentInParent<GameEvents>();
            cooldowns = GetComponentInParent<CooldownUtil>();
            events.DamageDealtListeners += ApplyRelatedEffects;
        }

        private void ApplyRelatedEffects(object sender, DamageDealtEventArgs eventArgs)
        {
            var sourceAbility = eventArgs.AbilityData;
            foreach (var relatedAbility in sourceAbility.RelatedAbilities)
            {
                if (relatedAbility.RestoresCooldown > 0)
                    cooldowns.DecreaseCurrentCooldown(relatedAbility.Ability.AbilityInfo.Name, relatedAbility.RestoresCooldown);
            }
        }
    }
}