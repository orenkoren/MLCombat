using UnityEngine;

namespace MiddleAges.SpecialEffects
{
    [CreateAssetMenu(fileName = "Effects", menuName = "Database/Effect", order = 0)]
    public class Effect : ScriptableObject
    {
        public SpecialEffect EffectInfo;
    }
}