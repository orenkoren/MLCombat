using UnityEngine;
using System.Collections;
using MiddleAges.Events;
using System;

namespace MiddleAges.Motion
{
    public class MovementSpeedManager : MonoBehaviour
    {

        private Movement movement;

        private void Start()
        {
            movement = GetComponentInParent<Movement>();
            GlobalEvents.timeSlowListeners += HandleTimeSlow;
        }

        private void HandleTimeSlow(object sender, TimeEventArgs eventArgs)
        {
            if (eventArgs.IsNormal)
            {
                movement.RestoreSpeed(90);
            }
            else
            {
                movement.ApplySlow(90);
            }
        }
    }
}