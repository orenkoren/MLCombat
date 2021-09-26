using MiddleAges.Events;
using MiddleAges.Sounds;
using MiddleAges.SpecialEffects;
using UnityEngine;

namespace MiddleAges.Animations
{
    public class AnimationEvents : MonoBehaviour
    {
        protected GameEvents events;

        #region Private Properties

        private SoundPlayer soundPlayer;

        #endregion Private Properties

        #region Unity Methods

        protected virtual void Start()
        {
            soundPlayer = GetComponent<SoundPlayer>();
            events = GetComponentInParent<GameEvents>();
        }

        #endregion Unity Methods

        #region Animation Events

        public void AnimationJump(string motionName)
        {
            events.FireVerticalMotion(this, new AbilityEventArgs(motionName));
        }

        public void AnimationThrust(string motionName)
        {
            events.FireHorizontalMotion(this, new AbilityEventArgs(motionName));
        }

        public void ApplyAbility(string abilityName)
        {
            events.FireAbilityExecuted(this, new AbilityEventArgs(abilityName));
        }

        public void PlaySoundFile(AudioClip clip) => soundPlayer.PlaySound(clip);

        public void SpawnEffectObject(Effect effect) => events.FireSpawnEffect(gameObject, effect);

        public void ApplyRootFromAnimation() => events.FireRoot(this, true);

        public void ReleaseRootFromAnimation() => events.FireRoot(this, false);

        public void ApplyRotationRootFromAnimation() => events.FireRotationRoot(this, true);

        public void ReleaseRotationRootFromAnimation() => events.FireRotationRoot(this, false);

        #endregion Animation Events

    }
}