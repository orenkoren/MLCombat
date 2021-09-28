using MiddleAges.Database;
using MiddleAges.Events;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace MiddleAges.Utils
{
    public class CooldownUtil : MonoBehaviour
    {

        private GameEvents events;
        private Abilities abilities;
        private Dictionary<string, float> cooldowns;
        private Dictionary<string, bool> hasFired;

        public void Start()
        {
            cooldowns = new Dictionary<string, float>();
            hasFired = new Dictionary<string, bool>();
            abilities = GetComponent<Abilities>();
            events = GetComponent<GameEvents>();
            events.AbilityExecutedListeners += ApplyCooldown;
            events.ResurrectionListeners += RestoreCooldowns;
            InitiateCooldowns();
        }

        private void RestoreCooldowns(object sender, int e)
        {
            InitiateCooldowns();
        }

        void Update()
        {
            var keys = new List<string>(cooldowns.Keys); // TODO: refactor this
            foreach (string abilityName in keys)
            {
                if (hasFired[abilityName] == false && cooldowns[abilityName] >= abilities.GetAbility(abilityName).Cooldown)
                {
                    events.FireAbilityReady(this, new AbilityEventArgs(abilityName));
                    hasFired[abilityName] = true;
                }
                cooldowns[abilityName] += Time.deltaTime;
            }
        }

        private void InitiateCooldowns()
        {
            foreach (var ability in abilities.abilitiesData)
            {
                cooldowns[ability.Value.Name] = ability.Value.Cooldown;
                hasFired[ability.Value.Name] = false;
            }
        }

        public bool IsAbilityReady(string abilityName)
        {
            return cooldowns[abilityName] >= abilities.GetAbility(abilityName).Cooldown;
        }

        public void ApplyCooldown(object sender, AbilityEventArgs abilityArgs)
        {
            cooldowns[abilityArgs.AbilityName] = 0;
            hasFired[abilityArgs.AbilityName] = false;
        }

        public void DecreaseCurrentCooldown(string abilityName, float amount)
        {
            if (!cooldowns.ContainsKey(abilityName)) return;
            cooldowns[abilityName] += amount;
        }

        public float GetCurrentCooldown(string abilityName)
        {
            if (!cooldowns.ContainsKey(abilityName)) return 0;
            return abilities.GetAbility(abilityName).Cooldown -  cooldowns[abilityName];
        }

        private void OnDestroy()
        {
            events.AbilityExecutedListeners -= ApplyCooldown;
            events.ResurrectionListeners -= RestoreCooldowns;
        }
    }
}
