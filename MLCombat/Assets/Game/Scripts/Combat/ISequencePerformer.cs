using UnityEngine;

namespace MiddleAges.Combat
{
    public interface ISequencePerformer
    {
        void Perform(Animator animator, AbilityOriginAndResponse sequence);
    }
}