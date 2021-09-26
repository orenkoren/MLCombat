using MiddleAges.Database;
using MiddleAges.Motion;
using UnityEngine;

namespace MiddleAges.Combat
{
    public class AICombat : CombatCore
    {
        public Animator animator;

        #region Private Properties

        private AIMovement AImovement;
        private Transform destinationTarget;
        private float distanceFromTarget;

        #endregion Private Properties

        #region Unity Methods

        protected override void Start()
        {
            base.Start();
            AImovement = (AIMovement)movement;
        }

        private void Update()
        {
            destinationTarget = AImovement.GetDestinationTarget();
            if (destinationTarget)
                distanceFromTarget = Vector3.Distance(destinationTarget.position, transform.position);
            else
                distanceFromTarget = 999;
            animator.SetFloat("DistanceFromTarget", distanceFromTarget);
        }

        public float GetDistanceFromTarget() => distanceFromTarget;

        protected override RaycastHit[] RangeAbilityRaycast(AbilityData abilityData)
        {
            return Physics.SphereCastAll(origin: weapon.position,
                                         radius: abilityData.HitRadius,
                                         direction: transform.forward,
                                         maxDistance: abilityData.Range,
                                         layerMask: hitableThings);
        }

        #endregion Unity Methods
    }
}
