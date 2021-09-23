using UnityEngine;

namespace MiddleAges.Combat
{
    public class AISequences : MonoBehaviour, ISequencePerformer
    {
        public void Perform(Animator animator, AbilityOriginAndResponse sequence)
        {
            animator.SetTrigger(sequence.TriggerName);
        }
    }
}