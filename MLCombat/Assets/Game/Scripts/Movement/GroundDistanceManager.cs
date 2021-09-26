using MiddleAges.Entities;
using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Motion
{
    public class GroundDistanceManager : MonoBehaviour
    {
        private Entity entity;
        private GameEvents events;
        private bool isGrounded = false;

        private void Start()
        {
            events = GetComponentInParent<GameEvents>();
            entity = GetComponentInParent<Entity>();
        }

        private void FixedUpdate()
        {
            bool checkGrounded = Physics.CheckSphere(entity.groundCheck.position, entity.groundDistance, entity.groundMask);
            if (checkGrounded != isGrounded)
            {
                isGrounded = checkGrounded;
                events.FireGrounded(this, new AnimationEventArgs("IsGrounded", isGrounded));
            }
        }

    }
}