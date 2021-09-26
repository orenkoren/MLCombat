using UnityEngine;
using System.Collections;
using MiddleAges.Events;
using System;

namespace MiddleAges.Animations
{
    public class AnimationTimeManager : MonoBehaviour
    {
        public Animator animator;

        private float currentSpeed;
        private bool isSlown;

        void Start()
        {
            GlobalEvents.timeSlowListeners += SlowDownCurrentlyPlayingAnimation;
        }

        private void Update()
        {
            if(animator && !isSlown && animator.GetFloat("AnimationSpeed") > 0.001f)
            {
                currentSpeed = animator.speed;
                animator.speed = animator.GetFloat("AnimationSpeed");
            }
        }

        private void SlowDownCurrentlyPlayingAnimation(object sender, TimeEventArgs eventArgs)
        {
            if (!animator) return;
            if (eventArgs.IsNormal)
            {
                animator.speed = 1;
                isSlown = false;
            }
            else
            {
                animator.speed = eventArgs.SlowOrRestoreAmount;
                isSlown = true;
            }
        }
    }
}