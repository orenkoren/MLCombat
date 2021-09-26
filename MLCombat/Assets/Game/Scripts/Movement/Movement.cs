using MiddleAges.Database;
using MiddleAges.Entities;
using MiddleAges.Events;
using MiddleAges.Global;
using MiddleAges.Saving;
using System;
using System.Collections;
using UnityEngine;

namespace MiddleAges.Motion
{
    public class Movement : MonoBehaviour, ISaveable
    {
        public float OriginalSpeed;
        public GameEvents events;
        [HideInInspector]
        public bool isForceAppliedExternally;

        protected Abilities abilities;
        protected float SpeedModifiers = 100f;
        protected Vector3 moveDirection;
        protected Entity entity;
        protected bool isMovingExternally;

        protected bool isGrounded = false;
        private bool isNotRooted = true;
        private bool isRotationRooted = false;

        protected virtual void Start()
        {
            entity = GetComponent<Entity>();
            abilities = GetComponent<Abilities>();
            events.AbilityExecutedListeners += ApplySlow;
            events.DamageTakenListeners += ApplySlow;
            events.AbilityEndedListeners += RestoreSpeed;
            events.VerticalMotionListeners += Jump;
            events.RootListeners += ApplyRoot;
            events.RotationRootListeners += ApplyRotationRoot;
            events.StunStateListeners += DisableMovement;
            //events.DeathListeners += ChangeToDeathLayer;
            //events.ResurrectionListeners += RestoreLayer;
            events.GroundedListeners += ChangeGroundedState;
        }

        protected virtual void FixedUpdate()
        {
            float attackMoveSpeed = entity.animator.GetFloat("ThrustSpeed");
            float retreatSpeed = entity.animator.GetFloat("RetreatSpeed");
            if (attackMoveSpeed > 0)
                MoveWithSpeedIgnoringEverything(attackMoveSpeed * OriginalSpeed);
            if (retreatSpeed > 0)
                MoveWithSpeedIgnoringEverything(retreatSpeed * -1 * OriginalSpeed);
            if (attackMoveSpeed > 0 || retreatSpeed > 0 || isForceAppliedExternally)
                isMovingExternally = true;
            else
                isMovingExternally = false;
        }

        public void ApplySlow(object sender, AbilityEventArgs abilityArgs)
        {
            ApplySlow(abilities.GetAbility(abilityArgs.AbilityName).SelfSlow);
        }

        public void ApplySlow(object sender, DamageTakenEventArgs eventArgs)
        {
            if (eventArgs.AbilityData.ApplySlow > 0)
                StartCoroutine(ApplyPeriodicSlow(eventArgs.AbilityData));
        }

        public void RestoreSpeed(object sender, AbilityEventArgs abilityArgs)
        {
            RestoreSpeed(abilities.GetAbility(abilityArgs.AbilityName).SelfSlow);
        }

        public void ApplySlow(int slowPercent)
        {
            int inversePercentage = 100 - slowPercent;
            if (SpeedModifiers > inversePercentage)
                SpeedModifiers = inversePercentage;
        }

        public void RestoreSpeed(int increasePercent)
        {
            int inversePercentage = 100 - increasePercent;
            if (SpeedModifiers >= inversePercentage)
                SpeedModifiers = 100;
        }

        public bool IsGrounded() => isGrounded;

        public virtual void ApplyRoot(object sender, bool isRooted)
        {
            isNotRooted = !isRooted;
        }

        public void ApplyRotationRoot(object sender, bool isRotationRooted) => this.isRotationRooted = isRotationRooted;

        public bool IsNotRooted() => isNotRooted;

        public bool IsRotationRooted() => isRotationRooted;

        public virtual void MoveWithSpeedIgnoringEverything(float forwardSpeed)
        {
            entity.rb.velocity = new Vector3(moveDirection.x * forwardSpeed * Time.deltaTime, entity.rb.velocity.y,
                                                moveDirection.z * forwardSpeed * Time.deltaTime);
        }

        public void PlayMotionAnimations(float forwardSpeed)
        {
            entity.animator.SetFloat("forwardSpeed", forwardSpeed);
        }

        public virtual void Jump(object sender, AbilityEventArgs abilityArgs)
        {
            if (IsGrounded())
            {
                AbilityData abilityData = abilities.GetAbility(abilityArgs.AbilityName);
                entity.rb.velocity = new Vector3(entity.rb.velocity.x, abilityData.VerticalForce, entity.rb.velocity.z);
            }
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            // think about AI saving.. disable navmeshagent if we want to move them.
            transform.position = position.ToVector();
        }

        private void ChangeGroundedState(object sender, AnimationEventArgs eventArgs)
        {
            isGrounded = eventArgs.BoolValue;
        }

        private void DisableMovement(object sender, bool isStunned)
        {
            enabled = !isStunned;
        }

        private void ChangeToDeathLayer(object sender, AbilityEventArgs e)
        {
            gameObject.layer = LayerMask.NameToLayer("Dead");
        }

        private void RestoreLayer(object sender, int e)
        {
            ((GameObject)sender).layer = LayerMask.NameToLayer("");
        }

        IEnumerator ApplyPeriodicSlow(AbilityData abilityData)
        {
            ApplySlow(abilityData.ApplySlow);
            yield return new WaitForSeconds(abilityData.SlowDuration);
            RestoreSpeed(abilityData.ApplySlow);
        }

        private void OnDestroy()
        {
            events.AbilityExecutedListeners -= ApplySlow;
            events.AbilityEndedListeners -= RestoreSpeed;
            events.VerticalMotionListeners -= Jump;
            events.RootListeners -= ApplyRoot;
            events.RotationRootListeners -= ApplyRotationRoot;
            events.StunStateListeners -= DisableMovement;
            //events.DeathListeners -= ChangeToDeathLayer;
            //events.ResurrectionListeners -= RestoreLayer;
        }
    }
}
