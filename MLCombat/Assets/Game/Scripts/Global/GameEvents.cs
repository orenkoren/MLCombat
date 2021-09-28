using MiddleAges.Database;
using MiddleAges.SpecialEffects;
using System;
using UnityEngine;

namespace MiddleAges.Events
{
    public class GameEvents : MonoBehaviour
    {
        #region State Events

        public event EventHandler<int> LevelUpListeners;
        public event EventHandler<AbilityEventArgs> DeathListeners;
        public event EventHandler<int> ResurrectionListeners;

        public void FireLevelChanged(object sender, int level) => LevelUpListeners?.Invoke(sender, level);
        public void FireDeath(object sender, AbilityEventArgs abilityArgs) => DeathListeners?.Invoke(sender, abilityArgs);
        public void FireResurrection(object sender, int placeHolder) => ResurrectionListeners?.Invoke(sender, placeHolder);

        #endregion State Events

        #region Ability Events
        public event EventHandler<AbilityEventArgs> AbilityExecutedListeners;
        public event EventHandler<AbilityEventArgs> AbilityTriggeredListeners;
        public event EventHandler<AbilityEventArgs> AbilityQueuedListeners;
        public event EventHandler<AbilityEventArgs> AbilityDequeueListeners;
        public event EventHandler<AbilityEventArgs> AbilityReadyListeners;
        public event EventHandler<AbilityEventArgs> AbilityUsableListeners;
        public event EventHandler<AbilityEventArgs> AbilityEndedListeners;
        public event EventHandler<bool> StunStateListeners;
        public event EventHandler<bool> BlockFollowupStateListeners;

        public void FireAbilityExecuted(object sender, AbilityEventArgs abilityArgs) => AbilityExecutedListeners.Invoke(sender, abilityArgs);
        public void FireAbilityTriggered(object sender, AbilityEventArgs abilityArgs) => AbilityTriggeredListeners?.Invoke(sender, abilityArgs);
        public void FireAbilityReady(object sender, AbilityEventArgs abilityArgs) => AbilityReadyListeners?.Invoke(sender, abilityArgs);
        public void FireAbilityQueued(object sender, AbilityEventArgs abilityArgs) => AbilityQueuedListeners.Invoke(sender, abilityArgs);
        public void FireAbilityDequeued(object sender, AbilityEventArgs abilityArgs) => AbilityDequeueListeners.Invoke(sender, abilityArgs);
        public void FireStun(object sender, bool isStunned) => StunStateListeners.Invoke(sender, isStunned);
        public void FireBlockFollowupState(object sender, bool isActive) => BlockFollowupStateListeners?.Invoke(sender, isActive);
        public void FireAbilityUsable(object sender, AbilityEventArgs abilityArgs) => AbilityUsableListeners?.Invoke(sender, abilityArgs);

        public event EventHandler<Effect> SpawnEffectPrefabListeners;
        public void FireSpawnEffect(object sender, Effect effect) => SpawnEffectPrefabListeners.Invoke(sender, effect);

        #endregion Ability Events

        #region Combat Events

        public event EventHandler<DamageTakenEventArgs> DamageTakenListeners;
        public event EventHandler<DamageDealtEventArgs> DamageDealtListeners;
        public event EventHandler<HealingTakenEventArgs> HealingTakenListeners;

        public void FireDamageDealt(object sender, DamageDealtEventArgs args) => DamageDealtListeners?.Invoke(sender, args);
        public void FireDamageTaken(object sender, DamageTakenEventArgs args) => DamageTakenListeners.Invoke(sender, args);
        public void FireHealingTaken(object sender, HealingTakenEventArgs args) => HealingTakenListeners.Invoke(sender, args);
        public void FireAbilityEnded(object sender, AbilityEventArgs abilityArgs) => AbilityEndedListeners.Invoke(sender, abilityArgs);

        #endregion Combat Events

        #region Motion Events

        public event EventHandler<int> WallHitListeners; 
        public event EventHandler<AbilityEventArgs> VerticalMotionListeners;
        public event EventHandler<AbilityEventArgs> HorizontalMotionListeners;
        public event EventHandler<bool> RootListeners;
        public event EventHandler<bool> RotationRootListeners;
        public event EventHandler<AnimationEventArgs> GroundedListeners;

