using MiddleAges.Events;
using UnityEngine;

namespace Assets.Game.Scripts.PersonalCanvas
{
    public class FloatingTextAnimationHandler : MonoBehaviour
    {
        private Animation floatAnimation;

        void Awake()
        {
            GlobalEvents.timeSlowListeners += SlowAnimation;
            floatAnimation = GetComponent<Animation>();
        }

        private void SlowAnimation(object sender, TimeEventArgs eventArgs)
        {
            if (!floatAnimation) return;
            if (eventArgs.IsNormal)
            {
                floatAnimation[floatAnimation.clip.name].speed = 1;
            }
            else
            {
                floatAnimation[floatAnimation.clip.name].speed = eventArgs.SlowOrRestoreAmount;
            }
        }
    }
}