using System.Collections.Generic;
using UnityEngine;

namespace MiddleAges.Database
{
    [System.Serializable]
    public class AbilityData
    {
        #region Basic
        public string Name;
        public AbilityType Type;
        #endregion Basic
        #region Common Fields
        #region Damage Fields
        public int BaseDamage;
        public float HitRadius;
        public float HitRadiusArcDegrees;
        public float Range;
        public float Cooldown;
        public int ApplySlow;
        public float SlowDuration;
        public int Dot;
        public float DotInterval;
        public float TickAmount;
        #endregion Damage Fields
        #region Channel
        public int DamageReduction;
        public int SelfSlow;
        public bool IsChanneled;
        #endregion Channel
        public string DamageText;
        public bool IsStun;
        public float StunDuration;
        public float VerticalForce;
        public float KnockbackTime;
        public float KnockbackPower;
        public List<AbilityConditionType> Conditions;
        #region Related Abilities
        public List<RelatedAbility> RelatedAbilities;
        #endregion Related Abilities
        #endregion Common Fields
        #region Player
        public Sprite Icon;
        public float RetCost;
        public float RetGain;
        public float SancCost;
        public float SancGain;
        public int ActionSlot;
        public KeyCode Keybind;
        public string Tooltip;
        #endregion Player
    }
}