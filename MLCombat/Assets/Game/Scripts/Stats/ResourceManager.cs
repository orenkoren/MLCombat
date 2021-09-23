using UnityEngine;
using System.Collections;
using MiddleAges.Events;
using System;

namespace MiddleAges.Resources
{
    public class ResourceManager : MonoBehaviour
    {
        private GameEvents events;


        // Use this for initialization
        void Start()
        {
            events = GetComponentInParent<GameEvents>();
            events.DamageDealtListeners += UpdateResource;
        }

        private void UpdateResource(object sender, DamageDealtEventArgs eventArgs)
        {
            var abilityData = eventArgs.AbilityData;
            
        }
    }
}