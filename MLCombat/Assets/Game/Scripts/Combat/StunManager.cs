using MiddleAges.Entities;
using MiddleAges.Events;
using System;
using System.Collections;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class StunManager : MonoBehaviour
    {
        private GameEvents events;
        private Entity entity;
        private float stunDuration = 0;

        void Start()
        {
            events = GetComponent<GameEvents>();
            entity = GetComponent<Entity>();
            events.DamageTakenListeners += AddStunDuration;
        }

        private void AddStunDuration(object sender, DamageTakenEventArgs eventArgs)
        {
            if (eventArgs.AbilityData.IsStun)
                StartCoroutine(AddStunDuration(eventArgs.AbilityData.StunDuration));
        }

        IEnumerator AddStunDuration(float addedStunDuration)
        {
            if (stunDuration == 0)
            {
                FireStunStatus(true);
            }
            stunDuration += addedStunDuration;
            yield return new WaitForSeconds(addedStunDuration);
            stunDuration -= addedStunDuration;
            if (stunDuration <= 0)
            {
                stunDuration = 0;
                FireStunStatus(false);
            }
        }

        void FireStunStatus(bool isStunned)
        {
            entity.animator.SetBool("Stun", isStunned);
            events.FireStun(gameObject, isStunned);
        }

        private void OnDestroy()
        {
            events.DamageTakenListeners -= AddStunDuration;
        }
    }
}