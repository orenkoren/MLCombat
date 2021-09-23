using UnityEngine;

namespace MiddleAges.SpecialEffects
{
    [System.Serializable]
    public class EffectData
    {
        public EffectType Type; // TODO: readonly?
        public GameObject Effect;
        public Transform EffectLocation;
        public float Lifetime;
        
    }
}