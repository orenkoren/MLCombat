using MiddleAges.Entities;
using MiddleAges.Global;
using UnityEngine;

namespace MiddleAges.Motion
{
    public class AIMovement : Movement
    {
        private NPC npc;
        private float angle;
        private Vector3 directionToDestinationTarget;
        private bool isAggroed;
        private Transform destinationTarget;
        private Vector3 velocity;
        RaycastHit[] hits;

        protected override void Start()
        {
            base.Start();
            npc = GetComponent<NPC>();
        }

        protected override void FixedUpdate()
        {
            velocity = npc.agent.velocity;
            if (!entity.IsAlive()) return;
            if (destinationTarget && !destinationTarget.GetComponent<Entity>().IsAlive())
                destinationTarget = null;
            base.FixedUpdate();
            Vector3 agentPosition = npc.agent.transform.position;
            isAggroed = Physics.CheckSphere(agentPosition, npc.aggroRange, npc.hostileMask);
            if (isAggroed)
            {
                if (IsNotRooted() && !isMovingExternally)
                {
                    float speedValue = OriginalSpeed * (SpeedModifiers / 100);
                    ChangeVelocity(speedValue);
                }
                else if (!isMovingExternally)
                {
                    npc.agent.velocity = new Vector3(0, npc.agent.velocity.y, 0);
                }
                hits = Physics.SphereCastAll(agentPosition, npc.aggroRange, new Vector3(0, 1, 0), 0.1f, npc.hostileMask);
                if (hits.Length > 0 && destinationTarget == null)
                    destinationTarget = hits[0].transform;
                for (var i = 0; i < hits.Length; i++)
                {
                    if (Vector3.Distance(hits[i].transform.position, transform.position) <
                        Vector3.Distance(destinationTarget.position, transform.position))
                    {
                        destinationTarget = hits[i].transform;
                    }
                    if (npc.agent.enabled == true)
                        npc.agent.SetDestination(destinationTarget.position);
                    directionToDestinationTarget = (destinationTarget.position - transform.position).normalized;
                }
            }
            else
            {
                destinationTarget = null;
            }

            if (destinationTarget && !IsRotationRooted())
            {
                angle = Mathf.Atan2(directionToDestinationTarget.x, directionToDestinationTarget.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            PlayMotionAnimations(Mathf.Clamp(new Vector3(npc.agent.velocity.x, 0, npc.agent.velocity.z).normalized.magnitude -
                                (100 - SpeedModifiers) / 100, 0, 1f));
        }

        public Transform GetDestinationTarget() => destinationTarget;

        public override void MoveWithSpeedIgnoringEverything(float forwardSpeed)
        {
            if (destinationTarget && ((Vector3.Distance(destinationTarget.position, transform.position) > npc.agent.stoppingDistance) || forwardSpeed < 0))
            {
                float speedValue = forwardSpeed * (SpeedModifiers / 100);
                ChangeVelocity(speedValue);
                
            }
            else
                npc.agent.velocity = new Vector3(0, npc.agent.velocity.y, 0);
        }

        private void ChangeVelocity(float speedValue)
        {
            npc.agent.velocity = new Vector3(transform.forward.x * speedValue * Time.deltaTime, npc.agent.velocity.y,
                                                transform.forward.z * speedValue * Time.deltaTime);
        }
    }
}
