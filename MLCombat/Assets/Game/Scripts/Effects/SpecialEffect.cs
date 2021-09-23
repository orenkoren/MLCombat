using MiddleAges.Database;
using UnityEngine;

namespace MiddleAges.SpecialEffects
{
    [System.Serializable]
    public class SpecialEffect
    {
        public GameObject EffectObject;
        public Vector3 EffectOffset;
        public EffectRotationType RotationType;
        public Vector3 EffectRotation;
        public Ability ForAbility;
        public EffectOrigin OriginPosition;
        public float StartRange;
        public float Lifetime;

    }
}