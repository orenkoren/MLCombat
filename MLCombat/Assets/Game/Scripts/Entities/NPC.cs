using MiddleAges.Events;
using UnityEngine;
using UnityEngine.AI;

namespace MiddleAges.Entities
{
    public class NPC : Entity
    {
        public NavMeshAgent agent;
        public float aggroRange;
        public LayerMask hostileMask;
        public int timeToDespawn;

        public override void ApplyDeath(object sender, AbilityEventArgs arg)
        {
            base.ApplyDeath(sender, arg);
            Destroy(gameObject, timeToDespawn);
        }
    }
}
