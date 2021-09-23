using MiddleAges.Entities;
using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Resources
{
    public class HealthPoints : RegenerableResource
    {
        private GameEvents events;
        private Entity entity;

        protected override void Start()
        {
            base.Start();
            events = GetComponentInParent<GameEvents>();
            entity = GetComponentInParent<Entity>();
            events.DamageTakenListeners += TakeDamage;
            events.HealingTakenListeners += HealSelf;
        }

        private void Update()
        {
            entity.animator.SetFloat("HealthPercentage", GetResourcePercentage());
        }

        private void TakeDamage(object sender, DamageTakenEventArgs args)
        {
            ForciblyDecResource(args.DamageAmount);
            if (IsEmpty() && entity.IsAlive())
            {
                StopCoroutine(regenCoroutine);
                events.FireDeath(((GameObject)sender).gameObject, new AbilityEventArgs("Death", new AnimationEventArgs("Death", true)));
            }
        }

        private void HealSelf(object sender, HealingTakenEventArgs args)
        {
            IncResource(args.HealingAmount);
        }

        private void OnDestroy()
        {
            events.DamageTakenListeners -= TakeDamage;
            events.HealingTakenListeners -= HealSelf;
        }
    }
}
