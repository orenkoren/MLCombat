using MiddleAges.Database;
using MiddleAges.Entities;
using MiddleAges.Events;
using MiddleAges.Manager;
using MiddleAges.Motion;
using MiddleAges.Resources;
using MiddleAges.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleAges.Combat
{

    public class CombatCore : MonoBehaviour
    {
        #region Public Properties

        public LayerMask hitableThings;
        public Transform weapon;
        public GameEvents events;

        #endregion Public Properties

        #region Protected Properties

        protected Abilities abilities;
        protected CooldownUtil cooldownUtil;
        protected Entity entity;
        protected Movement movement;
        protected Stats stats;
        protected bool combatEnabled = true;

        #endregion Protected Properties

        #region Private Properties

        private int critChance = 20;
        private int critDamage = 150;
        private int missChance = 0;
        private int healCritChance = 25;
        protected Vector3 crosshairTarget;

        #endregion Private Properties

        #region Unity Methods

        protected virtual void Start()
        {
            entity = GetComponent<Entity>();
            movement = GetComponent<Movement>();
            stats = GetComponent<Stats>();
            abilities = GetComponent<Abilities>();
            cooldownUtil = GetComponent<CooldownUtil>();
            events.AbilityExecutedListeners += UseAbility;
            events.DeathListeners += DisableCombat;
            events.ResurrectionListeners += EnableCombat;
            events.GroundedListeners += DisableCombat;
        }

        #endregion Unity Methods

        #region Public Methods

        public virtual void DealDamage(AbilityData abilityData)
        {
            List<Transform> attackHits = HasHitSomething(abilityData);
            if (attackHits.Count > 0)
            {
                foreach (var attackHit in attackHits)
                {
                    bool isCrit = DetermineIfAttackCrits();
                    int finalDamage = CalculateFinalDamage(abilityData.BaseDamage, isCrit);
                    attackHit.GetComponent<CombatCore>().TakeDamage(abilityData, finalDamage, isCrit, abilityData.Type, transform);
                    events.FireDamageDealt(gameObject, new DamageDealtEventArgs(abilityData, isCrit));
                }
            }
        }

        public virtual void HealSelf(AbilityData abilityData)
        {
            //int healAmount = Random.Range(abilityData.BaseDamage - 10, abilityData.BaseDamage + 10); // TODO: Fix this
            int healAmount = (int)stats.GetHealthPoints().GetMaxResourcePoints() / 5;
            bool isCrit = DetermineIfHealCrits();
            if (isCrit)
                healAmount *= 2;
            events.FireHealingTaken(this, new HealingTakenEventArgs(healAmount, isCrit));
        }

        public virtual void UseAbility(object sender, AbilityEventArgs abilityArgs) // maybe pass the ability data through the event
        {
            AbilityData abilityData = abilities.GetAbility(abilityArgs.AbilityName);
            if (abilityData.Type == AbilityType.Melee || abilityData.Type == AbilityType.Spell)
                DealDamage(abilityData);
            if (abilityData.Type == AbilityType.Heal)
                HealSelf(abilityData);
        }

        #endregion Public Methods

        #region Private Methods

        private void TakeDamage(AbilityData abilityData, int damage, bool isCrit, AbilityType damageType, Transform origin)
        {
            float actualDamage = damage;
            bool isTargetInFront = IsTargetInfront(origin);
            if (isTargetInFront)
                actualDamage *= ((float)(100 - stats.GetDamageResistance())) / 100;
            int roundedDamage = (int)actualDamage;
            bool isMiss = DetermineIfAttackMisses();
            if (isMiss)
                roundedDamage = 0;
            events.FireDamageTaken(gameObject, new DamageTakenEventArgs(roundedDamage, isCrit, isMiss,
                                                                        false, abilityData, damageType, !isTargetInFront));
        }

        private bool IsTargetInfront(Transform target)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            return Vector3.Dot(directionToTarget, transform.forward) > 0;
        }

        private bool IsTargetInHitArc(Transform target, float arcAngles)
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float angleToTarget = (Vector3.Angle(directionToTarget, transform.forward));
            return angleToTarget < arcAngles;
        }

        private int CalculateFinalDamage(int abilityBaseDamage, bool isCrit)
        {
            int damage = abilityBaseDamage;
            damage = Random.Range(damage, damage + 7);
            float finalDamage = damage;
            if (isCrit)
                finalDamage *= ((float)critDamage / 100);
            return (int)finalDamage;
        }

        #region Raycast Handlers

        private List<Transform> HasHitSomething(AbilityData abilityData)
        {

            RaycastHit[] hits = GetAbilityHits(abilityData);
            List<Transform> transformHits = new List<Transform>();
            for (var i = 0; i < hits.Length; i++)
            {
                if (IsTargetInHitArc(hits[i].transform, abilityData.HitRadiusArcDegrees) || abilityData.Range > 0)
                    transformHits.Add(hits[i].transform);
            }
            return transformHits;
        }

        private RaycastHit[] GetAbilityHits(AbilityData abilityData)
        {
            if (abilityData.Range > 0)
                return RangeAbilityRaycast(abilityData);
            else
                return NoRangeAbilityRaycast(abilityData);
        }

        private RaycastHit[] NoRangeAbilityRaycast(AbilityData abilityData)
        {
            return Physics.SphereCastAll(weapon.position,
                                        abilityData.HitRadius, transform.forward,
                                        abilityData.HitRadius, hitableThings);
        }

        protected virtual RaycastHit[] RangeAbilityRaycast(AbilityData abilityData)
        {
            crosshairTarget = RaycastManager.GetCrosshairTargetPoint();
            float crosshairTargetDistance = Vector3.Distance(transform.position, crosshairTarget);
            float abilityRange = crosshairTargetDistance < abilityData.Range ?
                                 crosshairTargetDistance : abilityData.Range;
            return Physics.SphereCastAll(origin: weapon.position,
                                                      radius: abilityData.HitRadius,
                                                      direction: (crosshairTarget - weapon.position).normalized,
                                                      maxDistance: abilityRange, hitableThings);
        }

        #endregion Raycast Handlers

        #region Attack Modifiers

        private bool DetermineIfAttackMisses()
        {
            return Random.Range(1, 100) < missChance;
        }

        private bool DetermineIfAttackCrits()
        {
            return Random.Range(1, 100) < critChance;
        }

        private bool DetermineIfHealCrits()
        {
            return Random.Range(1, 100) < healCritChance;
        }

        #endregion Attack Modifiers

        private void DisableCombat(object sender, AnimationEventArgs args)
        {
            combatEnabled = args.BoolValue;
        }

        private void DisableCombat(object sender, AbilityEventArgs abilityArgs)
        {
            combatEnabled = abilityArgs.AnimationParameters.BoolValue;
        }

        private void EnableCombat(object sender, int e)
        {
            combatEnabled = true;
        }

        private void OnDestroy()
        {
            events.AbilityExecutedListeners -= UseAbility;
            events.DeathListeners -= DisableCombat;
            events.GroundedListeners -= DisableCombat;
            events.ResurrectionListeners -= EnableCombat;
        }

        #endregion Private Methods
    }
}
