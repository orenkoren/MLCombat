using MiddleAges.Database;
using MiddleAges.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class HitPauseManager : MonoBehaviour
    {
        public AbilityPause[] Pauses;

        private GameEvents events;
        private bool isStopActive;
        private Dictionary<string, AbilityPause> abilityToPause;

        private void Start()
        {
            InitializePauses();
            events = GetComponentInParent<GameEvents>();
            events.DamageDealtListeners += Hitpause;
        }

        private void InitializePauses()
        {
            abilityToPause = new Dictionary<string, AbilityPause>();
            foreach (var pause in Pauses)
            {
                abilityToPause.Add(pause.ability.name, pause);
            }
        }

        private void Hitpause(object sender, DamageDealtEventArgs eventArgs)
        {
            if (isStopActive) return;
            if (!abilityToPause.ContainsKey(eventArgs.AbilityData.Name)) return;
            var pauseData = abilityToPause[eventArgs.AbilityData.Name];
            if (pauseData.OnlyOnCrit && !eventArgs.IsCrit) return;

            GlobalEvents.FireTimeSlow(gameObject, new TimeEventArgs(false, pauseData.SlowDownTo));
            StartCoroutine(Hitpause(pauseData.PauseDuration, pauseData.SlowDownTo));
        }

        private IEnumerator Hitpause(float pauseDuration, float slowDownTo)
        {
            isStopActive = true;
            yield return new WaitForSecondsRealtime(pauseDuration);
            GlobalEvents.FireTimeSlow(gameObject, new TimeEventArgs(true, slowDownTo));
            isStopActive = false;
        }
    }

    [System.Serializable]
    public class AbilityPause
    {
        public Ability ability;
        public float PauseDuration;
        public float SlowDownTo;
        public bool OnlyOnCrit;
    }
}