using UnityEngine;
using System.Collections;
using MiddleAges.Events;
using System;

namespace MiddleAges.Global
{
    public class TimeManager : MonoBehaviour
    {
        private static float currentTimeScale = 1.0f;

        private void Start()
        {
            GlobalEvents.timeSlowListeners += ChangeCurrentTimeScale;
        }

        public static float GetCurrentTimeScale() => currentTimeScale;

        private void ChangeCurrentTimeScale(object sender, TimeEventArgs eventArgs)
        {
            if (eventArgs.IsNormal)
                currentTimeScale = 1.0f;
            else
                currentTimeScale = eventArgs.SlowOrRestoreAmount;
        }
    }
}