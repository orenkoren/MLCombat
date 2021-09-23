using MiddleAges.Database;
using MiddleAges.Entities;
using MiddleAges.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class DotManager : MonoBehaviour
    {
        private GameEvents events;
        private Entity entity;
        private List<string> activeDots;
        void Start()
        {
            events = GetComponentInParent<GameEvents>();
            entity = GetComponentInParent<Entity>();
            activeDots = new List<string>();
            events.DamageTakenListeners += ApplyDot;
        }

        private void ApplyDot(object sender, DamageTakenEventArgs eventArgs)
        {
            if (eventArgs.DamageAmount > 0 && eventArgs.AbilityData.Dot > 0 && activeDots.Contains(eventArgs.AbilityData.Name) == false)
            {
                activeDots.Add(eventArgs.AbilityData.Name);
                StartCoroutine(TakeDotDamage(eventArgs.AbilityData));
            }
        }

        IEnumerator TakeDotDamage(AbilityData abilityData)
        {
            int ticks = 0;
            while (ticks <= abilityData.TickAmount)
            {
                yield return new WaitForSeconds(abilityData.DotInterval);
                if (entity.IsAlive())
                    events.FireDamageTaken(gameObject, new DamageTakenEventArgs(abilityData.Dot, false, false, false, abilityData, AbilityType.Spell, false));
                ticks++;
            }
            activeDots.Remove(abilityData.Name);
        }
    }
}