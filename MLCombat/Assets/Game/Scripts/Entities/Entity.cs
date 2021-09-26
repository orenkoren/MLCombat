using MiddleAges.Events;
using UnityEngine;

namespace MiddleAges.Entities
{
    public class Entity : MonoBehaviour
    {
        public Animator animator;
        public LayerMask groundMask;
        public float groundDistance;
        public Transform groundCheck;
        public Rigidbody rb;
        public GameEvents events;

        protected void Start()
        {
            events.DeathListeners += ApplyDeath;
            events.ResurrectionListeners += ApplyRessurection;
        }

        private bool isAlive = true;

        public bool IsAlive() => isAlive;

        public virtual void ApplyDeath(object sender, AbilityEventArgs arg) => isAlive = false;

        public void ApplyRessurection(object sender, int placeHolder) => isAlive = true;


        private void OnDestroy()
        {
            events.DeathListeners -= ApplyDeath;
            events.ResurrectionListeners -= ApplyRessurection;
        }
    }
}
