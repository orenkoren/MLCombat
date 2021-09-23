using UnityEngine;
using System.Collections;
using MiddleAges.Events;
using System;
using MiddleAges.Motion;

namespace MiddleAges.Combat
{
    public class ApplyForceManager : MonoBehaviour
    {
        private Rigidbody rb;
        private GameEvents events;
        private Movement movement;

        void Start()
        {
            rb = GetComponentInParent<Rigidbody>();
            events = GetComponentInParent<GameEvents>();
            movement = GetComponentInParent<Movement>();
            events.DamageTakenListeners += ApplyForce;
        }

        private void ApplyForce(object sender, DamageTakenEventArgs eventArgs)
        {
            ApplyKnockback(eventArgs.AbilityData.KnockbackTime, eventArgs.AbilityData.KnockbackPower);
        }

        private void ApplyKnockback(float forceTime, float power)
        {
            if (forceTime == 0) return;
            StartCoroutine(ApplyBackwardVelocity(forceTime, power));
        }

        IEnumerator ApplyBackwardVelocity(float forceTime, float power)
        {
            float time = 0;
            movement.isForceAppliedExternally = true;
            while (time < forceTime)
            {
                movement.MoveWithSpeedIgnoringEverything(power * -1);
                time += Time.deltaTime;
                yield return null;
            }
            movement.isForceAppliedExternally = false;
        }
    }
}