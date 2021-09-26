using MiddleAges.Database;
using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class AbilityExecutor : MonoBehaviour
    {
        public Ability ability;

        protected GameEvents events;
        protected bool isAbilityActive;

        protected virtual void Start()
        {
            events = GetComponentInParent<GameEvents>();
            events.AbilityTriggeredListeners += ApplyAbility;
            events.AbilityEndedListeners += FinishAbility;
        }

        protected virtual void ApplyAbility(object sender, AbilityEventArgs e)
        {
            if (e.AbilityName == ability.name)
                isAbilityActive = true;
        }

        protected virtual void FinishAbility(object sender, AbilityEventArgs e)
        {
            if (e.AbilityName == ability.name)
                isAbilityActive = false;
        }
    }
}