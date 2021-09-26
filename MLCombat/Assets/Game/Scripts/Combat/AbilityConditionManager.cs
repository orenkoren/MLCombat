using MiddleAges.Combat;
using MiddleAges.Database;
using MiddleAges.Events;
using MiddleAges.Manager;
using MiddleAges.Motion;
using MiddleAges.Resources;
using MiddleAges.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Game.Scripts.Combat
{
    public class AbilityConditionManager : MonoBehaviour
    {
        private CooldownUtil cooldownUtil;
        private Stats stats;
        private Movement movement;
        private ChannelAbilities channelManager;
        private SpecialFollowupManager followupManager;
        private GameEvents events;
        private Abilities abilities;

        void Start()
        {
            cooldownUtil = GetComponent<CooldownUtil>();
            stats = GetComponent<Stats>();
            movement = GetComponent<Movement>();
            channelManager = GetComponent<ChannelAbilities>();
            followupManager = GetComponent<SpecialFollowupManager>();
            events = GetComponent<GameEvents>();
            abilities = GetComponent<Abilities>();
        }

        private void Update()
        {
            foreach (var ability in abilities.abilitiesData.Values)
            {
                var abilityName = ability.Name;
                if (cooldownUtil.IsAbilityReady(abilityName))
                {
                    events.FireAbilityUsable(gameObject, new AbilityEventArgs(abilityName, IsAbilityUseable(ability)));
                }
            }
        }

        public bool IsAbilityUseable(AbilityData ability)
        {
            if (ability == null)
                return false;
            if (channelManager.GetCurrentlyChanneledAbility()?.Name == ability.Name)
                return false;
            if (cooldownUtil.IsAbilityReady(ability.Name) == false || stats.HasEnoughResources(ability) == false)
                return false;
            if (ability.Conditions == null || ability.Conditions.Count == 0)
                return channelManager.IsCurrentlyChanneling() == false && movement.IsNotRooted();
            return AbilityConditionsMet(ability);

        }

        private bool AbilityConditionsMet(AbilityData ability)
        {
            List<bool> results = new List<bool>();

            if (ability.Conditions.Contains(AbilityConditionType.WhileChanneling)) results.Add(true);
            else results.Add(channelManager.IsCurrentlyChanneling() == false);
            if (ability.Conditions.Contains(AbilityConditionType.WhileRooted)) results.Add(true);
            else results.Add(movement.IsNotRooted());
            if (ability.Conditions.Contains(AbilityConditionType.AfterBlock))
                results.Add(followupManager.IsBlockResponseActive());
            foreach (var result in results)
            {
                if (result == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}