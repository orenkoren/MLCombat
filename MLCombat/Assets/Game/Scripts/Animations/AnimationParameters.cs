using MiddleAges.Events;
using System.Collections.Generic;
using UnityEngine;

namespace MiddleAges.Animations
{
    public class AnimationParameters : AnimationEvents
    {
        public AnimationTracker[] TrackAnimations;

        private Dictionary<string, AnimationTracker> stateTagToTrackingData;
        private Animator animator;
        private bool hasTriggered;
        private bool hasEnded;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected override void Start()
        {
            base.Start();
            stateTagToTrackingData = new Dictionary<string, AnimationTracker>();
            foreach (var tracker in TrackAnimations)
            {
                stateTagToTrackingData.Add(tracker.StateTag, tracker);
            }
        }

        public void OnAnimatorMove()
        {
            if (animator.IsInTransition(0) || animator.IsInTransition(1))
                UpdateParameterValues();
            else
            {
                hasTriggered = false;
                hasEnded = false;
            }
        }

        private void UpdateParameterValues()
        {
            foreach (var tag in stateTagToTrackingData.Keys)
            {
                var trackingData = stateTagToTrackingData[tag];
                if (hasTriggered == false && 
                    (animator.GetNextAnimatorStateInfo(0).IsTag(tag) || animator.GetNextAnimatorStateInfo(1).IsTag(tag)))
                {
                    hasTriggered = true;
                    if (trackingData.AbilityName != "")
                    {
                        events.FireAbilityTriggered(gameObject, new AbilityEventArgs(trackingData.AbilityName));
                    }
                    if (trackingData.ShouldRoot)
                    {
                        events.FireRoot(gameObject, true);
                    }
                    if (trackingData.ShouldRotationRoot)
                        events.FireRotationRoot(gameObject, true);
                }
            }
            foreach (var tag in stateTagToTrackingData.Keys)
            {
                var trackingData = stateTagToTrackingData[tag];
                if (hasEnded == false && 
                    (animator.GetCurrentAnimatorStateInfo(0).IsTag(tag)) || animator.GetCurrentAnimatorStateInfo(1).IsTag(tag))
                {
                    hasEnded = true;
                    if (trackingData.AbilityName != "")
                        events.FireAbilityEnded(gameObject, new AbilityEventArgs(trackingData.AbilityName));
                    if (trackingData.ShouldRoot)
                    {
                        events.FireRoot(gameObject, false);
                    }
                    if (trackingData.ShouldRotationRoot)
                        events.FireRotationRoot(gameObject, false);
                }
            }            
        }
    }
    [System.Serializable]
    public class AnimationTracker
    {
        public string AbilityName;
        public string StateTag;
        public bool ShouldRoot;
        public bool ShouldRotationRoot;

        public AnimationTracker(string abilityName, string stateBoolean, bool shouldRoot, bool shouldRotationRoot)
        {
            AbilityName = abilityName;
            StateTag = stateBoolean;
            ShouldRoot = shouldRoot;
            ShouldRotationRoot = shouldRotationRoot;
        }
    }
}