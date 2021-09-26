using MiddleAges.Database;
using MiddleAges.Entities;
using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Resources
{
    public class Stats : MonoBehaviour //TODO: make the data scriptable, extract functionality
    {
        protected GameEvents events;
        protected HealthPoints hp;

        protected Entity entity;
        protected Abilities abilities;

        private int damageResistance = 0;

        protected virtual void Awake()
        {
            hp = GetComponentInChildren<HealthPoints>();
        }

        protected virtual void Start()
        {
            entity = GetComponent<Entity>();
            abilities = GetComponent<Abilities>();
            events = GetComponent<GameEvents>();
            events.AbilityExecutedListeners += ApplyStatChanges;
            events.AbilityEndedListeners += RestoreStatChanges;
        }

        protected virtual void Update()
        {
        }

        protected virtual void RestoreStatChanges(object sender, AbilityEventArgs abilityArgs)
        {
            var abilityData = abilities.GetAbility(abilityArgs.AbilityName);
            damageResistance -= abilityData.DamageReduction; // TODO: create logic for damage reductions
        }

        protected virtual void ApplyStatChanges(object sender, AbilityEventArgs abilityArgs)
        {
            var abilityData = abilities.GetAbility(abilityArgs.AbilityName);
            damageResistance = abilityData.DamageReduction;
        }

        public virtual bool HasEnoughResources(AbilityData abilityData) => true;

        public HealthPoints GetHealthPoints() => hp;

        //public void IncreaseDamageResistance(int damageResistIncrease)
        //{
        //    if(damageResistance + damageResistIncrease <= 100)
        //    damageResistance += damageResistIncrease;
        //}

        //public void DecreaseDamageResistance(int damageResistDecrease)
        //{
        //    damageResistance -= damageResistDecrease;
        //}

        public int GetDamageResistance() => damageResistance;

        private void OnDestroy()
        {
            events.AbilityExecutedListeners -= ApplyStatChanges;
            events.AbilityEndedListeners -= RestoreStatChanges;
        }
    }
}
