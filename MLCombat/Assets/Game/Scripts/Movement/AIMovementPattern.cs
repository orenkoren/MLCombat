using MiddleAges.Entities;
using UnityEngine;

namespace MiddleAges.Motion
{
    public class AIMovementPattern : MonoBehaviour
    {
        public float maintainDistance;

        private float currentDistance;
        private NPC npc;
        private AIMovement movement;

        private void Start()
        {
            npc = GetComponentInParent<NPC>();
            movement = GetComponentInParent<AIMovement>();
        }

        private void Update()
        {
            
        }

    }
}