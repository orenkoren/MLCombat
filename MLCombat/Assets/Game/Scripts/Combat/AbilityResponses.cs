using MiddleAges.Database;
using MiddleAges.Events;
using MiddleAges.Motion;
using MiddleAges.Utils;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class AbilityResponses : MonoBehaviour
    {
        public AbilityOriginAndResponse[] Sequences;


        private Transform target;
        private GameEvents targetEvents;
        private Animator animator;
        private CooldownUtil cooldownUtil;
        private AIMovement movement;

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            cooldownUtil = GetComponent<CooldownUtil>();
            movement = GetComponent<AIMovement>();
            target = movement.GetDestinationTarget();
            if (target)
            {
                UpdateTarget();
            }
        }

        private void LateUpdate()
        {
            if (movement.GetDestinationTarget() != target)
            {
                UpdateTarget();
            }
        }

        private void UpdateTarget()
        {
            if (targetEvents)
                UnsubscribeToEvents();
            target = movement.GetDestinationTarget();
            if (!target) return;
            targetEvents = target.GetComponent<GameEvents>();
            targetEvents.AbilityTriggeredListeners += RespondToAbility;
            targetEvents.AbilityEndedListeners += FinishResponse;
        }

        private void RespondToAbility(object sender, AbilityEventArgs abilityArgs)
        {
            foreach (var sequence in Sequences)
            {
                if (abilityArgs.AbilityName == sequence.SourceAbility.name && cooldownUtil.IsAbilityReady(sequence.ResponseAbility.name))
                    animator.SetBool(sequence.TriggerName, true);

            }
        }

        private void FinishResponse(object sender, AbilityEventArgs abilityArgs)
        {
            foreach (var sequence in Sequences)
            {
                if (abilityArgs.AbilityName == sequence.SourceAbility.name)
                    animator.SetBool(sequence.TriggerName, false);

            }
        }


        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        private void UnsubscribeToEvents()
        {
            if (!targetEvents) return;
            targetEvents.AbilityTriggeredListeners -= RespondToAbility;
            targetEvents.AbilityEndedListeners -= FinishResponse;
        }
    }

    [System.Serializable]
    public class AbilityOriginAndResponse
    {
        public Ability SourceAbility;
        public Ability ResponseAbility;
        public string TriggerName;

        public AbilityOriginAndResponse(Ability sourceAbility, Ability respondWith)
        {
            SourceAbility = sourceAbility;
            ResponseAbility = respondWith;
        }
    }
}