        public void FireVerticalMotion(object sender, AbilityEventArgs abilityArgs) => VerticalMotionListeners.Invoke(sender, abilityArgs);
        public void FireHorizontalMotion(object sender, AbilityEventArgs abilityArgs) => HorizontalMotionListeners.Invoke(sender, abilityArgs);
        public void FireRoot(object sender, bool isRooted) => RootListeners.Invoke(sender, isRooted);
        public void FireRotationRoot(object sender, bool isRotationRooted) => RotationRootListeners.Invoke(sender, isRotationRooted);
        public void FireGrounded(object sender, AnimationEventArgs isGrounded) => GroundedListeners?.Invoke(sender, isGrounded);
        public void FireWallHit(object sender, int placeholder) => WallHitListeners?.Invoke(sender, placeholder);

        #endregion Motion Events

        #region Unity Methods

        private void Start()
        {
            GlobalEvents.FireGameEventsAdded(this, gameObject.name);
        }

        private void OnDestroy()
        {
            GlobalEvents.FireGameEventsRemoved(this, gameObject.name);
        }

        #endregion Unity Methods
    }

    public class HealingTakenEventArgs
    {
        public int HealingAmount { get; set; }
        public bool IsCrit { get; set; }

        public HealingTakenEventArgs(int healingAmount, bool isCrit)
        {
            HealingAmount = healingAmount;
            IsCrit = isCrit;
        }
    }

    public class DamageTakenEventArgs
    {
        public int DamageAmount { get; set; }
        public bool IsCrit { get; set; }
        public bool IsMiss { get; set; }
        public bool IsHeal { get; set; }
        public bool IsHitFromBehind { get; set; }
        public AbilityData AbilityData { get; set; }
        public AbilityType DamageType { get; set; }

        public DamageTakenEventArgs(int damageAmount, bool isCrit, bool isMiss, bool isHeal,
            AbilityData abilityData, AbilityType damageType, bool isHitFromBehind = false)
        {
            DamageAmount = damageAmount;
            IsCrit = isCrit;
            IsMiss = isMiss;
            IsHeal = isHeal;
            AbilityData = abilityData;
            DamageType = damageType;
            IsHitFromBehind = isHitFromBehind;
        }
    }

    public class DamageDealtEventArgs
    {
        public AbilityData AbilityData { get; set; }
        public bool IsCrit;

        public DamageDealtEventArgs(AbilityData abilityData, bool isCrit = false)
        {
            AbilityData = abilityData;
            IsCrit = isCrit;
        }
    }

    public class AnimationEventArgs
    {
        public string AnimationParameterName { get; set; }
        public bool BoolValue { get; set; }
        public float FloatValue { get; set; }
        public int IntValue { get; set; }

        public AnimationEventArgs(string animationParameterName)
        {
            AnimationParameterName = animationParameterName;
        }

        public AnimationEventArgs(string animationParameterName, bool boolValue)
        {
            AnimationParameterName = animationParameterName;
            BoolValue = boolValue;
        }

        public AnimationEventArgs(string animationParameterName, float floatValue)
        {
            AnimationParameterName = animationParameterName;
            FloatValue = floatValue;
        }

        public AnimationEventArgs(string animationParameterName, int intValue)
        {
            AnimationParameterName = animationParameterName;
            IntValue = intValue;
        }
    }

    public class AbilityEventArgs
    {
        public string AbilityName { get; set; }
        public AnimationEventArgs AnimationParameters { get; set; }
        public bool IsUsable { get; set; }

        public AbilityEventArgs(string abilityName)
        {
            AbilityName = abilityName;
        }

        public AbilityEventArgs(string abilityName, bool isUsable)
        {
            AbilityName = abilityName;
            IsUsable = isUsable;
        }

        public AbilityEventArgs(string abilityName, AnimationEventArgs animationParameters) : this(abilityName)
        {
            AnimationParameters = animationParameters;
        }
    }
}
