using UnityEngine;
using System.Collections;
using MiddleAges.Database;
using MiddleAges.Events;
using System;

namespace MiddleAges.Combat
{
    public class AnimationResponses : MonoBehaviour
    {
        public Animator animator;
        public AnimationResponse[] responses;

        private GameEvents events;

        private void Start()
        {
            events = GetComponentInParent<GameEvents>();
            events.DamageTakenListeners += SetTrigger;
        }

        private void SetTrigger(object sender, DamageTakenEventArgs eventArgs)
        {
            foreach (var response in responses)
            {
                if (response.ability.name == eventArgs.AbilityData.Name)
                    animator.SetTrigger(response.AnimationTrigger);
            }
        }
    }

    [System.Serializable]
    public class AnimationResponse
    {
        public Ability ability;
        public string AnimationTrigger;
    }
}